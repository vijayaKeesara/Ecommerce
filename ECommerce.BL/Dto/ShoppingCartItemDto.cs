using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BL.Dto
{
    public partial class ShoppingCartItemDto
    {

        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
      
        public bool IsAvailable { get; set; }

        public decimal SubTotal { get; set; }
    }
}
