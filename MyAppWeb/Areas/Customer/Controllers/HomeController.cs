using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofwork;
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, 
            IUnitOfWork unitofwork,
            ApplicationDbContext context,
            INotyfService notyf

            )
        {
            _logger = logger;
            _unitofwork = unitofwork;
            _context = context;
            _notyf = notyf;
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

        [Authorize]
        public IActionResult AddToShopingCart(int productid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            ShoppingCart shoppingCart = new ShoppingCart() { };

            

            var cartItem = _context.ShoppingCarts.FirstOrDefault(x => x.ApplicationUserId == userId && x.ProductId == productid);
            if(cartItem != null)
            { 
                cartItem.count = cartItem.count + 1;
                cartItem.ApplicationUserId = userId;
                cartItem.ProductId = productid;

                _context.SaveChanges();

            }
            else
            {
                shoppingCart.ApplicationUserId = userId;
                shoppingCart.ProductId = productid;
                shoppingCart.count = 1;
                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }


            _notyf.Success("Add to Cart Successfully");

            return RedirectToAction("ShoppingCart");
        }

        [Authorize]
        public IActionResult ShoppingCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            List<ProductDb> cartList = new List<ProductDb>();

            //List<int> list = new List<int>();
            int[] array = new int[50];
            int i = 0;

            IEnumerable<ShoppingCart> shoppingCart = _context.ShoppingCarts.Where(x => x.ApplicationUserId == userId).ToList();
            if (shoppingCart != null)
            { 
                foreach (var s in shoppingCart)
                {
                    var x = _context.ProductDbs.Where(x => x.Id == s.ProductId).ToList();
                    cartList.AddRange(x);

                    //list.Add(s.count);
                    array[i] = s.count;
                    i++;
                } 
               
            }
            ViewBag.Product = array;

            return View(cartList.ToList());
        }

        public IActionResult RemoveProductFromCart(int productid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var product = _context.ShoppingCarts.Where(x => x.ProductId == productid && x.ApplicationUserId== userId);
            _context.ShoppingCarts.RemoveRange(product);
            _context.SaveChanges();
            var list = _context.ShoppingCarts;

            return RedirectToAction("ShoppingCart");
        }



        public IActionResult IncreaseShoppingCartProductCount(int productid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var product = _context.ShoppingCarts.FirstOrDefault(x => x.ProductId == productid && x.ApplicationUserId == userId);
            
            if(product != null)
            {
                product.count = product.count + 1;
                _context.SaveChanges();
            }

            return RedirectToAction("ShoppingCart");
        }


        public IActionResult DecreaseShoppingCartProductCount(int productid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var product = _context.ShoppingCarts.FirstOrDefault(x => x.ProductId == productid && x.ApplicationUserId == userId);

            if (product.count <= 1)
            {
                var countproduct = _context.ShoppingCarts.Where(x => x.ProductId == productid && x.ApplicationUserId == userId);
                _context.ShoppingCarts.RemoveRange(countproduct);
                _context.SaveChanges();
                return RedirectToAction("ShoppingCart");
            }

            if (product != null)
            {
                product.count = product.count - 1;
                _context.SaveChanges();
            }

            return RedirectToAction("ShoppingCart");
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