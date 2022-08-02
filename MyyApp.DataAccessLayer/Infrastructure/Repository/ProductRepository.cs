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
    public class ProductRepository : Repository<ProductDb>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ProductDb product)
        {
            var productDb = _context.ProductDbs.FirstOrDefault(p => p.Id == product.Id);
            if (productDb != null)
            {
                productDb.Name = product.Name;
                productDb.Description = product.Description;
                productDb.Price = product.Price;
                
                if (productDb.ImageUrl != null)
                {
                    productDb.ImageUrl = productDb.ImageUrl;
                }

            }
        }
    }
}
