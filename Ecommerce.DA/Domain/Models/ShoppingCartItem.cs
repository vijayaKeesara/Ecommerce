using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	[Table("ShoppingCartItem")]
	public partial class ShoppingCartItem 
	{
		[Key]
		public int Id { get; set; }

		public int CustomerId { get; set; }

		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; }

		[ForeignKey("ShoppingCart")]
		public int CartId { get; set; }

		public virtual ShoppingCart ShoppingCart { get; set; }

		public virtual Product Product { get; set; }
		public virtual Customer Customer { get; set; }

	}
}

