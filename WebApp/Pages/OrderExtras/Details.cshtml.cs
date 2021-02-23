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
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderExtra OrderExtra { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
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
    }
}
