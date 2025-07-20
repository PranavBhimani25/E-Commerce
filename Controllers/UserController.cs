using CodeTech_Task_1.Data;
using CodeTech_Task_1.Helpers;
using CodeTech_Task_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeTech_Task_1.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult UserHomePage()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }

            var custIdSession = HttpContext.Session.GetInt32("Cust_Id");
            var profile = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == custIdSession);
            if (profile == null) { return NotFound(); }
            return View(profile);
        }

        public async Task<IActionResult> Shop()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // Add to cart
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            //await Task.Delay(4000);
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            string cartKey = "Cart";

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity += 1;
            }
            else
            {
                cart.Add(new CartItem
                {
                    Id = id,
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson(cartKey, cart);
            
            return RedirectToAction("Shop");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, bool increase)
        {
            string cartKey = "Cart";
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(cartKey) ?? new List<CartItem>();

            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (increase)
                    item.Quantity += 1;
                else if (item.Quantity > 1)
                    item.Quantity -= 1;
            }

            HttpContext.Session.SetObjectAsJson(cartKey, cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveItem(int cartItemId)
        {
            string cartKey = "Cart";

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(cartKey);
            if (cart != null)
            {
                var itemToRemove = cart.FirstOrDefault(i => i.Id == cartItemId);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetObjectAsJson(cartKey, cart);
                }
            }

            return RedirectToAction("Cart");
        }


        // View cart
        public IActionResult Cart()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var userId = HttpContext.Session.GetInt32("Cust_Id");
            string cartKey = "Cart";
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(cartKey);

            if (cart == null || !cart.Any())
                return RedirectToAction("Cart");

            // Lookup the Customer by userId
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == userId);
            if (customer == null)
            {
                return Unauthorized();
            }

            

            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.Sum(i => i.Price * i.Quantity),
                Status = OrderStatus.Pending,
                OrderItems = cart.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove(cartKey); // Clear cart
            TempData["OderID"] = order.Id;
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            return View();
        }

        public IActionResult OrderHistory()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }
            var userId = HttpContext.Session.GetInt32("Cust_Id");

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == userId);
            if (customer == null)
            {
                return RedirectToAction("LoginPage", "User");
            }

            var orders = _context.Orders
                .Where(o => o.CustomerId == customer.CustomerId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // GET: /Order/Pay/{orderId}
        public IActionResult Pay(int orderId)
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }

            var userId = HttpContext.Session.GetInt32("Cust_Id");
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == userId);
            if (customer == null) return Unauthorized();

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == orderId && o.CustomerId == customer.CustomerId);
            if (order == null) return NotFound();

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount
            };

            ViewBag.Order = order;
            return View(payment);
        }

        // POST: /Payment/Pay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pay(Payment model)
        {
            if (!ModelState.IsValid)
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == model.OrderId);
                ViewBag.Order = order;
                return View(model);
            }

            model.TransactionId = Guid.NewGuid().ToString();
            model.PaymentDate = DateTime.UtcNow;
            model.Status = PaymentStatus.Completed;

            _context.Payments.Add(model);

            // Optionally update the order status
            var orderToUpdate = _context.Orders.FirstOrDefault(o => o.Id == model.OrderId);
            if (orderToUpdate != null)
            {
                orderToUpdate.Status = OrderStatus.Processing;
            }


            _context.SaveChanges();
            TempData["ShowModal"] = true;
            TempData["OrderId"] = model.OrderId;
            TempData["TransactionId"] = model.TransactionId;
            TempData["Amount"] = model.Amount.ToString();
            return RedirectToAction("OrderHistory", "User");
        }

        public async  Task<IActionResult> MyShippedOrders()
        {
            var sessionValue = HttpContext.Session.GetString("UserSession");
            if (sessionValue != "active")
            {
                return RedirectToAction("LoginPage", "Home");
            }

            var userId = HttpContext.Session.GetInt32("Cust_Id"); 

            var orders = await _context.Orders
                .Where(o => o.Customer.CustomerId == userId && o.Status == OrderStatus.Shipped)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
            
        }


        public async Task<IActionResult> ProductDelivered(int orderid)
        {
            var order = await _context.Orders.FindAsync(orderid);
            if (order != null) { NotFound(); }

            order.Status = OrderStatus.Delivered;

            _context.SaveChanges();
            TempData["ShowModal"] = true;
            return RedirectToAction("MyShippedOrders");
        }
    }

}

