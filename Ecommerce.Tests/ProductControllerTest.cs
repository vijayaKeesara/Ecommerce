using AutoMapper;
using Ecommerce.Controllers;
using ECommerce.BL.Dto;
using ECommerce.BL.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Ecommerce.Tests
{	
	public class ProductControllerTest
	{
		ProductController _controller;
		Mock<IProductService> _service;
		Mock<IMapper> _mapper;
		XunitLogger xunitLogger;
		private List<ProductDto> _productDto;
		public ProductControllerTest(ITestOutputHelper output)
		{
			_service = new Mock<IProductService>();
			_mapper = new Mock<IMapper>();
			xunitLogger = new XunitLogger(output);
			_controller = new ProductController(_mapper.Object,_service.Object, xunitLogger);			
			_productDto = new List<ProductDto>()
			{
				new ProductDto { ProductId = 1,Name = "bottle",Category ="house", UnitPrice = 2,UnitsInStock = 2 },
				new ProductDto { ProductId = 2,Name = "container",Category ="house", UnitPrice = 12,UnitsInStock = 1 },
				new ProductDto { ProductId = 2,Name = "dress",Category ="apparel", UnitPrice = 12,UnitsInStock = 0 },
			};
		}

		[Xunit.Fact]
		public async void Get_ShouldReturnProducts_WhenExcludeoutofStock()
		{
			var temp = _productDto.Where(i => i.UnitsInStock > 0);
			_service.Setup(x => x.GetProductsAsync(false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(temp));
			// Act
			var result = await _controller.GetProductAsync(false);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ProductDto>>();
			((result.Result as OkObjectResult)?.Value as List<ProductDto>).Count.Should().Be(2);
		}

		[Xunit.Fact]
		public async void Get_ShouldReturnProducts_WhenInludeOtofStock()
		{
			_service.Setup(x => x.GetProductsAsync(true)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(_productDto));
			// Act
			var result = await _controller.GetProductAsync(true);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ProductDto>>();			
			((result.Result as OkObjectResult)?.Value as List<ProductDto>).Count.Should().Be(3);
		}

		[Xunit.Fact]
		public async void Get_ShouldReturnNull()
		{
			_service.Setup(x => x.GetProductsAsync(false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(null));
			// Act
			var result = await _controller.GetProductAsync(false);

			// Assert
			Xunit.Assert.IsType<BadRequestObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
		}

		[Xunit.Fact]
		public async void GetProductById_ShouldReturnProducts()
		{
			var temp =_productDto.FirstOrDefault();
			_service.Setup(x => x.GetProductByIdAsync(1)).Returns(System.Threading.Tasks.Task.FromResult<ProductDto>(temp));
			// Act
			var result = await _controller.GetProductAsync(1);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<ProductDto>();
			((ECommerce.BL.Dto.ProductDto)((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).Value).ProductId.Should().Be(1);
		}

		[Xunit.Fact]
		public async void SearchProductByName_ShouldReturnProducts()
		{
			var temp = _productDto.Where(i=>i.Name == "bottle");
			_service.Setup(x => x.SearchProductsAsync(It.IsAny<string>(), null,false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(temp));
			// Act
			var result = await _controller.SearchProductAsync(It.IsAny<string>(), null, false);
			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ProductDto>>();
			((result.Result as OkObjectResult)?.Value as List<ProductDto>).Count.Should().Be(1);
		}

		[Xunit.Fact]
		public async void SearchProductByCategory_ShouldReturnProducts()
		{
			var temp = _productDto.Where(i => i.Category == "house");
			_service.Setup(x => x.SearchProductsAsync("", It.IsAny<string>(), false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(temp));
			// Act
			var result = await _controller.SearchProductAsync("", It.IsAny<string>(), false);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ProductDto>>();
			((result.Result as OkObjectResult)?.Value as List<ProductDto>).Count.Should().Be(2);
		}

		[Xunit.Fact]
		public async void SearchProductByNameAndCategory_ShouldReturnProducts()
		{
			var temp = _productDto.Where(i => i.Category == "house" && i.Name == "bottle");
			_service.Setup(x => x.SearchProductsAsync(It.IsAny<string>(), It.IsAny<string>(), false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(temp));
			// Act
			var result = await _controller.SearchProductAsync(It.IsAny<string>(), It.IsAny<string>(), false);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ProductDto>>();
			((result.Result as OkObjectResult)?.Value as List<ProductDto>).Count.Should().Be(1);
		}

		[Xunit.Fact]
		public async void SearchProductByName_ShouldNotReturnProducts()
		{
			_service.Setup(x => x.SearchProductsAsync(It.IsAny<string>(), null, false)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ProductDto>>(null));
			// Act
			var result = await _controller.SearchProductAsync(It.IsAny<string>(), null, false);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);
			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);			
		}

		[Xunit.Fact]
		public async void Add_ProductQuantity_ShouldReturnSuccess()
		{
			_service.Setup(x => x.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Add")).Returns(System.Threading.Tasks.Task.FromResult<bool>(true));
			// Act
			var result = await _controller.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Add");

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().Be(true);
		}

		[Xunit.Fact]
		public async void Remove_ProductQuantity_ShouldReturnSuccess()
		{
			_service.Setup(x => x.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Remove")).Returns(System.Threading.Tasks.Task.FromResult<bool>(true));
			// Act
			var result = await _controller.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Remove");

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().Be(true);
		}
		[Xunit.Fact]
		public async void Add_ProductQuantity_ShouldReturnFailure()
		{
			_service.Setup(x => x.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Add")).Returns(System.Threading.Tasks.Task.FromResult<bool>(false));
			// Act
			var result = await _controller.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Add");

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().Be(false);
		}
		[Xunit.Fact]
		public async void Remove_ProductQuantity_ShouldReturnFailure()
		{
			_service.Setup(x => x.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Remove")).Returns(System.Threading.Tasks.Task.FromResult<bool>(false));
			// Act
			var result = await _controller.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Remove");

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().Be(false);
		}

		[Xunit.Fact]
		public async void CreateProduct_ShouldReturnSuccess()
		{
			_service.Setup(x => x.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), "Add")).Returns(System.Threading.Tasks.Task.FromResult<bool>(true));
			// Act
			ProductDto productDto = new ProductDto
			{
				ProductId = 4,
				UnitPrice = 12,
				UnitsInStock = 2,
				Name = "p4",
				Category = "general"
			};
			var result = await _controller.CreateProductAsync(productDto);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
		}
	}
}
