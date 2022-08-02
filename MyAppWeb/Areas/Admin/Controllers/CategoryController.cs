using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitofwork;


        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitofwork.Category.GetAll();
            return View(categoryVM);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.Category.Add(category);
        //        _unitofwork.save();
        //        TempData["success"] = "Catecory Added Successfully to the Category List";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}


        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vm);
            }
           
            vm.Category = _unitofwork.Category.GetT(x => x.Id == id);

            if (vm.Category == null)
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
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Category == null || vm.Category.Id == 0)
                {
                    _unitofwork.Category.Add(vm.Category);
                    _unitofwork.save();
                    TempData["success"] = "Category Create Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    _unitofwork.Category.Update(vm.Category);
                    _unitofwork.save();
                    TempData["success"] = "Category Update Successfully";
                    return RedirectToAction("Index");

                }
                
            }

            return RedirectToAction("Index");
        }


        [HttpGet]

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofwork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitofwork.Category.Delete(category);
            _unitofwork.save();
            TempData["success"] = "Category Delete Successfully";


            return RedirectToAction("Index");
        }



    }
}
