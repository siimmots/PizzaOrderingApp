using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Topping
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; } = default!;

        public double ExtraPrice { get; set; } = default!;
        
        public int Calories { get; set; }

        public ICollection<OrderExtra>? OrderExtras { get; set; }

        public ICollection<PizzaTopping>? PizzaToppings { get; set; }
    }
}   