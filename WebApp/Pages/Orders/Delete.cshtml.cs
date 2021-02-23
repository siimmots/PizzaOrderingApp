using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        [BindProperty(SupportsGet = true)]
        public int? ClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PizzaId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Pizza).FirstOrDefaultAsync(m => m.Id == orderId );
            
            if (Order != null)
            {
                ClientId = Order.ClientId;
                PizzaId = Order.PizzaId;
            }
            
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? orderId, int? clientId, int? pizzaId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FindAsync(orderId);

            if (Order != null)
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }

            if (clientId != null && pizzaId != null)
            {
                return RedirectToPage("/Orders/Create", new {orderId, pizzaId = PizzaId, clientId = ClientId});
            }

            return RedirectToPage("./Index");
        }
    }
}
