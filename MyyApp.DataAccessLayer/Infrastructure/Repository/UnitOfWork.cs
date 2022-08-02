using MyyApp.DataAccessLayer.Data;
using MyyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
         private ApplicationDbContext _context;
         public ICategoryRepository Category { get; private set; }
        public IProductRepository ProductDb { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            ProductDb = new ProductRepository(context);

        }
    
       
        
        public void save()
        { 
            _context.SaveChanges();
        }
    }
}
