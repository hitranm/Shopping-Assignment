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
using System.ComponentModel.DataAnnotations;

namespace ShoppingAssignment_SE150568.Pages.OrderPage
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;

        public IndexModel(IOrderRepository _orderRepository, ICustomerRepository customerRepository)
        {
            orderRepository = _orderRepository;
            this.customerRepository = customerRepository;
        }

        public IEnumerable<Order> Order { get; set; }

        [BindProperty]
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        [BindProperty]
        public DateTime ToDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string role = HttpContext.Session.GetString("ROLE");
            string email = HttpContext.Session.GetString("EMAIL");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role == "Admin")
            {
                Order = orderRepository.GetOrders();
            }
            else
            {
                Customer customer = customerRepository.GetCustomerByEmail(email);
                Order = orderRepository.GetOrdersOfMember(customer.CustomerId);
            }
            return Page();
        }

        public async Task<IActionResult> OnGetReport()
        {
            int checkDate = DateTime.Compare(FromDate, ToDate);
            if (checkDate > 0)
            {
                TempData["Message"] = "From date must be earlier than to date";
                return Page();
            }
            Order = orderRepository.GetOrders()
                    .Where(o => DateTime.Compare((DateTime)o.OrderDate, (DateTime)FromDate) >= 0
                    && DateTime.Compare((DateTime)o.OrderDate, (DateTime)ToDate) <= 0).OrderByDescending(o => o.OrderDate);
            return Page();
        }
    }
}
