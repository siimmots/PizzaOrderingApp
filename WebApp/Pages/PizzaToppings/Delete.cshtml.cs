using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.PizzaToppings
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PizzaTopping PizzaTopping { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PizzaTopping = await _context.PizzaToppings
                .Include(p => p.Pizza)
                .Include(p => p.Topping).FirstOrDefaultAsync(m => m.Id == id);

            if (PizzaTopping == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PizzaTopping = await _context.PizzaToppings.FindAsync(id);

            if (PizzaTopping != null)
            {
                var pizza = await _context.Pizzas.Where(p => p.Id == PizzaTopping.PizzaId).FirstOrDefaultAsync();
                var topping = await _context.Toppings.Where(p => p.Id == PizzaTopping.ToppingId).FirstOrDefaultAsync();
                pizza.Calories -= topping.Calories;
                
                _context.PizzaToppings.Remove(PizzaTopping);
                await _context.SaveChangesAsync();
                
            }
            
            

            return RedirectToPage("./Index");
        }
    }
}
