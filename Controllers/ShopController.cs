using LexiElectronics.Data;

using LexiElectronics.Models;
using LexiElectronics.Models.DTOs;
using LexiElectronics.Models.ViewModels;
using LexiElectronics.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LexiElectronics.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext appDbContext;
        private const string SessionKeyCart = "ShoppingCart";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPaymentService paymentService;

        public ShopController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPaymentService paymentService)
        {
            appDbContext = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {            
            var products = await appDbContext.Products.Where(p => p.VisibleInShop).AsNoTracking().ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) 
                return NotFound();

            product.Quantity = 1;

            return View(product);
        }


        public IActionResult ShoppingCart()
        {
     
            ShoppingCartDto cart = GetCartFromSession();

            return View(cart);
        }

        private ShoppingCartDto GetCartFromSession()
        {
            var json = HttpContext.Session.GetString(SessionKeyCart);
            return string.IsNullOrEmpty(json) ? new ShoppingCartDto() : System.Text.Json.JsonSerializer.Deserialize<ShoppingCartDto>(json)!;
        }

        private void SaveCartToSession(ShoppingCartDto cart)
        {
            HttpContext.Session.SetString(SessionKeyCart, System.Text.Json.JsonSerializer.Serialize(cart));
        }   

        public IActionResult AddToCart(int productId, int quantity)
        {
            var cart = GetCartFromSession();

            var product = appDbContext.Products.FirstOrDefault(p => p.Id == productId);

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                cart.CartItems.Add(new CartItemDto(productId, product.Name, product.Price, quantity));
            else
                item.Quantity++;

            SaveCartToSession(cart);

            return RedirectToAction("ShoppingCart");
        }

        public IActionResult RemoveCartItem(int productId)
        {
            ShoppingCartDto cart = GetCartFromSession();
            CartItemDto cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            
            cart.CartItems.Remove(cartItem);

            SaveCartToSession(cart);

            return RedirectToAction("ShoppingCart"); ;
        }


        public IActionResult Checkout()
        {

            if (signInManager.IsSignedIn(User))
            {

                ShoppingCartDto shoppingCartDto = new ShoppingCartDto();

                shoppingCartDto = GetCartFromSession();

                CheckoutVM checkoutVM = new CheckoutVM(shoppingCartDto);

                var user = appDbContext.ApplicationUsers.Find(userManager.GetUserId(User));

                if (string.IsNullOrEmpty(user.DeliveryName) || string.IsNullOrEmpty(user.DeliveryStreetAddress) || string.IsNullOrEmpty(user.DeliveryZipcode)
                    || string.IsNullOrEmpty(user.DeliveryCity))
                {
                    checkoutVM.DeliveryAddressExists = false;
                    checkoutVM.ReturnUrl ??= Url.Content("~/");
                }
                else
                {
                    checkoutVM.DeliveryAddressExists = true;
                }

                checkoutVM.User = user;

                // Beräkna fraktkostnaden och totalkostnaden som kunden ska betala.
                if (checkoutVM.TotalSum < 500)
                {
                    checkoutVM.FreightSum = 59;
                    checkoutVM.TotalSumToPay = checkoutVM.TotalSum + checkoutVM.FreightSum;
                }
                else
                {
                    checkoutVM.FreightSum =0;
                    checkoutVM.TotalSumToPay = checkoutVM.TotalSum;
                }

                return View(checkoutVM);
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Checkout", "Shop") });
            }
        }

        public async Task<IActionResult>SendOrder()
        {
            ShoppingCartDto shoppingCartDto = GetCartFromSession();

            CheckoutVM checkoutVM = new CheckoutVM(shoppingCartDto);

            var user = appDbContext.ApplicationUsers.Find(userManager.GetUserId(User));



            Order order = new Order();

            order.UserIdStr = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Mottagen";
            order.TotalSum = checkoutVM.TotalSum;
            
            if (order.TotalSum < 500)
                order.FreightSum = 59;
            else
                order.FreightSum = 0;

            appDbContext.Orders.Add(order);
            await appDbContext.SaveChangesAsync();

            order.OrderItems = new List<OrderItem>();

            foreach(CartItemDto cartItem in shoppingCartDto.CartItems)
            {
                var orderItem = new OrderItem(order.Id, cartItem.ProductId, cartItem.Quantity, cartItem.Price);
                order.OrderItems.Add(orderItem);
            }


            appDbContext.OrderItems.AddRange(order.OrderItems);
            await appDbContext.SaveChangesAsync();

            // Placeholder for payment - using payment service
            var paymentResult = await paymentService.ProcessPaymentAsync(order);

            checkoutVM.PaymentResult = paymentResult.Success;

            return RedirectToAction("CheckoutFinished", checkoutVM);
        }

        public IActionResult CheckoutFinished(CheckoutVM checkoutVM)
        {
            ShoppingCartDto cart = GetCartFromSession();
            cart.CartItems = new List<CartItemDto>();
            SaveCartToSession(cart);

            return View(checkoutVM);
        }

        public async Task<IActionResult> MyOrders()
        {   
            var user = await userManager.GetUserAsync(User);
            List<Order> orders = await appDbContext.Orders.Where(o => o.UserIdStr == user.Id).ToListAsync();
            
            foreach(Order order in orders) {
                order.OrderItems = await appDbContext.OrderItems.Where(oi => oi.OrderId == order.Id).ToListAsync();
            }
            return View(orders);
        }

        public async Task<IActionResult> ViewOrder(int id)
        {
            
            Order order = await appDbContext.Orders.FindAsync(id);

            order.User = await userManager.GetUserAsync(User);

            order.OrderItems = await appDbContext.OrderItems.Where(oi => oi.OrderId == order.Id).ToListAsync();

            foreach (OrderItem item in order.OrderItems)
            {
                item.Product = await appDbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                item.Sum = item.Quantity * item.Price;
            }
            return View(order);
        }
    }
}
