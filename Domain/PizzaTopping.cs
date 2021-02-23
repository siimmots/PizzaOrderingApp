using System;

namespace Domain
{
    public class PizzaTopping
    {
        public int Id { get; set; }

        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }

        public int ToppingId { get; set; }
        public Topping? Topping { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}