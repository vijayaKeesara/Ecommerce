using AutoMapper;
using Bogus;
using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Tests
{
	public class ProductServiceTest
	{
		Mock<IMapper> _mapper;
		string connectionString = "data source=localhost;initial catalog=ProductTest;persist security info=True;Integrated Security=SSPI;";
		public ProductServiceTest()
		{
			_mapper = new Mock<IMapper>();
			var expected = GetProductdtosFakeData();
			_mapper.Setup(x => x.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
							.Returns(expected);
			var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
			try
			{
				var options = new DbContextOptionsBuilder<ShopingDatabaseContext>()
					.UseSqlServer(connection)
					.Options;

				using (var context = new ShopingDatabaseContext(options))
				{
					context.Database.EnsureCreated();
				}
				using (var context = new ShopingDatabaseContext(options))
				{
					if (!context.Products.Any())
					{
						var fakeproduct = GetProductsFakeData();
						context.Products.AddRange(fakeproduct);
						context.SaveChanges();
					}
				}
			}
			finally
			{
				//connection.Close();
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		private IEnumerable<Product> GetProductsFakeData()
		{
			var faker = new Faker<Product>()
				.RuleFor(u => u.ProductId, f => f.IndexVariable)
				.RuleFor(u => u.Category, f => f.Name.FirstName())
				.RuleFor(u => u.Name, f => f.Name.LastName())
				.RuleFor(u => u.UnitsInStock, f => f.IndexFaker)
				.RuleFor(u => u.UnitPrice, f => f.IndexFaker);
			return faker.Generate(10).ToList();
		}

		private IEnumerable<ProductDto> GetProductdtosFakeData()
		{
			var faker = new Faker<ProductDto>()
				.RuleFor(u => u.ProductId, f => f.IndexFaker)
				.RuleFor(u => u.Category, f => f.Name.FirstName())
				.RuleFor(u => u.Name, f => f.Name.LastName())
				.RuleFor(u => u.UnitsInStock, f => f.IndexFaker)
				.RuleFor(u => u.UnitPrice, f => f.IndexFaker);
			return faker.Generate(10).ToList();
		}

		[Xunit.Fact]
		public async Task Get_ShouldReturnProducts_WhenExcludeoutofStock()
		{
			var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
			//connection.Open();
			try
			{
				var options = new DbContextOptionsBuilder<ShopingDatabaseContext>()
						.UseSqlServer(connection)
						.Options;
				using (var context = new ShopingDatabaseContext(options))
				{
					
					var service = new ProductService(_mapper.Object, context);
					// act
					var results = await service.GetProductsAsync();

					var count = results.Count();

					count.Should().Be(10);
				}
			}
			finally
			{
				//connection.Close();
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}
	}
}
