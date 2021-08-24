using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	[Table("OrderProducts")]
    public class OrderProducts 
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }   
		public DateTime EstimatedDeliveryDate { get; set; }

		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
		public virtual Customer Customer { get; set; }
	}
}
