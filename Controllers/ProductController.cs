using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

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
    }    
}