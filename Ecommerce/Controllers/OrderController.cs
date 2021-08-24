using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Service;
using LoggerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ILoggerManager logger;
		public OrderController(IOrderService orderService, ILoggerManager logger)
		{
			_orderService = orderService;
			this.logger = logger;
		}

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Order>> GetAsync([FromQuery(Name = "orderId")] int orderId)
		{
			if (orderId <0)
				return BadRequest("The input couldnot be validated");
			try
			{
				var result = await _orderService.GetOrderByIdAsync(orderId);
				if (result == null)
				{
					return BadRequest("Not valid order");
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Order API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}


		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<string>>> CheckoutOrderAsync(int customerId,int storeId)
		{
			if (storeId <= 0 || customerId <= 0)
				return BadRequest("The input couldnot be validated");
			try
			{
				var result = await _orderService.ChekOutOrderAsync(customerId,storeId);
				if (result == null)
				{
					return BadRequest("Not valid order");
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Order API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}
	}
}
