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
    public class OrderController : Controller
    {
        private MyContext _context;
 
        public OrderController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [RouteAttribute("orders")]
        public IActionResult MyOrders(){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                HttpContext.Session.SetString("Error","Need to be logged in to see your orders");
                return RedirectToAction("Login","User");
            }
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            List<User> getOneUserWishList = _context.users
                .Where(user => user.id == getUserId)
                .Include(order => order.Purchases)
                    .ThenInclude(item => item.Product)
                .ToList();
            ViewBag.ShowOne = getOneUserWishList;
            return View();
        }
        [HttpGet]
        [RouteAttribute("order/{id}")]
        public IActionResult OrderItem(int id){
            if(HttpContext.Session.GetInt32("CurrentUser")==null){
                HttpContext.Session.SetString("Error","Need to be logged in to add to wishlist");
                return RedirectToAction("products","Product");
            }
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            User getWishList = _context.users
                .Where(user => user.id == (int)getUserId)
                .Include(orders => orders.Purchases)
                .SingleOrDefault();
            foreach(var order in getWishList.Purchases){
               if((order.UserId == (int)getUserId) && (order.ProductId == id)){
                   var orderId = order.id;
                   Order updateQuantity = _context.orders.SingleOrDefault(o => o.id == orderId);
                   updateQuantity.Quantity += 1;
                   _context.SaveChanges();
                   Product UpdateProductQuantity = _context.products.SingleOrDefault(item => item.id == id);
                    UpdateProductQuantity.Quantity--;
                    _context.SaveChanges();
                   HttpContext.Session.SetString("Error","");
                   return RedirectToAction("products","Product");
               }
            }
            Order newOrder = new Order(){
                UserId = (int)getUserId,
                ProductId = id,
                Quantity =  + 1
            };
            _context.Add(newOrder);
            _context.SaveChanges();
            Product changeQuantity = _context.products.SingleOrDefault(item => item.id == id);
            changeQuantity.Quantity--;
            _context.SaveChanges();
            HttpContext.Session.SetString("Error","");
            return RedirectToAction("products","Product");
        }
        [HttpGet]
        [RouteAttribute("remove/{id}")]
        public IActionResult RemoveOrder(int id){
            int? getUserId = HttpContext.Session.GetInt32("CurrentUser");
            List<Order> deleteAll = _context.orders
                .Where(product => product.ProductId == id)
                .ToList();
            foreach(var item in deleteAll){
                _context.Remove(item);
                _context.SaveChanges();
            }
            Product UpdateProductQuantity = _context.products.SingleOrDefault(prod => prod.id == id);
            foreach(var deleteditem in deleteAll){
                UpdateProductQuantity.Quantity+= deleteditem.Quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("MyOrders");
        }
    }
}