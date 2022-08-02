using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class ImageUpload
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }
    }
}
