﻿using System.ComponentModel.DataAnnotations;

namespace WebAPIProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
