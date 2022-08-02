using MyApp.Models;
using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyyApp.DataAccessLayer.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }

        public void Update(Category category)
        {
            var categoryDb = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (categoryDb != null)
            {
                categoryDb.Name = category.Name ;
                categoryDb.DisplayOrder = category.DisplayOrder ;
            }
        }
    }
}
