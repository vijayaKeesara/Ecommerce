using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	[Table("Product")]
	public class Product 
	{		
		[Key]
		public int ProductId { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[StringLength(200)]
		public string Description { get; set; }
		public string Rating { get; set; }

		[Required]
		[StringLength(100)]
		public string Category { get; set; }

		[Required]
		[DisplayName("Price")]
		[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
		public decimal UnitPrice { get; set; }

		[Required]
		public int UnitsInStock { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }		
	}
}
