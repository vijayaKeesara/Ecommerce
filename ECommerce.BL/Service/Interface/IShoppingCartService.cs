using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BL.Service.Interface
{
	public interface IShoppingCartService : IEntityService<ShoppingCart>
	{
		Task<ShoppingCart> GetShoppingCartByIdAsync(int customerId, int cartId);
		Task<IEnumerable<string>> AddToCartAsync(ShoppingCartItemDto shoppingCartItemDto);
		Task<(IEnumerable<string>, int, ShoppingCart, ShoppingCartItem)> ValidateItem(ShoppingCartItemDto shoppingCartItemDto);
		Task DeleteShoppingCartAsync(int cartId, bool savechanges);
		Task DeleteShoppingCartItemAsync(ShoppingCartItemDto shoppingCartItem, bool savechanges);
		Task<IEnumerable<ShoppingCartItemDto>> GetShoppingCartItemsAsync(int? customerId, int? storeId = null, int? productId = null);

		Task<(decimal, decimal)> GetSubTotalAsync(ShoppingCartItem shoppingCartItem);
	}
}
