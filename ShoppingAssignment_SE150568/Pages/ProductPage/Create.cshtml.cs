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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ShoppingAssignment_SE150568.Pages.ProductPage
{
    public class CreateModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISupplierRepository supplierRepository;
        private IHostingEnvironment _environment;

        public CreateModel(IProductRepository _productRepository, ICategoryRepository _categoryRepository, ISupplierRepository _supplierRepository, IHostingEnvironment environment)
        {
            this.productRepository = _productRepository;
            categoryRepository = _categoryRepository;
            supplierRepository = _supplierRepository;
            _environment = environment;
        }

        public IActionResult OnGet()
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
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please choose an image")]
        [Display(Name = "Choose an image to upload")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions ="png,jpg,jpeg")]
        public IFormFile ImageUpload { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Active?")]
        public bool ProductStatus { get; set; }

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
            if (ImageUpload != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot/Images",
                    ImageUpload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                    Product.ProductImage = ImageUpload.FileName;
                }
            }
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
                ViewData["SupplierId"] = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
                return Page();
            }
            else
            {
                Product.ProductStatus = byte.Parse(ProductStatus ? 1.ToString() : 0.ToString());
                productRepository.InsertProduct(Product);
            }


            return RedirectToPage("./Index");
        }
    }
}
