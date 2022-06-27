using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoppingAssignment_SE150568.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingAssignment_SE150568.Pages.CartPage
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository productRepository;
        public IndexModel(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        public List<CartItem> Cart { get; set; }
        public decimal TotalPrice { get; set; }
        public IActionResult OnGet()
        {
            //if (HttpContext.Session.GetString("EMAIL") == null)
            //{
            //    return RedirectToPage("/Login");                
            //}
            //List<CartItem> cart = null;
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);
                foreach(var item in Cart)
                {
                    TotalPrice += (item.Price * item.Quantity);
                }
            }
            
            return Page();
        }
        public IActionResult OnGetIncrease(int id)
        {
            //if (HttpContext.Session.GetString("EMAIL") == null)
            //{
            //    return RedirectToPage("/Login");
            //}
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);
            }
            if (Cart == null)
            {
                return RedirectToPage();
            }

            CartItem cartItem = Cart.FirstOrDefault(i => i.ProductId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            Product product = productRepository.GetProductByID(id);
            if (product.QuantityPerUnit == 0 || product.QuantityPerUnit < cartItem.Quantity + 1)
            {
                TempData["Message"] = "This product is not enough in stock.";
            }
            else
            {
                cartItem.Quantity += 1;
                foreach (var item in Cart)
                {
                    TotalPrice += (item.Price * item.Quantity);
                }
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(Cart));
            }
            return RedirectToPage();
        }
        public IActionResult OnGetDecrease(int id)
        {
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);

            }
            if (Cart == null)
            {
                return RedirectToPage();
            }

            CartItem cartItem = Cart.FirstOrDefault(i => i.ProductId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            Product product = productRepository.GetProductByID(id);
            if (cartItem.Quantity - 1 == 0)
            {
                OnPostRemove(id);
            }
            else
            {
                cartItem.Quantity -= 1;
                //cartItem.Price = (decimal)product.UnitPrice;
                foreach (var item in Cart)
                {
                    TotalPrice += (item.Price * item.Quantity);
                }
                HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(Cart));
            }
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(int productId)
        {
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);

            }

            if (Cart != null)
            {
                if (Cart.Exists(i => i.ProductId == productId))
                {
                    var item = Cart.FirstOrDefault(i => i.ProductId == productId);
                    Cart.Remove(item);
                    if (Cart.Count > 0)
                    {
                        foreach (var itemCart in Cart)
                        {
                            TotalPrice += (itemCart.Price * itemCart.Quantity);
                        }
                        HttpContext.Session.SetString("CART", JsonConvert.SerializeObject(Cart));
                    }
                    else
                    {
                        HttpContext.Session.Remove("CART");
                    }
                }
            }
            return RedirectToPage();
        }
    }
}
