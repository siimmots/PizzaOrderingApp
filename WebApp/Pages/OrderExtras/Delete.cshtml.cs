using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.OrderExtras
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderExtra OrderExtra { get; set; } = default!;

        public int OrderId { get; set; }
        
        public int ClientId { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id, int? orderId, int? clientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (orderId != null)
            {
                OrderId = (int) orderId;
            }
            
            if (clientId != null)
            {
                ClientId = (int) clientId;
            }

            OrderExtra = await _context.OrderExtras
                .Include(o => o.Order)
                .Include(o => o.Topping).FirstOrDefaultAsync(m => m.Id == id);

            if (OrderExtra == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? orderId, int? clientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderExtra = await _context.OrderExtras.FindAsync(id);

            if (OrderExtra != null)
            {
                _context.OrderExtras.Remove(OrderExtra);
                await _context.SaveChangesAsync();
            }

            if (orderId != null && clientId != null)
            {
                return RedirectToPage("/Orders/Create", new {orderId, clientId});
            }

            return RedirectToPage("./Index");
        }
    }
}
