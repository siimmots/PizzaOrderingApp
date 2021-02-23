using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Humanizer;

namespace WebApp.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? ClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PizzaId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<Topping>? Toppings { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<OrderExtra>? Extras { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? clientId,int orderId, int? pizzaId)
        {
            
            OrderId = orderId;
            
            if (orderId == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Pizza)
                .Include(o => o.OrderExtras)
                .FirstOrDefaultAsync(m => m.Id == orderId);
            
            ClientId = clientId ?? Order.ClientId;
            PizzaId = pizzaId ?? Order.PizzaId;
            
            var pizzaToppings = await _context.PizzaToppings
                .Where(p => p.PizzaId == PizzaId)
                .Include(p => p.Topping)
                .ToListAsync();

            foreach (var topping in pizzaToppings.Where(topping => topping.Topping != null))
            {
                Toppings?.Add(topping.Topping);
            }
            
            var extras = await _context.OrderExtras
                .Where(o => o.OrderId == OrderId)
                .Include(o => o.Topping)
                .ToListAsync();
            
            if (extras != null)
            {
                foreach (var extra in extras.Where(topping => topping.Topping != null))
                {
                    Extras?.Add(extra);
                }
            }
            
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
