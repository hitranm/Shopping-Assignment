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

namespace ShoppingAssignment_SE150568.Pages.CustomerPage
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;

        public DeleteModel(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role != "Admin")
            {
                return NotFound();
            }
            Customer = customerRepository.GetCustomerById(id);

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role != "Admin")
            {
                return NotFound();
            }
            if (orderRepository.GetOrdersOfMember(id) == null)
            {
                customerRepository.DeleteCustomer(id);
            }
            else
            {
                TempData["Message"] = "Cannot delete member that has orders";
                return RedirectToAction("Delete", new { id = id });
            }

            return RedirectToPage("./Index");
        }
    }
}
