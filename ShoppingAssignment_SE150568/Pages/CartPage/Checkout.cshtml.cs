using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoppingAssignment_SE150568.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShoppingAssignment_SE150568.Pages.CartPage
{
    public class CheckoutModel : PageModel
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly ICustomerRepository customerRepository;

        public CheckoutModel(IOrderRepository orderRepository,
                            IOrderDetailRepository orderDetailRepository,
                            IProductRepository productRepository,
                            ICustomerRepository customerRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
        }

        public List<CartItem> Cart { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Required Date")]
        [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Shipped Date")]
        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }
        [Required]
        [BindProperty]
        [Range(0, 100000000000, ErrorMessage = "You must enter freight between {1} and {2}")]
        public decimal Freight { get; set; }
        [Required]
        [Display(Name = "Ship Address")]
        [BindProperty]
        public string ShipAddress { get; set; }

        public decimal TotalPrice { get; set; }


        public IActionResult OnGet()
        {
            string customerEmail = HttpContext.Session.GetString("EMAIL");
            if (string.IsNullOrEmpty(customerEmail))
            {
                return RedirectToPage("/Login");
            }
            var data = HttpContext.Session.GetString("CART");
            if (data != null)
            {
                Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);

            }

            if (Cart != null)
            {
                TotalPrice = GetTotalPrice(Cart);
            }
            else
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        private decimal GetTotalPrice(List<CartItem> cart)
        {
            decimal result = 0;
            foreach (CartItem item in cart)
            {
                result += item.Price * item.Quantity;
            }
            return result;
        }

        public IActionResult OnPost()
        {
            try
            {
                Customer customer;
                string customerEmail = HttpContext.Session.GetString("EMAIL");
                if (string.IsNullOrEmpty(customerEmail))
                {
                    return RedirectToPage("/Login");
                }
                else
                {
                    customer = customerRepository.GetCustomerByEmail(customerEmail);
                }

                var data = HttpContext.Session.GetString("CART");
                if (data != null)
                {
                    Cart = JsonConvert.DeserializeObject<List<CartItem>>(data);

                }
                if (!ModelState.IsValid)
                {

                    if (Cart != null)
                    {
                        TotalPrice = GetTotalPrice(Cart);
                    }
                    else
                    {
                        return RedirectToPage("./Index");
                    }
                    return Page();
                }
                var checkDate = CheckDate(DateTime.Now, (DateTime)RequiredDate, (DateTime)ShippedDate);
                if (!String.IsNullOrEmpty(checkDate))
                {
                    TotalPrice = GetTotalPrice(Cart);
                    TempData["Message"] = checkDate;
                    return Page();
                }
                var resultCheckProductQuantity = CheckInStock(Cart);
                if (!String.IsNullOrEmpty(resultCheckProductQuantity))
                {
                    TotalPrice = GetTotalPrice(Cart);
                    TempData["Message"] = resultCheckProductQuantity;
                    return Page();
                }

                Order order = new Order
                {
                    OrderId = RandomString(),
                    CustomerId = customer.CustomerId,
                    Freight = this.Freight,
                    OrderDate = DateTime.Now,
                    ShipAddress = this.ShipAddress,
                    ShippedDate = this.ShippedDate,
                    RequiredDate = this.RequiredDate
                };


                //List<Order> orderList = orderRepository.GetAllOrders().ToList();
                //while (orderList.Exists(o => o.OrderId.Equals(order.OrderId)))
                //{
                //    order.OrderId = GetRandomString.GenerateRandomString();
                //}

                //Add order 
                orderRepository.InsertOrder(order);
                //List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (CartItem cartItem in Cart)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = cartItem.ProductId,
                        Quantity = (short)cartItem.Quantity,
                        UnitPrice = cartItem.Price
                    };
                    orderDetailRepository.InsertOrderDetail(orderDetail);
                }
                //if (orderDetails.Count > 0)
                //{
                //    //Add order details
                //    orderDetailRepository.InsertOrderDetail(orderDetails);
                //}

                //Update product's quantity in stock
                UpdateProductQuantityStock(Cart);

                //Delete cart after checkout
                HttpContext.Session.Remove("CART");
            }

            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return Page();
            }
            return RedirectToPage("/OrderPage/Index");
        }


        //--------------------------------------------------------------
        private string CheckDate(DateTime orderDate, DateTime requiredDate, DateTime shippedDate)
        {
            string result = "";
            int compareRequiredWithOrderDate = DateTime.Compare(orderDate, requiredDate);
            int compareShippedWithOrderDate = DateTime.Compare(orderDate, shippedDate);
            if (compareRequiredWithOrderDate > 0 || compareShippedWithOrderDate > 0)
            {
                result = $"ShippedDate and RequiredDate cannot be earlier than OrderDate";
                //throw new Exception("ShippedDate and RequiredDate cannot be earlier than OrderDate");
            }
            int compare = DateTime.Compare(requiredDate, shippedDate);
            if (compare < 0)
            {
                result = "ShippedDate must be earlier than RequiredDate";
            }
            return result;
        }
        private void UpdateProductQuantityStock(List<CartItem> cart)
        {
            foreach (CartItem cartItem in cart)
            {
                Product product = productRepository.GetProductByID(cartItem.ProductId);
                product.QuantityPerUnit -= cartItem.Quantity;
                productRepository.UpdateProduct(product);
            }
        }

        private string CheckInStock(List<CartItem> cart)
        {
            string result = "";
            foreach (CartItem cartItem in cart)
            {
                Product product = productRepository.GetProductByID(cartItem.ProductId);
                if (product.QuantityPerUnit < cartItem.Quantity)
                {
                    result = $"The product ${product.ProductName} does not have enough quantity in stock(${product.QuantityPerUnit})";
                    break;
                }
            }
            return result;
        }

        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
