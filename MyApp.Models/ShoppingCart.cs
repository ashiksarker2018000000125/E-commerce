using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        //[ValidateNever]
        //public ProductDb ProductDb { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int count { get; set; }
    }
}
