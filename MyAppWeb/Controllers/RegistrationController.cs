using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;

namespace MyAppWeb.Controllers
{
    public class RegistrationController : Controller
    {
        private ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registration() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration( Registration registration)
        {
            if (ModelState.IsValid){


                if (_context.Registrations.Any( x=> x.Email == registration.Email)) {
                    TempData["error"] = "Email Allready Taken";
                    return View();
                }
                if (registration.Password != registration.ConfirmPassword)
                {
                    TempData["error"] = "Password Not Match";
                    return View();
                }

                _context.Registrations.Add(registration);
                _context.SaveChanges();

                HttpContext.Session.SetString("username", "Ashik");
                var x = HttpContext.Session.GetString("username");
                Console.WriteLine(x);
                TempData["error"] = x;

                //HttpContext.Session.SetString("username", registration.Email);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Registration registration)
        {



            if(ModelState.IsValid){

                
                var checklogin = _context.Registrations.Where(x => x.Email.Equals(registration.Email) && x.Password.Equals(registration.Password));
               if (checklogin != null)
                {
                    HttpContext.Session.SetString("username", registration.Email);
                }
            }

            return View();
        }
    }
}
