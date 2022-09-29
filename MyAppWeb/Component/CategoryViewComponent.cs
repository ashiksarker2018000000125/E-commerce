using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyyApp.DataAccessLayer.Data;

namespace MyAppWeb.Component
{
    public class CategoryViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CategoryViewComponent(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Category> categorylist = _context.Categories; 
            return View("Index", _context.Categories.ToList());
        }
    }
}
