using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.BL.Dto
{
	public class CustomerDto
	{
		public int CustomerId { get; set; }
		[StringLength(100)]
		[Required]
		public string FirstName { get; set; }
		[StringLength(100)]
		public string LastName { get; set; }

		[StringLength(150)]
		[Required]
		public string Email { get; set; }
	}
}
