﻿using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        public DateTime CreateTimeDate { get; set; } = DateTime.Now;

    }
}
