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

namespace ShoppingAssignment_SE150568.Pages.ProductPage
{
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository productRepository;

        public DeleteModel(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
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
            Product = productRepository.GetProductByID(id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
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
            productRepository.DeleteProduct(id);

            return RedirectToPage("./Index");
        }
    }
}
