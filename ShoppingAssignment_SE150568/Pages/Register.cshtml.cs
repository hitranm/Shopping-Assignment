using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ShoppingAssignment_SE150568.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public RegisterModel(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }
        [BindProperty]
        public Customer Customer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
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

            return RedirectToPage("/Login");
        }

    }
}
