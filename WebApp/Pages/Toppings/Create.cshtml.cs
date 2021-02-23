using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.Toppings
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            ToppingPrices = new SelectList(new[] {1,1.5,2,2.5,3.5,4.5});
            return Page();
        }

        [BindProperty]
        public Topping Topping { get; set; } = default!;

        public SelectList ToppingPrices { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Toppings.Add(Topping);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
