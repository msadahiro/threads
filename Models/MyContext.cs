using Microsoft.EntityFrameworkCore;
 
namespace eCommerce.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Product> products {get;set;}
    }
}
