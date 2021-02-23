using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Pizza
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        [MaxLength(4096)]
        public string Description { get; set; } = default!;

        public double Price { get; set; }

        public int AmountSold { get; set; }

        public int Calories { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<PizzaTopping>? PizzaToppings { get; set; }
        
    }
}