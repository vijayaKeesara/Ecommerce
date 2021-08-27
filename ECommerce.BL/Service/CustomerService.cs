using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public class CustomerService : EntityService<Customer>, ICustomerService
	{
		private readonly ShopingDatabaseContext _context;
		private readonly IServiceScopeFactory _scopeFactory;
		public CustomerService(ShopingDatabaseContext context, IServiceScopeFactory scopeFactory) : base(context)
		{
			_context = context;
			_scopeFactory = scopeFactory;
		}

		//public virtual async Task<Customer> AddAsync(Customer customer, bool savechanges = false)
		//{
		//	await _context.Customers.AddAsync(customer);
		//	if (savechanges)
		//		await _context.SaveChangesAsync();
		//	return customer;
		//}

		//public virtual async Task<Customer> UpdateAsync(Customer customer, bool savechanges = false)
		//{
		//	_context.Customers.Update(customer);
		//	if (savechanges)
		//		await _context.SaveChangesAsync();
		//	return customer;
		//}
		public async Task<Customer> GetCustomerByIdAsync(int customerId)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<ShopingDatabaseContext>();
				return await db.Customers.FirstOrDefaultAsync(item => item.CustomerId == customerId);
			}
		}
	}
}
