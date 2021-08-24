using AutoMapper;
using Ecommerce.Controllers;
using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace Ecommerce.Tests
{
	public class ShoppingCartControllerTest
	{
		ShoppingCartController _controller;
		Mock<IShoppingCartService> shoppingCartService;
		Mock<IMapper> _mapper;
		XunitLogger xunitLogger;
		private List<ShoppingCartItemDto> ShoppingCartItemDtos;
		public ShoppingCartControllerTest(ITestOutputHelper output)
		{
			shoppingCartService = new Mock<IShoppingCartService>();
			_mapper = new Mock<IMapper>();
			xunitLogger = new XunitLogger(output);
			_controller = new ShoppingCartController(shoppingCartService.Object, xunitLogger);
			ShoppingCartItemDtos = new List<ShoppingCartItemDto>
			{
				new ShoppingCartItemDto
				{
					CartId = 1,
					CustomerId = 1,
					Quantity = 2,
					ProductId = 1
				}
			};
		}

		[Xunit.Fact]
		public async void Get_ShouldReturnProducts_WhenExcludeoutofStock()
		{
			shoppingCartService.Setup(x => x.GetShoppingCartItemsAsync(It.IsAny<int>(), It.IsAny<int>(),null)).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<ShoppingCartItemDto>>(ShoppingCartItemDtos));
			// Act
			var result = await _controller.GetAsync(It.IsAny<int>(), It.IsAny<int>());

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<ShoppingCartItemDto>>();
			((result.Result as OkObjectResult)?.Value as List<ShoppingCartItemDto>).Count.Should().Be(1);
		}


		[Xunit.Fact]
		public async void CheckOut_ReturnsSucess()
		{
			var warnings = new List<string>();
			shoppingCartService.Setup(x => x.AddToCartAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(System.Threading.Tasks.Task.FromResult<IEnumerable<string>>(warnings));
			// Act
			var result = await _controller.CheckoutOrderAsync(1,1,1,1);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<string>>();
		}
	}
}
