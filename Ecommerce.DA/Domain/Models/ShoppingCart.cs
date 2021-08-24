using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	[Table("ShoppingCart")]
	public class ShoppingCart
	{		
		[Key]
		public int CartId { get; set; }

		public int CustomerId { get; set; }

		public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
	}
}
