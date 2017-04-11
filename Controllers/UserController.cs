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
            HttpContext.Session.SetString("Error","");
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
            return RedirectToAction ("Dashboard","Home");
        }
        // Get : goes to edit page
        [HttpGet]
        [RouteAttribute("edit")]
        public IActionResult Edit(){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                HttpContext.Session.SetString("Error","Need an account to view Settings page");
                return RedirectToAction("Login","User");
            }
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            User editInfo = _context.users
                .Where(user => user.id == (int)getUserId)
                .SingleOrDefault();
            ViewBag.Edit = editInfo;
            ViewBag.Errors = new List<string>();
            return View();
        }
        [HttpPost]
        [RouteAttribute("edit")]
        public IActionResult EditUser(LoginViewModel model, User newUserInfo){
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            if(ModelState.IsValid){
                User updateUserInfo = _context.users
                    .SingleOrDefault(user => user.id == (int)getUserId);
                    updateUserInfo.FirstName = newUserInfo.FirstName;
                    updateUserInfo.LastName = newUserInfo.LastName;
                    updateUserInfo.Email = newUserInfo.Email;
                    updateUserInfo.Password = newUserInfo.Password;
                    _context.SaveChanges();
                    return RedirectToAction("Settings","Home");
            }
            ViewBag.Edit = _context.users
                .SingleOrDefault(user => user.id == (int)getUserId);
            ViewBag.Errors = ModelState.Values;
            return View("Edit");
        }
        [HttpGet]
        [RouteAttribute("delete")]
        public IActionResult Delete(){
        int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
        List<Order> deleteAll = _context.orders
            .Where(order => order.UserId == getUserId).ToList();
            foreach(var order in deleteAll){
                _context.Remove(order);
                _context.SaveChanges();
            }
            User deleteAccount = _context.users
                .Where(user => user.id == getUserId)
                .SingleOrDefault();
                _context.Remove(deleteAccount);
                _context.SaveChanges();
                HttpContext.Session.Clear();
                return RedirectToAction("dashboard","Home");
        }
    }
}