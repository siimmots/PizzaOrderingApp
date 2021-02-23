using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.OrderExtras
{
    public class EditModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public EditModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderExtra OrderExtra { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public List<SelectListItem>? Orders { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<SelectListItem>? Toppings { get; set; }

        
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
            
            Orders = await _context.Orders.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"{o.Client!.FirstName} {o.Client!.LastName} - {o.Pizza!.Name.ToString()}"
                    
            }).ToListAsync();
            
            Toppings = await _context.Toppings.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"Extra {o.Name}"
            }).ToListAsync();
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? clientId, int? orderId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OrderExtra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExtraExists(OrderExtra.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (clientId != null && orderId != null)
            {
                return RedirectToPage("/Orders/Create", new {clientId, orderId});
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExtraExists(int id)
        {
            return _context.OrderExtras.Any(e => e.Id == id);
        }
    }
}
