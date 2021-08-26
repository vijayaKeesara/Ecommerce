using AutoMapper;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
	public class CustomerController : BaseController
	{
		private readonly ICustomerService _customerService;
		private readonly ILoggerManager logger;
		private readonly IMapper _mapper;

		public CustomerController(IMapper mapper, ICustomerService customerService, ILoggerManager logger)
		{
			_customerService = customerService;
			this.logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Customer>> GetCustomersAsync(int customerId)
		{
			try
			{
				var result = await _customerService.GetCustomerByIdAsync(customerId);
				if (result == null)
				{
					return NotFound("Not valid customer");
				}
				return Ok(result);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Customer API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CustomerDto>> CreateAsync([FromBody]CustomerDto customerDto)
		{
			try
			{
				Customer customer = _mapper.Map<Customer>(customerDto);
				var result = await _customerService.AddAsync(customer, true);

				var temp = _mapper.Map<CustomerDto>(result);

				return Ok(temp);
			}
			catch (Exception ex)
			{
				logger.LogInfo("Customer API- " + ex.Message);
				return BadRequest(ex.Message);
			}
		}
	}
}
