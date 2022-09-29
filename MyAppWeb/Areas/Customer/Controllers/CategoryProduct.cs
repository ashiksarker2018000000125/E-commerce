using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryProduct : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            IEnumerable <ProductDb> products = _context.ProductDbs.Where(x => x.CategoryId == id).ToList();
            return View(products);
        }
    }
}
