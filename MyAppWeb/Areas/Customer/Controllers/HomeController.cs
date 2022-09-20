using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using System.Diagnostics;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofwork;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, 
            IUnitOfWork unitofwork,
            ApplicationDbContext context

            )
        {
            _logger = logger;
            _unitofwork = unitofwork;
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductDb> products = _unitofwork.ProductDb.GetAll();
            return View(products);
        }

        public IActionResult Details(int? id)
        {
            Cart cart = new Cart() { 
                ProductDb = _unitofwork.ProductDb.GetT(x=> x.Id==id, includeProperties:"Category"),
                Count =1
            };
            return View(cart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}