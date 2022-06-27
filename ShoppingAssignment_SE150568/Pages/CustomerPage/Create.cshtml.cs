using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;

namespace ShoppingAssignment_SE150568.Pages.CustomerPage
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public CreateModel(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        public IActionResult OnGet()
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
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
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

            if (customerRepository.GetCustomerById(Customer.CustomerId) != null)
            {
                ModelState.AddModelError("Customer.CustomerId", "This ID is already existed");
            }
            if (customerRepository.GetCustomerByEmail(Customer.Email) != null)
            {
                ModelState.AddModelError("Customer.Email", "This email has been used!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                customerRepository.AddCustomer(Customer);
            }



            return RedirectToPage("./Index");
        }
    }
}
