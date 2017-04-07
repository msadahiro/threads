using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
 
        public HomeController(MyContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Dashboard()
        {
            var today = DateTime.Today;
            var todayString = today.ToString("yyyy-MM-dd");
            List<User> getRecent = _context.users
                .OrderByDescending(create => create.CreatedAt)
                .ToList();
            var latestCustomers = getRecent.Take(3);

            List<Order> getRecentOrders = _context.orders
                .OrderByDescending(orders => orders.CreatedAt)
                .Include(user => user.User)
                .Include(item => item.Product)
                .ToList();
            var latestOrders = getRecentOrders.Take(3);
            ViewBag.RecentOrders = latestOrders;
            ViewBag.RecentCustomers = latestCustomers;
            ViewBag.Today = todayString;
            return View();
        }
        [HttpGet]
        [RouteAttribute("customers")]
        public IActionResult Customers(){
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            List<User> getAllUsers = _context.users
                .Where(user => user.id != getUserId)
                .Include(order => order.Purchases)
                    .ThenInclude(item => item.Product)
                .ToList();
            ViewBag.ListOfUsers = getAllUsers;
            return View();
        }
    }
}
