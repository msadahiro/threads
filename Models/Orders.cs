using System;

namespace eCommerce.Models{
    public class Order{
        public int id {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt {get;set;}

        public Order(){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}