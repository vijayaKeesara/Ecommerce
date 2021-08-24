using AutoMapper;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.MiddleWare
{
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			CreateMap<ProductDto, Product>(); 
			CreateMap<Product, ProductDto>(); 
			CreateMap<CustomerDto, Customer>();
			CreateMap<Customer, CustomerDto>();
			CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
			CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
			CreateMap<OrderProducts, ShoppingCartItemDto>();
			CreateMap<ShoppingCartItemDto, OrderProducts>();			
		}
	}
}
