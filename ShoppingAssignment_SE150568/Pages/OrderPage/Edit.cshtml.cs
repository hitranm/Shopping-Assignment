using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;

namespace ShoppingAssignment_SE150568.Pages.OrderPage
{
    public class EditModel : PageModel
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public EditModel(IOrderRepository _orderRepository, IOrderDetailRepository _orderDetailRepository)
        {
            this.orderDetailRepository = _orderDetailRepository;
            this.orderRepository = _orderRepository;
        }

        [BindProperty]
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role == "Customer")
            {
                return NotFound();
            }
            Order = orderRepository.GetOrderByID(id);
            Details = orderDetailRepository.GetOrderDetailByID(id);

            if (Order == null || Details == null)
            {
                return NotFound();
            }
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                orderRepository.UpdateOrder(Order);
            }
            catch (Exception)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        //private bool OrderExists(string id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}
    }
}
