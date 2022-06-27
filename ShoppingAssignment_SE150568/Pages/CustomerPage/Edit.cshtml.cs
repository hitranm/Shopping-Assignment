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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShoppingAssignment_SE150568.Pages.CustomerPage
{
    public class EditModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public EditModel(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }
        [BindProperty]
        public String CustomerId { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password length must be from 6 to 50 charactor", MinimumLength = 6)]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Contact Name")]
        [StringLength(50, ErrorMessage = "Name length must be from 6 to 50 charactor", MinimumLength = 6)]
        public string ContactName { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number must contain 10 characters", MinimumLength = 10)]
        [Display(Name = "Phone number")]
        [BindProperty]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please input correct email address")]
        [BindProperty]
        public string Email { get; set; }
        //public Customer customer { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
           
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            if ((role == "Customer" && email != customer.Email))
            {
                return NotFound();
            }
            CustomerId = id;
            ContactName = customer.ContactName;
            Address = customer.Address;
            Phone = customer.Phone;
            Email = customer.Email;


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
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
            
            if (String.IsNullOrEmpty(Password))
            {
                Password = customerRepository.GetCustomerById(CustomerId).Password;
            }
            if (customerRepository.GetCustomerByEmail(Email) != null && customerRepository.GetCustomerById(CustomerId).Email != Email)
            {
                ModelState.AddModelError("Email", "This email has been used!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {

                customerRepository.UpdateCustomer(
                    new Customer
                    {
                        CustomerId = CustomerId,
                        Password = Password,
                        Phone = Phone,
                        Email = Email,
                        Address = Address,
                        ContactName = ContactName
                    });

            }
            catch (Exception ex)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        //private bool CustomerExists(string id)
        //{
        //    return _context.Customers.Any(e => e.CustomerId == id);
        //}
    }
}
