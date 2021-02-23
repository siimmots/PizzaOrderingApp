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

namespace WebApp.Pages.OrderExtras
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        [BindProperty(SupportsGet = true)]
        public List<SelectListItem>? Orders { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<SelectListItem>? Toppings { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ClientId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? OrderId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? PizzaId { get; set; }
        
        
        [BindProperty(SupportsGet = true)]
        public Order? Order { get; set; }
        

        public async Task<IActionResult> OnGet(int? clientId, int? pizzaId, int? orderId)
        {

            Order = await _context.Orders.Where(o => o.Id == orderId)
                .Include(o => o.Pizza)
                .Include(o => o.OrderExtras).FirstOrDefaultAsync();
            
            Orders = _context.Orders.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"{o.Client!.FirstName} {o.Client!.LastName} - {o.Pizza!.Name.ToString()}"
                    
                }).ToList();
            
            Toppings = _context.Toppings.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = $"Extra {o.Name}"
            }).ToList();
            
            return Page();
        }

        [BindProperty]
        public OrderExtra OrderExtra { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? clientId, int? orderId, int? pizzaId)
        {
            Order = await _context.Orders.Where(o => o.Id == orderId)
                .Include(o => o.Pizza)
                .Include(o => o.OrderExtras).FirstOrDefaultAsync();
            
            if (orderId != null)
            {
                OrderExtra.OrderId = (int) orderId;
            }

            await _context.OrderExtras.AddAsync(OrderExtra);
            Order?.OrderExtras!.Add(OrderExtra);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Orders/Create", new {clientId, orderId, pizzaId});
        }
    }
}
