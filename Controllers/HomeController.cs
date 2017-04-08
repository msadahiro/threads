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

            List<Product> getRecentProd = _context.products
                .OrderByDescending(created => created.CreatedAt)
                .ToList();
            var latestProducts = getRecentProd.Take(5);
            ViewBag.RecentProducts = latestProducts;
            ViewBag.RecentOrders = latestOrders;
            ViewBag.RecentCustomers = latestCustomers;
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
        [HttpGet]
        [RouteAttribute("customer/{id}")]
        public IActionResult ShowOneCustomer(int id){
            User getOne = _context.users
                .Where(user => user.id == id)
                .Include(orders => orders.Purchases)
                    .ThenInclude(item => item.Product)
                .SingleOrDefault();
            ViewBag.OneCustomer = getOne;
            return View();
        }
        [HttpGet]
        [RouteAttribute("settings")]
        public IActionResult Settings(){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                HttpContext.Session.SetString("Error","Need an account to view Settings page");
                return RedirectToAction("Login","User");
            }
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            User getInfo = _context.users
                .Where(user => user.id == (int)getUserId)
                .Include(orders => orders.Purchases)
                    .ThenInclude(item => item.Product)
                .SingleOrDefault();
            ViewBag.SettingsForOne = getInfo;
            return View();
        }
    }
}
