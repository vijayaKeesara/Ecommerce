using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public interface IProductService : IEntityService<Product>
	{
		Task<IEnumerable<ProductDto>> GetProductsAsync(bool includeOutOfStock = false);
		Task<ProductDto> GetProductByIdAsync(int productId);
		Task<IEnumerable<ProductDto>> SearchProductsAsync(string name, string category, bool includeOutOfStock = false);
		Task<List<ShoppingCartItemDto>> CheckStockAvailabilityAsync(List<ShoppingCartItemDto> cartItems);
		Task<bool> UpdateProductQuantityAsync(int productId, int qunatity,string typeOfAction,bool issavechages=false);
	}
}
