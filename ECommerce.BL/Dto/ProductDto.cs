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
		[Required]
		public string Category { get; set; }

		[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
		[DataType(DataType.Currency)]
		public decimal UnitPrice { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
		public int UnitsInStock { get; set; }

	}
}
