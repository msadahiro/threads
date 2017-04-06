using System;

namespace eCommerce.Models{
    public class Product{
        public int id {get;set;}
        public string Name {get;set;}
        public string Image {get;set;}
        public string Description {get;set;}
        public int Quantity {get;set;}

        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        public Product(){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}