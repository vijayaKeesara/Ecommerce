using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	[Table("Orders")]
	public class Order 
	{		
		[Key]
		public int OrderId { get; set; }
		[Required]
		[StringLength(100)]
		public string OrderNo { get; set; }

		public string Address { get; set; }

		public OrderStatus OrderStatus { get; set; }
		public DateTime OrderDate { get; set; }

		public int CustomerId { get; set; }
		public virtual List<OrderProducts> OrderProducts { get; set; }
	}
}
