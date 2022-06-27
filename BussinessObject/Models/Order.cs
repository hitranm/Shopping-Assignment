using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BussinessObject.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Required]
        [Display(Name ="Order ID")]
        public string OrderId { get; set; }

        [Required]
        [Display(Name ="Customer ID")]
        public string CustomerId { get; set; }
        [Required]
        [Display(Name ="Order Date")]
        public DateTime? OrderDate { get; set; }
        [Required]
        [Display(Name = "Required Date")]
        public DateTime? RequiredDate { get; set; }
        [Required]
        [Display(Name = "Shipped Date")]
        public DateTime? ShippedDate { get; set; }
        [Required]
        [Range(0, 100000000000, ErrorMessage = "You must enter freight between {1} and {2}")]
        public decimal? Freight { get; set; }
        [Required]
        [Display(Name ="Ship Address")]
        public string ShipAddress { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
