using Ecommerce.DA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BL.Service.Interface
{
	public interface ICustomerService : IEntityService<Customer>
	{
		Task<Customer> GetCustomerByIdAsync(int customerId);	
	}
}
