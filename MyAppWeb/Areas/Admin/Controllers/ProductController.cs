using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly INotyfService _notyf;

        public ProductController(
            IUnitOfWork unitofwork,
            IWebHostEnvironment hostEnvironment,
            INotyfService notyf,
            ApplicationDbContext context
            )
        {
            _unitofwork = unitofwork;
            _hostEnvironment = hostEnvironment;
            _notyf = notyf;
            _context = context;
        }

        #region APICALL
        public IActionResult AllProducts()
        {
            IEnumerable<ProductDb> products = _unitofwork.ProductDb.GetAll(includeProperties:"Category");
            //var x = _context.ProductDbs.Include(x => x.Category);
            return Json(new { data = products });

        }
        #endregion

        public IActionResult Index()
        {
            IEnumerable<ProductDb> products = _unitofwork.ProductDb.GetAll();
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

                if ( vm.ProductDb.Id == 0)//Create er somoy id==0 ashe ..Create process
                {
                    _unitofwork.ProductDb.Add(vm.ProductDb);
                    _unitofwork.save();
                    _notyf.Success("Product Carate successfully");
                }
                else //Update er somoy id!=0 ashe ..Update process
                {
                    _unitofwork.ProductDb.Update(vm.ProductDb);
                    _unitofwork.save();
                    _notyf.Success("Product Update successfully");
                }
                
                return RedirectToAction("Index");


            }

            return RedirectToAction("Index","Category");
        }


        //use for delete 

        //public IActionResult Delete(int? id)
        //{
        //    if (id==null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var deletecheck = _unitofwork.ProductDb.GetT(x => x.Id == id);
        //    if(deletecheck != null )
        //    {
        //        _unitofwork.ProductDb.Delete(deletecheck);
        //        _unitofwork.save();
        //        return RedirectToAction("Index", "Product");

        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //}


        #region APICALL DELETE
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
           
            var deletecheck = _unitofwork.ProductDb.GetT(x => x.Id == id);
            if (deletecheck == null)
            {
                _notyf.Error("Error in fetching data");
                return Json(new {success=false});

            }
            else
            {
                if (deletecheck.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, deletecheck.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitofwork.ProductDb.Delete(deletecheck);
                _unitofwork.save();
                _notyf.Success("delete Successfully");
                return Json(new { success = true });
            }

        }
        #endregion
    }
}
