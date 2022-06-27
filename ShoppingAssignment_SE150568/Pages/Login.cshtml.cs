using BussinessObject;
using BussinessObject.Models;
using System.IO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingAssignment_SE150568.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;
        public LoginModel(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        [BindProperty]
        [Required]
        [EmailAddress(ErrorMessage = "Wrong format for email address")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(string? msg)
        {
            if (msg != null)
            {
                ViewData["Message"] = "Register successfully";
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var adminAccount = Program.Configuration.GetSection("DefaultAccount").Get<DefaultAccount>();
                string email = adminAccount.Email;
                string password = adminAccount.Password;
                Customer customer = customerRepository.GetCustomerByEmail(Email);
                if (customer != null && customer.Password == Password)
                {
                    HttpContext.Session.SetString("EMAIL", customer.Email);
                    HttpContext.Session.SetString("CUSTID", customer.CustomerId);
                    HttpContext.Session.SetString("ROLE", "Customer");
                    //HttpContext.Session.SetString("CART", null);
                    return RedirectToPage("./Index");

                }
                else if (Email == email && Password == password)
                {
                    HttpContext.Session.SetString("EMAIL", adminAccount.Email);
                    HttpContext.Session.SetString("ROLE", adminAccount.Role);
                    return RedirectToPage("./Index");

                }
                else
                {
                    ViewData["ErrorMessage"] = "Wrong email or password";
                    return Page();
                }

            }
            else
            {
                return Page();
            }
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("EMAIL");
            HttpContext.Session.Remove("ROLE");
            HttpContext.Session.Remove("CART");
            return RedirectToPage("/Index");
        }
    }
}
