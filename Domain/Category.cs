using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Category
    {
        public int Id { get; set; }
        
        [MaxLength(64)]
        public string Name { get; set; } = default!;

        public ICollection<Pizza>? Pizzas { get; set; }
    }
}