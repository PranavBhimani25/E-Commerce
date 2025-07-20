using CodeTech_Task_1.Data;
using CodeTech_Task_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CodeTech_Task_1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly AppDbContext _context;

        public AdminController(ILogger<AdminController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult AdminIndex()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            return View();
        }

        //public IActionResult Profile()
        //{
        //    var sessionValue = HttpContext.Session.GetString("UserSession");
        //    if (sessionValue != "active")
        //    {
        //        return RedirectToAction("LoginPage", "Home");
        //    }
        //    return View();
        //}

        public IActionResult Product()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var allproduct = _context.Products.ToList();
            return View(allproduct);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                Directory.CreateDirectory(uploadsFolder); // just in case

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/Images/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            ViewBag.ViewModel = "New Product is Added !";
            return Json(new { success = true });
        }

        //public IActionResult Semo()
        //{
        //    var sessionValue = HttpContext.Session.GetString("UserSession");
        //    if (sessionValue != "active")
        //    {
        //        return RedirectToAction("LoginPage", "Home");
        //    }
        //    return View();
        //}

        public async Task<IActionResult> Edit(int id)
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product); // This returns the edit view with product model
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile ImageFile)
        {
            var existing = await _context.Products.FindAsync(product.ProductId);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Description = product.Description;
            existing.Stock = product.Stock;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                existing.ImageUrl = "/Images/" + fileName;
            }

            await _context.SaveChangesAsync();
            ViewBag.ViewModel = "Product is Updated !";
            return RedirectToAction("Product","Admin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product); // This returns the edit view with product model
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // delete the image file in Wwwroot image folder
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            ViewBag.ViewModel = "Product is Deleted !";
            return RedirectToAction("Product","Admin");
        }

        public IActionResult Custmber()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var customberlist = _context.Customers.ToList();
            return View(customberlist);
        }

        public async Task<IActionResult> EditCust(int id)
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null) return NotFound();
            return View(Customer); 
        }

        public async Task<IActionResult> EditCustmber(Customer customer)
        {
            var existing = await _context.Customers.FindAsync(customer.CustomerId);
            if (existing == null) return NotFound();

            existing.Name = customer.Name;
            existing.Email = customer.Email;
            await _context.SaveChangesAsync();
            return RedirectToAction("Custmber","Admin");
        }

        public async Task<IActionResult> DeleteCust(int id)
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null) return NotFound();
            return View(Customer);
        }

        public async Task<IActionResult> DeleteCustmber(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Custmber", "Admin");
        }


        public async Task<IActionResult> ViewAllOrderHistory() 
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();

            return View(order);
        }


        public async Task<IActionResult> _OrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            switch (status.ToLower())
            {
                case "shipped":
                    order.Status = OrderStatus.Shipped;
                    TempData["OrderStatus"] = "Shipped";
                    break;
                case "cancelled":
                    order.Status = OrderStatus.Cancelled;
                    TempData["OrderStatus"] = "Cancelled";
                    break;
            }
            await _context.SaveChangesAsync();

            TempData["ShowModal"] = true;
            TempData["OrderId"] = orderId;
            return RedirectToAction("ViewAllOrderHistory");
        }

        //public IActionResult OrderDetails()
        //{
        //    return View();
        //}

    }
}
