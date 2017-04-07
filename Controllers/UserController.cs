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
            ViewBag.LoginError = HttpContext.Session.GetString("Error");
            ViewBag.LogError = "";
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
            ViewBag.LogError = "";
            ViewBag.RegEmailError = "";
            ViewBag.errors = ModelState.Values;
            return View("Login");
        }
        [HttpPost]
        [RouteAttribute("signIn")]
        public IActionResult LoginUser(string Email, string Password, LoginViewModel model){
            if(ModelState.IsValid){
                User SignInUser = _context.users.Where(User => User.Email == Email).SingleOrDefault();
                if(SignInUser != null && Password != null){
                    if(SignInUser.Password == Password){
                        HttpContext.Session.SetInt32("CurrentUser",(int)SignInUser.id);
                        return RedirectToAction ("dashboard","Home");
                    }
                }
            }
            ViewBag.errors = new List<string>();
            ViewBag.RegEmailError = "";
            ViewBag.LogError = "Invalid Login";
            return View ("Login");
        }
        [HttpGet]
        [RouteAttribute("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction ("Login");
        }
        
    }
}