using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public interface IOrderService : IEntityService<Order>
	{	
		Task<Order> GetOrderByIdAsync(int orderId);
		Task<Order> GetOrderByCustomOrderNumberAsync(string customOrderNumber);

		Task<List<string>> ChekOutOrderAsync(int customerId,int storeId);
	}
}
