using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModels
{
    public class Cart
    {
        public ProductDb ProductDb { get; set; }
        public int Count { get; set; }
    }
}
