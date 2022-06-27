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
using ShoppingAssignment_SE150568.ViewModel;
using Newtonsoft.Json;

namespace ShoppingAssignment_SE150568.Pages.ProductPage
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository productRepository;

        public DetailsModel(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }
        [BindProperty]
        public Product Product { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string role = HttpContext.Session.GetString("ROLE");

            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Product = productRepository.GetProductByID((int)id);
                if (Product == null)
                {
                    return NotFound();
                }
                else if (role!="Admin" && Product.ProductStatus==0)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPostAddToCart([FromForm] int productId)
        {
            Product product = productRepository.GetProductByID(productId);
            if (product == null)
            {
                return NotFound();
            }
            if (product.QuantityPerUnit == 0)
            {
                ViewData["Message"] = "This product is not enough in stock";
                return RedirectToPage();
            }
            var cart = new List<CartItem>();
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(data);
            }
            
            var item = new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = (decimal)product.UnitPrice,
                Quantity = 1,
            };
            if (cart != null)
            {
                if (cart.Exists(i => i.ProductId == product.ProductId))
                {
                    int index = cart.FindIndex(i => i.ProductId == product.ProductId);
                    if (product.QuantityPerUnit < cart[index].Quantity + 1)
                    {
                        TempData["Message"] = "The product is not enough in stock";
                    }
                    else
                    {
                        cart[index].Quantity += 1;
                        cart[index].Price = (decimal)product.UnitPrice;
                        TempData["Message"] = "Successfully add product to cart";
                    }
                }
                else
                {
                    cart.Add(item);
                    TempData["Message"] = "Successfully add product to cart";

                }
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(cart));
            }
            else
            {
                var list = new List<CartItem>();
                list.Add(item);
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(list));
                TempData["Message"] = "Successfully add product to cart";

            }
            return RedirectToPage("/CartPage/Index");
        }
    }
}

