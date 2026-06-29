using LexiElectronics.Data;
using LexiElectronics.Models;
using LexiElectronics.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LexiElectronics.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext appDbContext;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IWebHostEnvironment webHostEnv;

        private Order order;


        public AdminController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, 
                               RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnv)
        {
            appDbContext = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.webHostEnv = webHostEnv;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighhet.
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                // List<Order> orders = appDbContext.Orders.Where(o => o.Status == "Skickad").ToList();
                List<Order> orders = appDbContext.Orders.ToList();

                List<ApplicationUser> users = appDbContext.ApplicationUsers.ToList();

                foreach (var order in orders)
                {
                    var user1 = users.FirstOrDefault(u => u.Id == order.UserIdStr);
                    if (user1 != null)
                    {
                        order.User = user1;
                    }
                }

                return View(orders);
            }
        }


        public async Task<IActionResult> ViewAndEditOrder(int id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                order = appDbContext.Orders.FirstOrDefault(o => o.Id == id);
                order.OrderItems = appDbContext.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();

                foreach (OrderItem item in order.OrderItems)
                {
                    item.Product = appDbContext.Products.Find(item.ProductId);
                }

                order.User = appDbContext.ApplicationUsers.Find(order.UserIdStr);

                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }

        }

        public async Task<IActionResult> AllProducts()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<Product> allProducts = appDbContext.Products.ToList();

                List<OrderItem> orderItems = appDbContext.OrderItems.ToList();

                foreach (Product product in allProducts)
                {
                    product.Category = appDbContext.ProductCategories.Find(product.ProductCategoryId);

                    foreach (OrderItem item in orderItems)
                    {
                        if (product.Id == item.ProductId)
                        {
                            product.nbrOfItemsSold = product.nbrOfItemsSold + item.Quantity;
                        }
                    }
                }

                allProducts = allProducts.OrderBy(p => p.Category.Name).ThenBy(p => p.Price).ToList();

                return View(allProducts);

            }
        }

        public async Task<IActionResult> ShowProduct(int Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                Product product = appDbContext.Products.Find(Id);
                product.Category = appDbContext.ProductCategories.Find(product.ProductCategoryId);
                product.Manufacturer = appDbContext.Manufacturers.Find(product.ManufacturerId);

                List<OrderItem> orderItems = appDbContext.OrderItems.ToList();

                foreach (OrderItem item in orderItems)
                {
                    if (product.Id == item.ProductId)
                    {
                        product.nbrOfItemsSold = product.nbrOfItemsSold + item.Quantity;
                    }
                }

                return View(product);
            }
        }
        public async Task<IActionResult> CreateProduct()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<ProductCategory> productCategories = appDbContext.ProductCategories.ToList();
                List<SelectListItem> productCategoryListItems = new List<SelectListItem>();

                foreach (ProductCategory productCategory in productCategories)
                {

                    SelectListItem item = new SelectListItem() { Value = productCategory.Id.ToString(), Text = productCategory.Name };
                    productCategoryListItems.Add(item);
                }

                List<Manufacturer> manufacturers = appDbContext.Manufacturers.ToList();
                List<SelectListItem> manufacturerListItems = new List<SelectListItem>();

                foreach (Manufacturer manufacturer in manufacturers)
                {
                    SelectListItem item = new SelectListItem() { Value = manufacturer.Id.ToString(), Text = manufacturer.BrandName };
                    manufacturerListItems.Add(item);
                }

                ProductVM productVM = new ProductVM(productCategoryListItems, manufacturerListItems);

                return View(productVM);
            }
        }

        public async Task<IActionResult> SaveCreatedProduct([Bind("Id, Name, ArticleNo, ShortDescription, LongDescription, Price,Stock, ImageFile," +
                                                                  "ImageFilename, ProductCategoryId, ManufacturerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var uploads = Path.Combine(webHostEnv.WebRootPath, "images");

                    var filePath = Path.Combine(uploads, product.ImageFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    product.ImageFilename = product.ImageFile.FileName;
                }


                appDbContext.Products.Add(product);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllProducts));
            }
            else
            {
                ProductVM productVM = new ProductVM(product);
                return View("CreateProduct", productVM);
            }
        }

        public async Task<IActionResult> EditProduct(int Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<ProductCategory> productCategories = appDbContext.ProductCategories.ToList();
                List<SelectListItem> productCategoryListItems = new List<SelectListItem>();

                foreach (ProductCategory productCategory in productCategories)
                {
                    SelectListItem item = new SelectListItem() { Value = productCategory.Id.ToString(), Text = productCategory.Name };
                    productCategoryListItems.Add(item);
                }

                List<Manufacturer> manufacturers = appDbContext.Manufacturers.ToList();
                List<SelectListItem> manufacturerListItems = new List<SelectListItem>();

                foreach (Manufacturer manufacturer in manufacturers)
                {
                    SelectListItem item = new SelectListItem() { Value = manufacturer.Id.ToString(), Text = manufacturer.BrandName };
                    manufacturerListItems.Add(item);
                }

                Product product = appDbContext.Products.Find(Id);

                ProductVM productVM = new ProductVM(product);
                productVM.ProductCategoryListItems = productCategoryListItems;
                productVM.ManufacturerListItems = manufacturerListItems;

                return View(productVM);
            }
        }

        public async Task<IActionResult> SaveEditedProduct([Bind("Id,Name, ArticleNo, ShortDescription, LongDescription, Price, Stock, ImageFile," +
                                                                 "ImageFilename, ProductCategoryId, ManufacturerId, VisibleInShop")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var uploads = Path.Combine(webHostEnv.WebRootPath, "images");

                    var filePath = Path.Combine(uploads, product.ImageFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    product.ImageFilename = product.ImageFile.FileName;
                }

                var existingProduct = await appDbContext.Products.FindAsync(product.Id);

                if (existingProduct == null)
                    return NotFound();


                appDbContext.Entry(existingProduct).CurrentValues.SetValues(product);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllProducts));
            }
            else
            {
                ProductVM productVM = new ProductVM(product);
                return View("EditProduct", productVM);
            }
        }



        public async Task<IActionResult> AllProductCategories()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<ProductCategory> allProductCategories = appDbContext.ProductCategories.ToList();

                foreach (ProductCategory productCategory in allProductCategories)
                {
                    productCategory.Products = appDbContext.Products.Where(p => p.ProductCategoryId == productCategory.Id).ToList();
                }

                allProductCategories = allProductCategories.OrderBy(p => p.Name).ToList();

                return View(allProductCategories);

            }
        }

        public async Task<IActionResult> CreateProductCategory()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                ProductCategory productCategory = new ProductCategory();

                return View(productCategory);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCreatedProductCategory(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                appDbContext.ProductCategories.Add(productCategory);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllProductCategories));
            }
            else
            {
                return RedirectToAction(nameof(CreateProductCategory));
            }
        }

        public async Task<IActionResult> EditProductCategory(int Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                ProductCategory productCategory = appDbContext.ProductCategories.Find(Id);

                return View(productCategory);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditedProductCategory([Bind("Id,Name,Description")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                appDbContext.ProductCategories.Update(productCategory);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllProductCategories));
            }
            else
            {
                return RedirectToAction(nameof(CreateProductCategory));
            }
        }

        public async Task<IActionResult> RemoveProductCategory(int Id)
        {

            ProductCategory productCategory = appDbContext.ProductCategories.Find(Id);

            if (productCategory != null)
            {
                productCategory.Products = appDbContext.Products.Where(p => p.ProductCategoryId == productCategory.Id).ToList();

                if (productCategory.Products.Count == 0)
                {
                    appDbContext.ProductCategories.Remove(productCategory);

                    await appDbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AllProductCategories));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrder(int? Id, [Bind("Id, UserIdStr, User, OrderDate, TotalSum,Status,OrderItems")] Order order)
        {
            Order dbOrder = appDbContext.Orders.FirstOrDefault(o => o.Id == order.Id);
            dbOrder.OrderItems = appDbContext.OrderItems.Where(oi => oi.OrderId == dbOrder.Id).ToList();

            int totalSum = 0;
            foreach (OrderItem item in order.OrderItems ?? Enumerable.Empty<OrderItem>())
            {
                var dbItem = dbOrder.OrderItems.FirstOrDefault(i => i.Id == item.Id);
                if (dbItem != null && item.Quantity != dbItem.Quantity)
                {
                    dbItem.Quantity = item.Quantity;
                    appDbContext.OrderItems.Update(dbItem);

                }

                if (dbItem != null)
                {
                    totalSum += dbItem.Quantity * dbItem.Price;
                }
            }

            dbOrder.Status = order.Status;
            dbOrder.TotalSum = totalSum;

            if (dbOrder.TotalSum < 500)
            {
                dbOrder.FreightSum = 59;
            }
            else
            {
                dbOrder.FreightSum = 0;
            }

            appDbContext.Orders.Update(dbOrder);

            await appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveOrder(int orderId)
        {
            var order = appDbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                order.OrderItems = appDbContext.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();
                appDbContext.Orders.Remove(order);
                appDbContext.OrderItems.RemoveRange(order.OrderItems);
                await appDbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveOrderItem(int orderId, int orderItemId)
        {
            var order = appDbContext.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.OrderItems = appDbContext.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();
                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);

                if (orderItem != null)
                {
                    order.TotalSum = order.TotalSum - (orderItem.Quantity * orderItem.Price);

                    order.OrderItems.Remove(orderItem);
                    appDbContext.OrderItems.Remove(orderItem);

                    appDbContext.Orders.Update(order);

                    await appDbContext.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(ViewAndEditOrder), new { id = order.Id });
        }

        public async Task<IActionResult> AllUsers()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<ApplicationUser> allUsers = appDbContext.ApplicationUsers.ToList();

                foreach (ApplicationUser user in allUsers)
                {
                    var roles = await userManager.GetRolesAsync(user); // List<string>
                    user.UserRoleName = roles[0];
                }

                return View(allUsers);
            }
        }

        public async Task<IActionResult> ShowUser(string Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                ApplicationUser user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == Id);
                
                var roles = await userManager.GetRolesAsync(user); // List<string>
                user.UserRoleName = roles[0];

                return View(user);
            }
        }

        public async Task<IActionResult> EditUser(string Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                ApplicationUser user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == Id);

                user.UserRoleListItems = roleManager.Roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

                var roles = await userManager.GetRolesAsync(user); // List<string>
                user.UserRoleName = roles[0];

                var role = await roleManager.FindByNameAsync(user.UserRoleName);
                user.UserRoleId = role.Id;

                return View(user);
            }
        }


        public async Task<IActionResult> SaveEditedUser([Bind("Id, Firstname, Lastname, Email, PhoneNumber, InvoiceName, InvoiceStreetAddress, InvoiceZipcode, " +
                                                              "InvoiceCity, DeliveryName, DeliveryStreetAddress, DeliveryZipcode, DeliveryCity, PreventAccess, UserRoleId")] 
                                                              ApplicationUser user)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
           
            var dbUser = appDbContext.ApplicationUsers.Find(user.Id);

            dbUser.Firstname = user.Firstname;
            dbUser.Lastname = user.Lastname;
            dbUser.Email = user.Email;
            dbUser.PhoneNumber = user.PhoneNumber;
            dbUser.InvoiceName = user.InvoiceName;
            dbUser.InvoiceStreetAddress = user.InvoiceStreetAddress;
            dbUser.InvoiceZipcode = user.InvoiceZipcode;
            dbUser.InvoiceCity = user.InvoiceCity;
            dbUser.DeliveryName = user.DeliveryName;
            dbUser.DeliveryStreetAddress = user.DeliveryStreetAddress;
            dbUser.DeliveryZipcode = user.DeliveryZipcode;
            dbUser.DeliveryCity = user.DeliveryCity;
            dbUser.PreventAccess = user.PreventAccess;

            appDbContext.ApplicationUsers.Update(dbUser);

            var roles = await userManager.GetRolesAsync(dbUser); // List<string>
            var role = await roleManager.FindByNameAsync(roles[0]);
            dbUser.UserRoleId = role.Id;

            if (dbUser.UserRoleId != user.UserRoleId)
            {
                await UpdateUserRoleAsync(dbUser.Id, user.UserRoleId);
            }

            await appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(AllUsers));
           
        }

        public async Task<IActionResult> AllManufacturers()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                List<Manufacturer> allManufacturers = appDbContext.Manufacturers.ToList();

                foreach (Manufacturer manufacturer in allManufacturers) {
                    manufacturer.NbrOfProducts = appDbContext.Products.Where(p => p.ManufacturerId == manufacturer.Id).ToList().Count;
                }

                return View(allManufacturers);
            }
        }

        public async Task<IActionResult> CreateManufacturer()
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                Manufacturer manufacturer = new Manufacturer();

                return View(manufacturer);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCreatedManufacturer(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                appDbContext.Manufacturers.Add(manufacturer);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllManufacturers));
            }
            else
            {
                return RedirectToAction(nameof(CreateManufacturer));
            }
        }

        public async Task<IActionResult> EditManufacturer(int Id)
        {
            // IsUserloggedIn kontrollerar om användaren är inloggad och har rätt behörighet
            if ((await IsUserloggedIn()) == false)
            {
                return Forbid();
            }
            else
            {
                Manufacturer manufacturer = appDbContext.Manufacturers.Find(Id);

                return View(manufacturer);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEditedManufacturer([Bind("Id,BrandName,CompanyName")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                appDbContext.Manufacturers.Update(manufacturer);

                await appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllManufacturers));
            }
            else
            {
                return RedirectToAction(nameof(EditManufacturer));
            }
        }

        public async Task<IActionResult> RemoveManufacturer(int Id)
        {

            Manufacturer manufacturer = appDbContext.Manufacturers.Find(Id);

            if (manufacturer != null)
            {
               
                manufacturer.NbrOfProducts = appDbContext.Products.Where(p => p.ManufacturerId == manufacturer.Id).ToList().Count;

                if (manufacturer.NbrOfProducts == 0)
                {
                    appDbContext.Manufacturers.Remove(manufacturer);

                    await appDbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(AllManufacturers));
        }

        private async Task UpdateUserRoleAsync(string userId, string newRoleId)
        {
            var newRole = await roleManager.FindByIdAsync(newRoleId);
            var user = await userManager.FindByIdAsync(userId);
            var currentRole = await userManager.GetRolesAsync(user); // List<string>

            // Remove existing role         
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRole);

            if (!removeResult.Succeeded)
                throw new InvalidOperationException("Failed to remove roles");

            // Add the new role
            var addResult = await userManager.AddToRoleAsync(user, newRole.Name);

            if (!addResult.Succeeded)
                throw new InvalidOperationException("Failed to add role");

            // Optionally refresh security stamp so active cookies get invalidated
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == user.Id)
                await userManager.UpdateSecurityStampAsync(user);
        }

        // Kontrollerar om användaren är inloggad och har rätt behörighhet
        private async Task<bool> IsUserloggedIn()
        {
            if (signInManager.IsSignedIn(User))
            {
                var user = await userManager.GetUserAsync(User);

                bool isAdminManager = await userManager.IsInRoleAsync(user, "AdminManager");
                bool isShopManager = await userManager.IsInRoleAsync(user, "ShopManager");

                if (isAdminManager || isShopManager)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

