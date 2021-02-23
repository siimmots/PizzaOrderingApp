using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Client> Clients { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderExtra> OrderExtras { get; set; } = default!;
        public DbSet<Pizza> Pizzas { get; set; } = default!;
        public DbSet<PizzaTopping> PizzaToppings { get; set; } = default!;
        public DbSet<Topping> Toppings { get; set; } = default!;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}