using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DA.Domain.Models
{
	public enum OrderStatus
	{
		OrderCreated =1,
		OrderPlaced =2,
		OrderDelivered =3,
		OrderCancelled =4,
	}
}
