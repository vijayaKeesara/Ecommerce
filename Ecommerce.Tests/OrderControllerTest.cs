using AutoMapper;
using Ecommerce.Controllers;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Service;
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
	public class OrderControllerTest
	{
		OrderController _controller;
		Mock<IOrderService> _service;
		Mock<IMapper> _mapper;
		XunitLogger xunitLogger;

		public OrderControllerTest(ITestOutputHelper output)
		{
			_service = new Mock<IOrderService>();
			_mapper = new Mock<IMapper>();
			xunitLogger = new XunitLogger(output);
			_controller = new OrderController( _service.Object, xunitLogger);			
		}

		[Xunit.Fact]
		public async void Get_ShouldReturnProducts_WhenExcludeoutofStock()
		{
			_service.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).Returns(System.Threading.Tasks.Task.FromResult<Order>(new Order { OrderId =1}));
			// Act
			var result = await _controller.GetAsync(It.IsAny<int>());

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<Order>();
			((result.Result as OkObjectResult)?.Value as Order).OrderId.Should().Be(1);
		}


		[Xunit.Fact]
		public async void CheckOut_ReturnsSucess()
		{
			var warnings = new List<string>();
			_service.Setup(x => x.ChekOutOrderAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(System.Threading.Tasks.Task.FromResult<List<string>>(warnings));
			// Act
			var result = await _controller.CheckoutOrderAsync(1,1);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);

			(result.Result as StatusCodeResult)?.StatusCode.Should().Be((int)HttpStatusCode.OK);
			(result.Result as OkObjectResult)?.Value.Should().BeOfType<List<string>>();
		}

		[Xunit.Fact]
		public async void CheckOut_WhenCartIdZero_ReturnsBadRequest()
		{
			var warnings = new List<string>();
			_service.Setup(x => x.ChekOutOrderAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(System.Threading.Tasks.Task.FromResult<List<string>>(warnings));
			// Act
			var result = await _controller.CheckoutOrderAsync(1, 0);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);

			(result.Result as BadRequestObjectResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
		}

		[Xunit.Fact]
		public async void CheckOut_WhenCustomerIdZero_ReturnsBadRequest()
		{
			var warnings = new List<string>();
			_service.Setup(x => x.ChekOutOrderAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(System.Threading.Tasks.Task.FromResult<List<string>>(warnings));
			// Act
			var result = await _controller.CheckoutOrderAsync(0, 1);

			// Assert
			Xunit.Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result.Result);

			(result.Result as BadRequestObjectResult)?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
		}
	}
}
