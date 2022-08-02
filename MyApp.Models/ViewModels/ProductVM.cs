using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Models;

namespace MyApp.Models.ViewModels
{
    public class ProductVM
    {
        public ProductDb ProductDb { get; set; } = new ProductDb();
        [ValidateNever]
        public IEnumerable<ProductDb> ProductDbs { get; set; } = new List<ProductDb>();
        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }


    }
}
