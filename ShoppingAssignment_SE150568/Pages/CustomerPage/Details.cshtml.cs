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
    public class DetailsModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public DetailsModel(ICustomerRepository _customerRepository)
        {
            this.customerRepository = _customerRepository;
        }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            string role = HttpContext.Session.GetString("ROLE");
            string email = HttpContext.Session.GetString("EMAIL");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            Customer = customerRepository.GetCustomerById(id);


            if (Customer == null)
            {
                return NotFound();
            }
            if ((role == "Customer" && email != Customer.Email))
            {
                return NotFound();
            }
            return Page();
        }
    }
}
