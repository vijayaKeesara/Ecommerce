using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	public class ProductStock
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }
		[Required]
		[DisplayName("Price")]
		[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
		public decimal UnitPrice { get; set; }
		[Required]
		public int Quantity { get; set; }

		public ProductStock Product { get; set; }
	}
}
