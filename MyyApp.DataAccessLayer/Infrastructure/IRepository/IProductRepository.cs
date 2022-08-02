using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyyApp.DataAccessLayer.Infrastructure.IRepository
{
    public interface IProductRepository: IRepository<ProductDb> 
    {
        void Update(ProductDb product);
    }
}
