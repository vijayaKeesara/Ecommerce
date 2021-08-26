using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	public class ShoppingCartController : BaseController
	{
		private readonly IShoppingCartService _shoppingCartService;		
		private readonly ILoggerManager logger;
		public ShoppingCartController(IShoppingCartService shoppingCartService, ILoggerManager logger )
		{
			_shoppingCartService = shoppingCartService;
			this.logger = logger;
		}

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<ShoppingCartItemDto>>> GetAsync(int cartId,int customerId)
		{			
			if(cartId  <= 0 || customerId <=0)
				return BadRequest("The input couldnot be validated");
			try
			{
				var result = await _shoppingCartService.GetShoppingCartItemsAsync(customerId, cartId);
				if (result == null)
				{
					return NotFound("Not valid order");
				}
				return Ok(result.ToList());
			}
			catch (Exception ex)
			{
				logger.LogInfo("Shopping Cart API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}


		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<string>>> CheckoutOrderAsync(ShoppingCartItemDto shoppingCartItemDto)
		{
			
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var result = await _shoppingCartService.AddToCartAsync(shoppingCartItemDto);
				
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Shopping Cart API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}
	}
}
