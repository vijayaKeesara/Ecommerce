using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.BL.Dto
{
    public partial class ShoppingCartItemDto
    {

        public int CartId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Quantity { get; set; }
      
        public bool IsAvailable { get; set; }

        public decimal SubTotal { get; set; }
    }
}
