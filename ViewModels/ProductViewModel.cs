using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models{
    public class ProductViewModel{
        [Required]
        public string Name {get;set;}
        [RequiredAttribute]
        public string Image {get;set;}

        [RequiredAttribute]
        public string Description {get;set;}

        [RequiredAttribute]
        public int Quantity {get;set;}
        
    }
}