using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BussinessObject.Models
{
    public partial class OrderDetail
    {
        [Required]
        [Display(Name ="Order ID")]
        public string OrderId { get; set; }
        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Required]
        public short Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
