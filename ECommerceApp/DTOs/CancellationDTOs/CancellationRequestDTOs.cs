﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs.CancellationDTOs
{

    public class CancellationRequestDTOs
    {
        [Required(ErrorMessage = "Customer Id is required.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Cancellation reason is required.")]
        [StringLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters.")]
        public string Reason { get; set; }
    }
}


