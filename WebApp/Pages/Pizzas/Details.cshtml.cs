using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Pizzas
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public Pizza Pizza { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public List<Topping?>? Toppings { get; set; }
        
        
        [BindProperty(SupportsGet = true)]
        public double ToppingRevenue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pizza = await _context.Pizzas
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);

            var pizzaToppings = await _context.PizzaToppings
                .Where(p => p.PizzaId == id)
                .Include(p => p.Topping)
                .ToListAsync();

            var orders = await _context.Orders.Where(p => p.PizzaId == id).ToListAsync();

            foreach (var order in orders)
            {
                var extras = await _context.OrderExtras
                    .Where(o => o.OrderId == order.Id)
                    .Include(o => o.Topping)
                    .ToListAsync();
                foreach (var extra in extras)
                {
                    ToppingRevenue += extra.Topping!.ExtraPrice;
                }
            }

            foreach (var topping in pizzaToppings.Where(topping => topping.Topping != null))
            {
                Toppings?.Add(topping.Topping);
            }
            
            if (Pizza == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
