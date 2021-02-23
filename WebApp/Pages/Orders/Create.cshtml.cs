using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)] public Client Client { get; set; } = default!;
        
        
        [BindProperty(SupportsGet = true)]
        public Pizza Pizza { get; set; } = default!;
        
        
        [BindProperty(SupportsGet = true)]
        public string? Btn { get; set; }

        [BindProperty(SupportsGet = true)]
        public double TotalPrice { get; set; }

        [BindProperty(SupportsGet = true)] public ICollection<Order> ActiveOrders { get; set; } = new List<Order>();
        
        public async Task<IActionResult> OnGetAsync(int? pizzaId, int? clientId, int? orderId)
        {
            Client = await _context.Clients.Where(c => c.Id == clientId).Include(c => c.Orders).FirstOrDefaultAsync();
            Pizza = await _context.Pizzas.Where(p => p.Id == pizzaId).Include(p => p.Category).FirstOrDefaultAsync();
            
            if (clientId == null)
            {
                return RedirectToPage("/Clients/Create", new {pizzaId});
            }

            var order = new Order();
            if (pizzaId == null && orderId != null)
            {
                order = await _context.Orders.Where(o => o.Id == orderId).Include(o => o.Pizza).FirstOrDefaultAsync();
                Pizza = await _context.Pizzas.Where(p => p.Id == order.Pizza!.Id).Include(p => p.Category).FirstOrDefaultAsync();
            }

            if (pizzaId != null && orderId == null)
            {
                order = new Order()
                {
                    ClientId = (int) clientId,
                    PizzaId = (int) pizzaId,
                    Price = Pizza.Price,
                    Pizza = Pizza,
                    OrderExtras = new List<OrderExtra>()
                };
                await _context.Orders.AddAsync(order);
                Client.Orders!.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Orders/Create", new {pizzaId, clientId, orderId = order.Id});
            }
            
            
            var clientOrders = await _context.Orders.Where(c => c.ClientId == clientId)
                .Include(p => p.Pizza)
                .Include(p => p.Pizza!.Category).Include(o => o.OrderExtras)
                .ToListAsync();

            if (clientOrders.Count == 0)
            {
                return RedirectToPage("/Pizzas/Index");
            }
            
            ActiveOrders = clientOrders;

            foreach (var activeOrder in ActiveOrders)
            {
                TotalPrice += activeOrder.Pizza!.Price;
                
                var extras = await _context.OrderExtras
                    .Where(o => o.OrderId == activeOrder.Id)
                    .Include(o => o.Topping)
                    .ToListAsync();
                
                foreach (var extra in extras)
                {
                    TotalPrice += extra.Topping!.ExtraPrice;
                }
            }

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? pizzaId, int? clientId, bool? anotherPizza)
        {
            Client = await _context.Clients.Where(c => c.Id == clientId).Include(c => c.Orders).FirstOrDefaultAsync();
            Pizza = await _context.Pizzas.Where(p => p.Id == pizzaId).Include(p => p.Category).FirstOrDefaultAsync();
            
            if (clientId != null)
            {

                if (Btn != "Confirm")
                {
                    return RedirectToPage("/Pizzas/Index", new {clientId});
                }
                
                var clientOrders = await _context.Orders.Where(c => c.ClientId == clientId).Include(p => p.Pizza)
                    .ToListAsync();
                
                clientOrders.ForEach(c => c.IsCompleted = true);
                clientOrders.ForEach(c => c.Pizza!.AmountSold++);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Pizzas/Index", new {confirmed = true});
            }

            return Page();
        }
    }
}
