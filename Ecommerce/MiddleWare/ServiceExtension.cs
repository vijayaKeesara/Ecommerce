using Microsoft.EntityFrameworkCore;
using Ecommerce.DA.Domain;
using ECommerce.BL.Service;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BL.Service.Interface;

namespace Ecommerce.MiddleWare
{
	public static class ServiceExtension
	{
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowOrigin",
				builder =>
				{
					builder.WithOrigins("http://localhost:44392",
										"http://localhost").AllowAnyMethod();
				});
			});
		}		

		public static void ConfigureService(this IServiceCollection services)
		{
			services.AddSingleton<ILoggerManager, LoggerManager>();
			services.AddScoped<IDbContext, ShopingDatabaseContext>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IShoppingCartService, ShoppingCartService>();
			services.AddScoped<ICustomerService, CustomerService>();
		}

		public static void ConfigureSqlServer(this IServiceCollection services,IConfiguration config)
		{
			var connectionString = config["DefaultConnection:connectionString"];
			services.AddEntityFrameworkSqlServer().AddDbContext<ShopingDatabaseContext>(options =>
				options.UseSqlServer(connectionString));
		}
	}
}
