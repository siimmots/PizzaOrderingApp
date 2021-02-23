using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Client
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; } = default!;
        
        [MaxLength(64)]
        public string LastName { get; set; } = default!;

        [MaxLength(15)] public string PhoneNr { get; set; } = default!;

        public ICollection<Order>? Orders { get; set; }
    }
}