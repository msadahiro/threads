using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Controllers {
    public class ProductController:Controller{
        private MyContext _context;

        public ProductController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [RouteAttribute("products")]
        public IActionResult Products(){
            List<Product> getAllProducts = _context.products
                .ToList();
            ViewBag.LoginError = HttpContext.Session.GetString("Error");
            HttpContext.Session.SetString("Error","");
            ViewBag.AllProducts = getAllProducts;
            ViewBag.errors = new List<string>();
            return View();
        }
        [HttpPost]
        [RouteAttribute("create")]
        public IActionResult Create(ProductViewModel model, Product newProduct){
            if(ModelState.IsValid){
                _context.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction ("products");
            }
            else{
                ViewBag.errors = ModelState.Values;
                return View("Products");
            }
        }
        [HttpGet]
        [RouteAttribute("showProduct/{id}")]
        public IActionResult showProduct(int id){
            Product showOne = _context.products
                .Where(item => item.id == id)
                .Include(order => order.Orders)
                    .ThenInclude(user => user.User)
                .SingleOrDefault();
            ViewBag.ShowProduct = showOne;
            return View();
        }
    }    
}