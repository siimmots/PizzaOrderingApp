using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        public int? PizzaId { get; set; }

        public IActionResult OnGet(int? pizzaId)
        {
            
            PizzaId = pizzaId;
            return Page();
        }

        [BindProperty]
        public Client Client { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? pizzaId)
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.Orders = new List<Order>();

            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();
            
            
            return RedirectToPage("/Orders/Create", new{clientId = Client.Id, pizzaId});
        }
    }
}
