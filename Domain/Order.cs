using System;
using System.Collections.Generic;

namespace Domain
{
    public class Order
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public bool IsCompleted { get; set; }
        
        public int  ClientId { get; set; }
        public Client? Client { get; set; }

        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<OrderExtra>? OrderExtras { get; set; }
    }
}