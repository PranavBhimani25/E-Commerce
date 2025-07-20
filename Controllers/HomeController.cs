using CodeTech_Task_1.Data;
using CodeTech_Task_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeTech_Task_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public IActionResult Demo()
        //{
        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPage(Customer model)
            {
            if (ModelState.IsValid)
            {

                if (model.Email == "admin@gmail.com")
                {
                    if (model.Password == "admin@123")
                    {
                        HttpContext.Session.SetString("UserSession", "active");
                        return RedirectToAction("AdminIndex", "Admin");
                    }
                    else {

                        ViewBag.LoginError = "Admin Password is incorrect !";
                        return View();
                        
                    }
                }

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email && c.Password == model.Password);
                if (customer != null)
                {
                    //Login is Successfull
                    HttpContext.Session.SetString("UserSession", "active");
                    HttpContext.Session.SetInt32("Cust_Id", customer.CustomerId);
                    HttpContext.Session.SetString("Cust_Name", customer.Name);
                    return RedirectToAction("UserHomePage","User");
                }
                else
                {
                    ViewBag.LoginError = "Invalid Email or Password !";
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Validation Error in '{key}' : '{error.ErrorMessage}'");
                    }
                }
            }
            return View();
        }

        public IActionResult SignupPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignupPage(Customer model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);

                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(model);
                }

                var cust = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                _context.Customers.Add(cust);
                await _context.SaveChangesAsync();

                //Session["Cust_ID"] = model.CustomerId;

                return RedirectToAction("LoginPage");
            }
            else
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Validation Error in '{key}' : '{error.ErrorMessage}'");
                    }
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            string cartKey = "Cart";
            HttpContext.Session.Remove(cartKey);

            HttpContext.Session.Remove("UserSession");
            HttpContext.Session.Remove("Cust_Name");
            HttpContext.Session.Remove("Cust_Id");

            return RedirectToAction("LoginPage","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
