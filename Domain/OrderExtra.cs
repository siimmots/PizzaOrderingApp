using System;

namespace Domain
{
    public class OrderExtra
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ToppingId { get; set; }
        public Topping? Topping { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}