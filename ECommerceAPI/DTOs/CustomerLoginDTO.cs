﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class CustomerLoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
