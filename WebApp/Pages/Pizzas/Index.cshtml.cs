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
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pizza> Pizza { get;set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SearchDescription { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SearchCategories { get; set; }
        
        
        [BindProperty(SupportsGet = true)]
        public string? SearchToppings { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? Btn { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Confirmed { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? clientId, bool? confirmed)
        {
            if (clientId != null)
            {
                ClientId = clientId;
            }
            Confirmed = confirmed;
            
            var query = _context.Pizzas
                .Include(p => p.Category).AsQueryable();

            if (Btn == "Reset")
            {
                SearchCategories = "";
                SearchDescription = "";
                SearchName = "";
                SearchToppings = "";

                if (clientId != null)
                {
                    return RedirectToPage("/Pizzas/Index", new {clientId});
                }
                return RedirectToPage("/Pizzas/Index");
            }
            
            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                query = query.Where(p => p.Name.Contains(SearchName));
            }
            if (!string.IsNullOrWhiteSpace(SearchDescription))
            {
                query = query.Where(p => p.Description.Contains(SearchDescription));
            }
            if (!string.IsNullOrWhiteSpace(SearchCategories))
            {
                query = query.Where(p => p.Category!.Name.Contains(SearchCategories));
            }
            if (!string.IsNullOrWhiteSpace(SearchToppings))
            {
                var topping = await _context.PizzaToppings.Where(p => p.Topping!.Name.Contains(SearchToppings)).FirstOrDefaultAsync();
                
                query = query.Where(p => p.PizzaToppings!.Contains(topping));
            }
            
            Pizza = await query.ToListAsync();


            return Page();
        }
    }
}
