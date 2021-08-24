using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public class CustomerService : EntityService<Customer>,ICustomerService
	{
		private ShopingDatabaseContext _context;
		public CustomerService(ShopingDatabaseContext context) : base(context)
		{
			_context = context;
		}

		//public virtual async Task InsertAsync(Customer customer, bool savechanges = false)
		//{
		//	await _context.Customers.AddAsync(customer);
		//	if (savechanges)
		//		await _context.SaveChangesAsync();
		//}

		//public virtual async Task UpdateAsync(Customer customer, bool savechanges = false)
		//{
		//	 _context.Customers.Update(customer);
		//	if (savechanges)
		//		await _context.SaveChangesAsync();
		//}
		public virtual async Task<Customer> GetCustomerByIdAsync(int customerId)
		{
			return await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == customerId);
		}
	}
}
