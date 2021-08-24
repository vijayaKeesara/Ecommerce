using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.BL.Dto
{
	public class ProductDto 
	{
		public int ProductId { get; set; }
		[StringLength(100)]
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		public string Rating { get; set; }
		public string Category { get; set; }

		[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
		public decimal UnitPrice { get; set; }

		public int UnitsInStock { get; set; }

	}
}
