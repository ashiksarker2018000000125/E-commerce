using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofwork;
        private ApplicationDbContext _context;
        private IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitofwork, ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _unitofwork = unitofwork;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        #region APICALL
        public IActionResult AllProducts()
        {
            IEnumerable<ProductDb> products = _unitofwork.ProductDb.GetAll(includeProperties:"Category");
            return Json(new { data = products });

        }
        #endregion

        public IActionResult Index()
        {
            IEnumerable<ProductDb> products = _context.ProductDbs;
            return View(products);
        }


        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                ProductDb = new (),
                Categories = _unitofwork.Category.GetAll().Select(x=> new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            //ViewBag.list = _unitofwork.Category.GetAll().Select(x => new SelectListItem()
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});

            if (id == null || id == 0)
            {
                return View(vm);
            }

            vm.ProductDb = _unitofwork.ProductDb.GetT(x => x.Id == id); 

            if (vm.ProductDb == null)
            {
                return NotFound();
            }
            else
            {
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM vm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (file != null) 
                {
                    string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "ProductImage");
                    filename = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir,filename);

                    if(vm.ProductDb.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, vm.ProductDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.ProductDb.ImageUrl = @"\ProductImage\" + filename;

                }

                if ( vm.ProductDb.Id == 0)//Edit er somoy id==0 ashe ..Edit process
                {
                    _unitofwork.ProductDb.Add(vm.ProductDb);
                    _unitofwork.save();
                }
                else //Update er somoy id!=0 ashe ..Update process
                {
                    _unitofwork.ProductDb.Update(vm.ProductDb);
                    _unitofwork.save();
                }
                
                return RedirectToAction("Index");


            }

            return RedirectToAction("Index","Category");
        }


        public IActionResult Delete(int? id)
        {
            if (id==null || id == 0)
            {
                return NotFound();
            }
            var deletecheck = _unitofwork.ProductDb.GetT(x => x.Id == id);
            if(deletecheck != null )
            {
                _unitofwork.ProductDb.Delete(deletecheck);
                _unitofwork.save();
                return RedirectToAction("Index", "Product");

            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
