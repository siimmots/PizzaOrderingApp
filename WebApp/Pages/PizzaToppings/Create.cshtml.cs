using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.PizzaToppings
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> PizzaIds { get; set; } = default!;
        public List<SelectListItem> ToppingIds { get; set; } = default!;
        
        public async Task<IActionResult> OnGet()
        {
            PizzaIds = await _context.Pizzas.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToListAsync();
            
            ToppingIds = await _context.Toppings.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToListAsync();
            return Page();
        }

        [BindProperty]
        public PizzaTopping PizzaTopping { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.PizzaToppings.AddAsync(PizzaTopping);

            var pizza = await _context.Pizzas.Where(p => p.Id == PizzaTopping.PizzaId).FirstOrDefaultAsync();
            var topping = await _context.Toppings.Where(p => p.Id == PizzaTopping.ToppingId).FirstOrDefaultAsync();
            pizza.Calories += topping.Calories;
            
            
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
