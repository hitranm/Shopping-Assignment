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
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public IndexModel(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public IEnumerable<Customer> Customer { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            String email = HttpContext.Session.GetString("EMAIL");
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role == "Customer")
            {
                return NotFound();
            }
            Customer = customerRepository.GetCustomers();
            return Page();
        }
    }
}
