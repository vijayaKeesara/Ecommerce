using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	public class Customer 
	{		
		[Key]
		public int CustomerId { get; set; }
		[StringLength(100)]
		public string FirstName { get; set; }
		[StringLength(100)]
		public string LastName { get; set; }

		[StringLength(150)]
		public string Email { get; set; }		
	}
}
