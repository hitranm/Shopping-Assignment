using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BussinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [Required]
        [Display(Name ="Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name ="Product Name")]
        [StringLength(40, ErrorMessage = "Max length of product name is 40 characters")]
        public string ProductName { get; set; }

        [Required]
        public int? SupplierId { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Display(Name ="Quantity")]
        [Required]
        [Range(0,100, ErrorMessage ="You must enter a number between {1} and {2} for {0}")]
        public int? QuantityPerUnit { get; set; }

        [Display(Name ="Price")]
        [Required]
        [Range(0, 100000000000, ErrorMessage ="You must enter price between {1} and {2}")]
        public decimal? UnitPrice { get; set; }

        [Display(Name ="Product Image")]
        public string ProductImage { get; set; }

        [Display(Name ="Status")]
        public byte? ProductStatus { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
