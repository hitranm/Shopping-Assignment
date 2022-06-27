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
    public class IndexModel : PageModel
    {
        private readonly IProductRepository productRepository;
        public IndexModel(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        public IEnumerable<Product> Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            Product = productRepository.GetProducts();
            string role = HttpContext.Session.GetString("ROLE");
            if (role == "Customer" || role == null)
            {
                Product = Product.Where(p => p.ProductStatus == 1).ToList();
            }

        }

        //public IActionResult OnGetSearch([FromForm] string searchVSealue)
        //{
        //    if (!String.IsNullOrEmpty(SearchString))
        //    {
        //        Product = Product.Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim())).ToList();
        //        string role = HttpContext.Session.GetString("ROLE");
        //        if (role == "Customer" || role == null)
        //        {
        //            Product = Product.Where(p => p.ProductStatus == 1).ToList();
        //        }
        //    }
        //    else
        //    {
        //        Product = productRepository.GetProducts();

        //    }

        //    return Page();
        //}
    }
}
