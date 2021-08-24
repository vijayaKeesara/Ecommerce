using AutoMapper;
using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public class OrderService : EntityService<Order>, IOrderService
	{
		private readonly ShopingDatabaseContext _context;
		private readonly IMapper _mapper;
		private readonly IProductService _productService;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly ILogger<OrderService> _logger;

		public OrderService(IMapper mapper, ILogger<OrderService> logger, ShopingDatabaseContext context,IProductService productService,
			IShoppingCartService shoppingCartService) : base(context)
		{
			_context = context;
			_productService = productService;
			_shoppingCartService = shoppingCartService;
			_mapper = mapper;
			_logger = logger;
		}

		//public virtual async Task InsertAsync(Order order, bool savechanges = false)
		//{
		//	await _context.Orders.AddAsync(order);
		//	if (savechanges)
		//		return	await _context.SaveChangesAsync();
		//}

		//public virtual async Task UpdateAsync(Order order, bool savechanges = false)
		//{
		//	_context.Orders.Update(order);
		//	if (savechanges)
		//		return await _context.SaveChangesAsync();
		//}

		public virtual async Task<Order> GetOrderByIdAsync(int orderId)
		{
			return await _context.Orders.Include(c=>c.OrderProducts).FirstOrDefaultAsync(item => item.OrderId == orderId) ;
		}

		public virtual async Task<Order> GetOrderByCustomOrderNumberAsync(string orderNo)
		{
			return await _context.Orders.Include(c=>c.OrderProducts).FirstOrDefaultAsync(item => item.OrderNo == orderNo);
		}

		public virtual async Task<List<string>> ChekOutOrderAsync(int customerId,int storeId)
		{
			var cartItems = await _shoppingCartService.GetShoppingCartItemsAsync(customerId, storeId);
			var warnings = new List<string>();
			if(cartItems == null)
			{
				warnings.Add("quantity should be greater than zero/remove the product");
				return warnings;
			}
			var response = await _productService.CheckStockAvailabilityAsync(cartItems.ToList());
			if (response != null && response.Count() != cartItems.Count())
			{
				warnings.Add("Some products are not available");
				return warnings;
			}

			try
			{
				var shoppingitem = cartItems.First();
				Order order = new Order();
				order.CustomerId = shoppingitem.CustomerId;
				order.OrderStatus = OrderStatus.OrderCreated;
				order.OrderDate = System.DateTime.Now;
				await base.AddAsync(order,false);
				order.OrderNo = DateTime.Now.ToString("ddmmmyyyyHHmmss") ;
				foreach (var item in cartItems)
				{
					OrderProducts order_Products = _mapper.Map<OrderProducts>(item);
					order_Products.Order = order;
					order_Products.OrderId = order.OrderId;
					order_Products.TotalPrice = item.SubTotal;
					order_Products.Status = OrderStatus.OrderCreated;
					await _context.OrderProducts.AddAsync(order_Products);
					await _productService.UpdateProductQuantityAsync(item.ProductId, item.Quantity, "Remove");
				}
				await _shoppingCartService.DeleteShoppingCartAsync(storeId, false);
				await _context.SaveChangesAsync(true);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex.Message);
				warnings.Add("some error occured please try again");
				return warnings;
			}
			warnings.Add("Order generated successfully");
			return warnings;
		}				
	}
}
