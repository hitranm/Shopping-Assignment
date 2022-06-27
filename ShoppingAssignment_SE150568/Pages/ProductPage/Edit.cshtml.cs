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
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShoppingAssignment_SE150568.Pages.ProductPage
{
    public class EditModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISupplierRepository supplierRepository;
        private IHostingEnvironment _environment;


        public EditModel(IHostingEnvironment environment, IProductRepository productRepository, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.supplierRepository = supplierRepository;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        //[Required(ErrorMessage = "Please choose an image")]
        [Display(Name = "Choose an image to upload")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions ="png,jpg,jpeg")]
        public IFormFile ImageUpload { get; set; }

        [BindProperty]
        [Required]
        [Display(Name ="Active?")]
        public bool ProductStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            Product = productRepository.GetProductByID((int)id);
            ProductStatus = bool.Parse(Product.ProductStatus == 1 ? true.ToString() : false.ToString());

            if (Product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetSuppliers(), "SupplierId", "CompanyName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
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

            try
            {
                Product.ProductStatus = byte.Parse(ProductStatus ? 1.ToString() : 0.ToString());
                productRepository.UpdateProduct(Product);
            }
            catch (Exception ex)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}
    }
}
