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
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
