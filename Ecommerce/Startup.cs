using Microsoft.EntityFrameworkCore;
using Ecommerce.DA.Domain;
using Ecommerce.MiddleWare;
using ECommerce.BL.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;
using ECommerce.BL.Service.Interface;
using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using Polly;

namespace Ecommerce
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

			Configuration = configuration;			
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureCors();
			services.ConfigureSqlServer(Configuration);
			services.ConfigureService();

			services.AddAutoMapper(typeof(Startup));

			services.AddControllers().AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("V1", new OpenApiInfo { Title = "My API", Version = "V1" });
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//ExecuteMigrations(app,env);

			app.UseHttpsRedirection();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("V1/swagger.json", "My API V1");
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			});

		}

		private void ExecuteMigrations(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.EnvironmentName == "Testing") return;

			var retry = Policy.Handle<SqlException>()
				.WaitAndRetry(new TimeSpan[]
				{
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(6),
					TimeSpan.FromSeconds(12)
				});

			retry.Execute(() =>
				app.ApplicationServices.GetService<ShopingDatabaseContext>().Database.Migrate());
			retry.Execute(() =>
				app.ApplicationServices.GetService<ShopingDatabaseContext>().Database.EnsureCreated());
		}
	}
}
