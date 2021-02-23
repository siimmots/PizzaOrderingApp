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
    public class IndexModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<OrderExtra> OrderExtra { get;set; } = default!;

        public async Task OnGetAsync()
        {
            OrderExtra = await _context.OrderExtras
                .Include(o => o.Order)
                .Include(o => o.Topping).ToListAsync();
        }
    }
}
