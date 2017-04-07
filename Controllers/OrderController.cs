using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    public class OrderController : Controller
    {
        private MyContext _context;
 
        public OrderController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [RouteAttribute("order/{id}")]
        public IActionResult OrderItem(int id){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                HttpContext.Session.SetString("Error","Need to be logged in to add to wishlist");
                return RedirectToAction("products","Product");
            }
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            Order newOrder = new Order(){
                UserId = (int)getUserId,
                ProductId = id
            };
            _context.Add(newOrder);
            _context.SaveChanges();
            Product changeQuantity = _context.products.SingleOrDefault(item => item.id == id);
            changeQuantity.Quantity--;
            _context.SaveChanges();
            HttpContext.Session.SetString("Error","");
            return RedirectToAction("products","Product");
        }
    }
}