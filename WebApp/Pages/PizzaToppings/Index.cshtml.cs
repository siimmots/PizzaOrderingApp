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
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PizzaTopping> PizzaTopping { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PizzaTopping = await _context.PizzaToppings
                .Include(p => p.Pizza)
                .Include(p => p.Topping).ToListAsync();
        }
    }
}
