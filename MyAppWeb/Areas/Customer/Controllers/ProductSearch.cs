using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductSearch : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitofwork;
        private readonly INotyfService _notyf;
        public ProductSearch(
            IUnitOfWork unitofwork,
            INotyfService notyf,
            ApplicationDbContext context
            )
        {
            _unitofwork = unitofwork;
            _notyf = notyf;
            _context = context;

        }
        public IActionResult Search(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<ProductDb> searchProduct = _context.ProductDbs.Where(s => s.Name!.Contains(searchString)).ToList();
                return View(searchProduct);
            }
            _notyf.Warning("Enter Keyword to Search");

            return View();
        }
    }
}
