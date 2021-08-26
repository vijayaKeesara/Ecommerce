using AutoMapper;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IProductService _productService;
		private readonly ILoggerManager logger;
		private readonly IMapper _mapper;
		public ProductController(IMapper mapper, IProductService productService, ILoggerManager logger)
		{
			_productService = productService;
			this.logger = logger;
			_mapper = mapper;
		}

		[HttpGet, Route("GetInStockProducts")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<ProductDto>>> GetProductAsync(bool includeOutOfStock)
		{
			try
			{
				var result = await _productService.GetProductsAsync(includeOutOfStock);
				if (result == null)
				{
					return BadRequest("No products found");
				}
				return Ok(result.ToList());
			}
			catch (Exception ex)
			{
				logger.LogInfo("Prodcuct API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet, Route("GetProduct")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductDto>> GetProductAsync(int productId)
		{
			try
			{
				var result = await _productService.GetProductByIdAsync(productId);
				if (result == null)
				{
					return BadRequest("Not valid product");
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Product API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpGet, Route("SearchProduct")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<ProductDto>>> SearchProductAsync(string name, string category, bool includeOutOfStock)
		{
			try
			{
				var result = await _productService.SearchProductsAsync(name, category, includeOutOfStock);
				if (result == null || result.Count() <=0)
				{
					return BadRequest("Please widen the search");
				}
				return Ok(result.ToList());
			}
			catch (Exception ex)
			{
				logger.LogInfo("Product API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductDto>> CreateProductAsync(ProductDto productDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				Product product = _mapper.Map<ProductDto, Product>(productDto);
				var result = await _productService.AddAsync(product, true);
				var temp = _mapper.Map<ProductDto>(result);
				return Ok(temp);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Customer API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost, Route("UpdateProductQuantity")]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<bool>> UpdateProductQuantityAsync(int productId, int quantity, string action)
		{
			try
			{
				var result = await _productService.UpdateProductQuantityAsync(productId, quantity, action,true);
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Product API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}
	}
}
