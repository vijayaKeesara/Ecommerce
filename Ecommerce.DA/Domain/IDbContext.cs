using Ecommerce.DA.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.DA.Domain
{
	public interface IDbContext
	{
		DbSet<Customer> Customers { get; set; }
		DbSet<Product> Products { get; set; }
		DbSet<Order> Orders { get; set; }
		DbSet<OrderProducts> OrderProducts { get; set; }
		DbSet<ShoppingCart> ShoppingCart { get; set; }
		DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }

		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}
