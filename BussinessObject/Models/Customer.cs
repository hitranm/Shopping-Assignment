using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BussinessObject.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }
        [Required]
        [Key]
        [Display(Name = "ID")]
        public string CustomerId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage ="Password length must be from 6 to 50 charactor", MinimumLength =6)]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Contact Name")]
        [StringLength(50, ErrorMessage = "Name length must be from 6 to 50 charactor", MinimumLength = 6)]
        public string ContactName { get; set; }
        public string Address { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number must contain 10 characters", MinimumLength = 10)]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please input correct email address")]
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
