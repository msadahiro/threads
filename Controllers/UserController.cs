using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class UserController : Controller
    {
        private MyContext _context;
 
        public UserController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [RouteAttribute("login")]
        public IActionResult Login(){
            ViewBag.errors = new List<string>();
            ViewBag.RegEmailError = "";
            return View();
        }
        [HttpPost]
        [RouteAttribute("RegisterUser")]
        public IActionResult RegisterUser(RegisterViewModel model, User newUser){
            if(ModelState.IsValid){
                User RegCheckEmail = _context.users.Where(User => User.Email == newUser.Email).SingleOrDefault();
                if(RegCheckEmail == null){
                    _context.Add(newUser);
                    _context.SaveChanges();
                    var CurrentUserId = newUser.id;
                    HttpContext.Session.SetInt32("CurrentUser", (int)CurrentUserId);
                    return RedirectToAction ("dashboard","Home");
                }
                else{
                    ViewBag.RegEmailError = "Email already used";
                }
            }
            else{
                ViewBag.RegEmailError = "";
            }
            ViewBag.errors = ModelState.Values;
            return View("Login");
        }
    }
}