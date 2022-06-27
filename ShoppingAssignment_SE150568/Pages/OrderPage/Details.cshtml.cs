using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;

namespace ShoppingAssignment_SE150568.Pages.OrderPage
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public DetailsModel(IOrderRepository _orderRepository, IOrderDetailRepository _orderDetailRepository)
        {
            this.orderDetailRepository = _orderDetailRepository;
            this.orderRepository = _orderRepository;
        }

        public Order Order { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string email = HttpContext.Session.GetString("EMAIL");
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            
            Order = orderRepository.GetOrderByID(id);
            if ((role=="Customer" && email != Order.Customer.Email))
            {
                return NotFound();
            }
            Details = orderDetailRepository.GetOrderDetailByID(id);

            if (Order == null || Details == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
