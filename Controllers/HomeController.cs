using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        [HttpGet]
        [RouteAttribute("customers")]
        public IActionResult Customers(){
            List<User> getAllUsers = _context.users
                .ToList();
            ViewBag.ListOfUsers = getAllUsers;
            return View();
        }
    }
}
