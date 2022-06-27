using BussinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssignment_SE150568.Pages
{
    public class SaleReportModel : PageModel
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;

        public SaleReportModel(IOrderRepository _orderRepository, ICustomerRepository customerRepository)
        {
            orderRepository = _orderRepository;
            this.customerRepository = customerRepository;
        }

        public IEnumerable<Order> Order { get; set; }

        [BindProperty]
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        [BindProperty]
        public DateTime ToDate { get; set; }

        public void OnGet()
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string role = HttpContext.Session.GetString("ROLE");
            string email = HttpContext.Session.GetString("EMAIL");
            if (string.IsNullOrEmpty(role))
            {
                return RedirectToPage("/Login");
            }
            else if (role != "Admin")
            {
                return NotFound();
            }
            //Order = null;
            //FromDate = DateTime.Now.Date;
            //ToDate = DateTime.Now.Date;
            int checkDate = DateTime.Compare(FromDate, ToDate);
            if (checkDate > 0)
            {
                TempData["Message"] = "From date must be earlier than to date";
                return Page();
            }
            else
            {
                Order = orderRepository.GetOrders()
                                    .Where(o => DateTime.Compare((DateTime)o.OrderDate, (DateTime)FromDate) >= 0
                                    && DateTime.Compare((DateTime)o.OrderDate, (DateTime)ToDate) <= 0).OrderByDescending(o => o.OrderDate);
            }
            return Page();
        }

        //public async Task<IActionResult> OnGetReport()
        //{
        //    int checkDate = DateTime.Compare(FromDate, ToDate);
        //    if (checkDate > 0)
        //    {
        //        TempData["Message"] = "From date must be earlier than to date";
        //        return Page();
        //    }
        //    Order = orderRepository.GetOrders()
        //            .Where(o => DateTime.Compare((DateTime)o.OrderDate, (DateTime)FromDate) >= 0
        //            && DateTime.Compare((DateTime)o.OrderDate, (DateTime)ToDate) <= 0).OrderByDescending(o => o.OrderDate);
        //    return Page();
        //}
    }
}
