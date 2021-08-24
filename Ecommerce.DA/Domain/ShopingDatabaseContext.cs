using Ecommerce.DA.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.DA.Domain
{
	public class ShopingDatabaseContext : DbContext, IDbContext
	{
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProducts> OrderProducts { get; set; }
		public DbSet<ShoppingCart> ShoppingCart { get; set; }

		public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		//	optionsBuilder.UseSqlServer(@"data source=localhost;initial catalog=Product;persist security info=True;Integrated Security=SSPI;");
		}

		public ShopingDatabaseContext(DbContextOptions options) : base(options)
		{
		}
		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		private IDbContextTransaction _transaction;

		public void BeginTransaction()
		{
			_transaction = Database.BeginTransaction();
		}

		public void Commit()
		{
			try
			{
				SaveChanges();
				_transaction.Commit();
			}
			finally
			{
				_transaction.Dispose();
			}
		}

		public void Rollback()
		{
			_transaction.Rollback();
			_transaction.Dispose();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<ShoppingCart>(table =>
			{
				table.HasIndex(e => new { e.CustomerId });
			});
			
			builder.Entity<ShoppingCartItem>(table =>
			{
				table.HasKey(e => new { e.CartId,e.CustomerId, e.ProductId});
				table.HasIndex(e => new { e.CartId,e.CustomerId, e.ProductId});
			});
			
			builder.Entity<Order>(table =>
			{
				table.HasIndex(e => e.OrderStatus);
			});

			builder.Entity<OrderProducts>(table =>
			{
				table.HasIndex(e => new { e.ProductId, e.OrderId,e.CustomerId });				
				table.HasKey(e => new { e.ProductId, e.OrderId,e.CustomerId });
			});
			
			base.OnModelCreating(builder);
		}
	}
}
