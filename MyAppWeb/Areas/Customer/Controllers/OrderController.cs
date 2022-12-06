using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyAppWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public OrderController(ApplicationDbContext context,INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddShippingDetails()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var carts = _context.ShoppingCarts.Where(x => x.ApplicationUserId == userId).ToList();

            if(carts.Count != 0)
            {
                OrderHeader orderHeader = new OrderHeader();
                return View(orderHeader);
            }
            else
            {
                _notyf.Warning("You don't have any product in shopping cart");
                return RedirectToAction("Index", "Home", "Customer");
            }
            
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddShippingDetails(OrderHeader order)
        {
            OrderHeader orderHeader = new OrderHeader();
            orderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var carts = _context.ShoppingCarts.Where( x => x.ApplicationUserId == userId).ToList();

            foreach(var cart in carts)
            {
                var productprice = _context.ProductDbs.Find(cart.ProductId);
                orderHeader.OrderTotal += (cart.count * productprice.Price);
            }

            
            orderHeader.OrderStatus = OrderStatus.StatusPending;
            orderHeader.PaymentStatus = PaymentStatus.StatusPending;
            orderHeader.DateOfOrder = DateTime.Now;
            orderHeader.ApplicationUserId = userId;
            orderHeader.Name = order.Name;
            orderHeader.StreetAddress = order.StreetAddress;
            orderHeader.City = order.City;
            orderHeader.PhoneNumber = order.PhoneNumber;

            _context.orderHeaders.Add(orderHeader);
            _context.SaveChanges();


            //now for add to OrderDetail Table
            foreach(var cart in carts)
            {
                var productprice = _context.ProductDbs.Find(cart.ProductId);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = orderHeader.Id,
                    Price = productprice.Price,
                    Count = cart.count
                };
                _context.orderDetails.Add(orderDetail);
                _context.SaveChanges();
            }



            //var domain = "https://localhost:7118/";
            //var options = new SessionCreateOptions
            //{
            //    LineItems = new List<SessionLineItemOptions>(),
            //    Mode = "payment",
            //    SuccessUrl = domain +$"Customer/Order/OderSuccess?id={orderHeader.Id}", //  Area/Controller/Action
            //    CancelUrl = domain + $"Customer/Home/ShoppingCart",
            //};


            //foreach(var item in carts)
            //{
            //    var product = _context.ProductDbs.Find(item.ProductId);
            //    var lineItemsoptions = new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            UnitAmount = (long)product.Price,
            //            Currency = "BDT",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions
            //            {
            //                Name = product.Name,
            //            },

            //        },
            //        Quantity = item.count,
                    
            //    };
            //    options.LineItems.Add(lineItemsoptions);
            //}


            //var service = new SessionService();
            //Session session = service.Create(options);

            //Response.Headers.Add("Location", session.Url);
            //return new StatusCodeResult(303);





            _context.ShoppingCarts.RemoveRange(carts);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home", "Customer");
        }
    }
}
