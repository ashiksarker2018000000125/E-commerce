using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyyApp.DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ImageUpload> ImageUploads { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<ProductDb> ProductDbs { get; set; }
    }

}
