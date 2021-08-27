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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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
			services.AddTransient<IDbContext, ShopingDatabaseContext>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IShoppingCartService, ShoppingCartService>();
			services.AddScoped<ICustomerService, CustomerService>();
		}

		public static void ConfigureSqlServer(this IServiceCollection services,IConfiguration config)
		{
			var connectionString = config["DefaultConnection:connectionString"];
			services.AddEntityFrameworkSqlServer().AddDbContext<ShopingDatabaseContext>(options =>
				options.UseSqlServer(connectionString), ServiceLifetime.Transient);
		}

		public static IServiceCollection ConfigSwagger(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddSwaggerGen(swagger =>
			{
				//This is to generate the Default UI of Swagger Documentation
				swagger.SwaggerDoc("V1", new OpenApiInfo
				{
					Version = "V1",
					Title = "JWT Token Authentication API",
					Description = "ASP.NET Core 3.1 Web API"
				});
				// To Enable authorization using Swagger (JWT)
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}

					}
				});
			});
			return services;
		}
		public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					//ValidateLifetime = false,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
				};
			});
			return services;
		}
	}
}
