using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
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
            ViewBag.RecentCustomers = latestCustomers;
            ViewBag.Today = todayString;
            return View();
        }
        [HttpGet]
        [RouteAttribute("customers")]
        public IActionResult Customers(){
            List<User> getAllUsers = _context.users
                .Include(order => order.Purchases)
                    .ThenInclude(item => item.Product)
                .ToList();
            ViewBag.ListOfUsers = getAllUsers;
            return View();
        }
    }
}
