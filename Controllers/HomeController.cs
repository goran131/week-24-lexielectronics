using LexiElectronics.Data;
using LexiElectronics.Models;
using LexiElectronics.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LexiElectronics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext appDbContext;
      
        public HomeController(ApplicationDbContext context)
        {
            appDbContext = context;
            if (!appDbContext.Users.Any() && !appDbContext.Products.Any())
            {
                AddData();
            }

            // AddDataOne();
        }

        public async Task<IActionResult> Index()
        {
            // CreateGUID createGUID = new CreateGUID();
            // return View(createGUID);

            List<Product> products = await appDbContext.Products.Where(p => p.VisibleInShop).AsNoTracking().ToListAsync();

            products = GetSixTopSellers(products);
            
            return View(products);

            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Product> GetSixTopSellers(List<Product> products)
        {
            List<OrderItem> orderItems = appDbContext.OrderItems.ToList();

            foreach (Product product in products)
            {
                foreach (OrderItem item in orderItems)
                {
                    if (product.Id == item.ProductId)
                    {
                        product.nbrOfItemsSold = product.nbrOfItemsSold + item.Quantity;
                    }
                }
            }

            products = products.OrderBy(p => p.nbrOfItemsSold).ToList();

            products.Reverse();

            products.RemoveRange(5, products.Count() - 6);

            return products;
        }

        private void AddDataOne() {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "maria@overling.se";
            user.NormalizedUserName = "MARIA@OVERLING.SE";
            user.Email = "maria@overling.se";
            user.NormalizedEmail = "MARIA@OVERLING.SE";
            user.EmailConfirmed = true;
            user.PasswordHash = "AQAAAAIAAYagAAAAELBSw1854cAsBZvW9kmiJajYNK9UfoobyP2KEbLCboryFpTsp2mQtMRNFXzb0lodyg==";
            user.SecurityStamp = "LUSZTETYT63Q4DPCVMUSX33CEA5LJGUE";
            user.ConcurrencyStamp = "2b1354de-7d28-4682-b52b-4e7dede75dac";
            user.PhoneNumber = "073-2198068";
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;
            user.Discriminator = "ApplicationUser";
            user.DeliveryName = "Maria Överling";
            user.DeliveryCity = "Tomelilla";
            user.DeliveryStreetAddress = "Hökaregatan 9";
            user.DeliveryZipcode = "27334";
            user.InvoiceName = "Maria Överling";
            user.InvoiceCity = "Tomelilla";
            user.InvoiceStreetAddress = "Hökaregatan 9";
            user.InvoiceZipcode = "27334";
            user.Firstname = "Maria";
            user.Lastname = "Överling";
            user.PreventAccess = false;

            appDbContext.ApplicationUsers.Add(user);
            appDbContext.SaveChanges();
        }
        private void AddData()
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "goran.rosenberg@tomelilla.nu";
            user.NormalizedUserName = "GORAN.ROSENBERG@TOMELILLA.NU";
            user.Email = "goran.rosenberg@tomelilla.nu";
            user.NormalizedEmail = "GORAN.ROSENBERG@TOMELILLA.NU";
            user.EmailConfirmed = true;
            user.PasswordHash = "AQAAAAIAAYagAAAAEDSPYICngzxw4abIgENa7HiaqBHZiQd5ANFYdegktU7Oh9mpRD1jJNMGLe1fnedC1w==";
            user.SecurityStamp = "5OR5O35FWLHBGGJHKZPQA7KHZPPAM7KR";
            user.ConcurrencyStamp = "a9ba364b-6efd-4576-a9df-086bb0774b23";
            user.PhoneNumber = "073-2198068";
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;
            //user.Discriminator = "ApplicationUser";
            user.DeliveryCity = "Tomelilla";
            user.DeliveryStreetAddress = "Västergatan 44A";
            user.DeliveryZipcode = "27334";
            user.InvoiceCity = "Tomelilla";
            user.InvoiceStreetAddress = "Västergatan 44A";
            user.InvoiceZipcode = "27334";
            user.Firstname = "Göran";
            user.Lastname = "Rosenberg";

            appDbContext.ApplicationUsers.Add(user);

            user = new ApplicationUser();
            user.UserName = "goran.ros@gmail.com";
            user.NormalizedUserName = "GORAN.ROS@GMAIL.COM";
            user.Email = "goran.ros@gmail.com";
            user.NormalizedEmail = "GORAN.ROS@GMAIL.COM";
            user.EmailConfirmed = true;
            user.PasswordHash = "AQAAAAIAAYagAAAAELBSw1854cAsBZvW9kmiJajYNK9UfoobyP2KEbLCboryFpTsp2mQtMRNFXzb0lodyg==";
            user.SecurityStamp = "LUSZTETYT63Q4DPCVMUSX33CEA5LJGUE";
            user.ConcurrencyStamp = "2b1354de-7d28-4682-b52b-4e7dede75dac";
            user.PhoneNumber = "073-2198068";
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;
            // user.Discriminator = "ApplicationUser";
            user.DeliveryCity = "Tomelilla";
            user.DeliveryStreetAddress = "Västergatan 44A";
            user.DeliveryZipcode = "27334";
            user.InvoiceCity = "Tomelilla";
            user.InvoiceStreetAddress = "Västergatan 44A";
            user.InvoiceZipcode = "27334";
            user.Firstname = "Göran";
            user.Lastname = "Rosenberg";

            appDbContext.ApplicationUsers.Add(user);

            user = new ApplicationUser();
            user.UserName = "maria@overling.se";
            user.NormalizedUserName = "MARIA@OVERLING.SE";
            user.Email = "maria@overling.se";
            user.NormalizedEmail = "MARIA@OVERLING.SE";
            user.EmailConfirmed = true;
            user.PasswordHash = "AQAAAAIAAYagAAAAELBSw1854cAsBZvW9kmiJajYNK9UfoobyP2KEbLCboryFpTsp2mQtMRNFXzb0lodyg==";
            user.SecurityStamp = "LUSZTETYT63Q4DPCVMUSX33CEA5LJGUE";
            user.ConcurrencyStamp = "2b1354de-7d28-4682-b52b-4e7dede75dac";
            user.PhoneNumber = "073-2198068";
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnd = null;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;
            user.Discriminator = "ApplicationUser";
            user.DeliveryName = "Maria Överling";
            user.DeliveryCity = "Tomelilla";
            user.DeliveryStreetAddress = "Hökaregatan 9";
            user.DeliveryZipcode = "27334";
            user.InvoiceName = "Maria Överling";
            user.InvoiceCity = "Tomelilla";
            user.InvoiceStreetAddress = "Hökaregatan 9";
            user.InvoiceZipcode = "27334";
            user.Firstname = "Maria";
            user.Lastname = "Överling";
            user.PreventAccess = false;
            appDbContext.ApplicationUsers.Add(user);
        }
    }
}
