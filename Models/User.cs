using System;
using System.Collections.Generic;

namespace eCommerce.Models{
    public class User {
        public int id {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}

        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        public List <Order> Purchases {get;set;}
        public User (){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        internal void Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}