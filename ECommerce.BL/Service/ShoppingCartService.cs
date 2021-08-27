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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECommerce.BL.Service
{
    public class ShoppingCartService :  IShoppingCartService
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IDbContext _context;
        private readonly ILogger<ShoppingCartService> _logger;
        private readonly IMapper _mapper;

        public ShoppingCartService(ILogger<ShoppingCartService> logger, IMapper mapper,ICustomerService customerService, IProductService productService,
            IDbContext context) 
        {
            this._customerService = customerService;
            _productService = productService;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public virtual async Task<ShoppingCart> AddAsync(ShoppingCart shoppingCart, bool savechanges = false)
        {
            await _context.ShoppingCart.AddAsync(shoppingCart);
            if (savechanges)
                await _context.SaveChangesAsync();
            return shoppingCart;
        }

        public virtual async Task<ShoppingCart> UpdateAsync(ShoppingCart shoppingCart, bool savechanges = false)
        {
            _context.ShoppingCart.Update(shoppingCart);
            if (savechanges)
                await _context.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task InsertAsync(ShoppingCartItem item, bool savechanges = false)
		{
			await _context.ShoppingCartItem.AddAsync(item);
            if (savechanges)
                await _context.SaveChangesAsync(savechanges);
		}

		public async Task UpdateAsync(ShoppingCartItem item, bool savechanges = false)
		{
			_context.ShoppingCartItem.Update(item);
            if (savechanges)
                await _context.SaveChangesAsync(savechanges);
		}

		public async Task<ShoppingCart> GetShoppingCartByIdAsync(int customerId, int cartId)
        {
            return _context.ShoppingCart.FirstOrDefault(item => item.CartId == cartId && item.CustomerId == customerId);
        }  
        public async Task<IEnumerable<string>> AddToCartAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            var (warnings, newquantity, shoppingcart, cartItem) = await ValidateItem(shoppingCartItemDto); ;
            if (warnings != null && warnings.Count() > 0)
                return warnings;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await CreateOrUpdateShoppingCart(shoppingCartItemDto, newquantity, shoppingcart, cartItem);
                    var temp = await _context.SaveChangesAsync(true);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);              
                return new List<string> { "some error occured please try again" };
            }
            return new List<string> { "Added to cart successfully." };
        }

		private async Task CreateOrUpdateShoppingCart(ShoppingCartItemDto shoppingCartItemDto, int newquantity, ShoppingCart oldshoppingCart,ShoppingCartItem cartItem)
		{
            int cartId = shoppingCartItemDto.CartId;
            var newcart = new ShoppingCart
            {
                CartId = shoppingCartItemDto.CartId,
                CustomerId = shoppingCartItemDto.CustomerId
            };
            if (oldshoppingCart != null)
            {
                cartId = oldshoppingCart.CartId;
                newcart = oldshoppingCart;
            }
            var shoppingCartItem = new ShoppingCartItem
            {
                CartId = cartId,
                ProductId = shoppingCartItemDto.ProductId,
                Quantity = newquantity,
                CustomerId = shoppingCartItemDto.CustomerId,
                ShoppingCart = newcart
            };

            if (oldshoppingCart!=null && oldshoppingCart.CartId > 0 )
            {
                if (cartItem == null)
                {
                    await InsertAsync(shoppingCartItem);
                }
                else
                {
                    cartItem.ShoppingCart = oldshoppingCart;
                    cartItem.Quantity = newquantity;
                    await UpdateAsync(cartItem);
                }
            }
            else
            {
                await _context.ShoppingCart.AddAsync(newcart);
                shoppingCartItem.CartId = newcart.CartId;
                await InsertAsync(shoppingCartItem);
            }
        }

        public async Task<(IEnumerable<string>,int, ShoppingCart, ShoppingCartItem)> ValidateItem(ShoppingCartItemDto shoppingCartItemDto)
		{
            var warnings = new List<string>();
            var customer = await _customerService.GetCustomerByIdAsync(shoppingCartItemDto.CustomerId);
            if (customer == null)
            {
                warnings.Add("Invalid customer");
                return (warnings,0,null,null);
            }

            var product = await _productService.GetProductByIdAsync(shoppingCartItemDto.ProductId);
            if (product == null)
            {
                warnings.Add("Invalid product");
                return (warnings,0,null,null);
            }

            var newquantity = shoppingCartItemDto.Quantity;
            ShoppingCartItem cartItem = null;
            ShoppingCart shoppingCart = null;

            //check if the cart exists
            if (shoppingCartItemDto.CartId > 0)
            {
                 shoppingCart = await GetShoppingCartByIdAsync(shoppingCartItemDto.CustomerId, shoppingCartItemDto.CartId);

                if (shoppingCart == null)
                {
                    warnings.Add("Invalid cart");
                    return (warnings, 0, null, null);
                }
                else 
                {
                    cartItem = _context.ShoppingCartItem.AsNoTracking().FirstOrDefault(item => item.CartId == shoppingCartItemDto.CartId 
                    && item.CustomerId == shoppingCartItemDto.CustomerId && item.ProductId == shoppingCartItemDto.ProductId);

                    if (cartItem != null)
                    {
                        newquantity += cartItem.Quantity;
                    }
                }
            }
           
            //Check product availability
            if (product.UnitsInStock ==0 || product.UnitsInStock < newquantity)
            {
                warnings.Add("Stock not available");
                return (warnings, 0, null, null);
            }
            return (warnings,newquantity, shoppingCart,cartItem);
        }

        public async Task DeleteShoppingCartAsync(int cartId, bool savechanges)
        {            
            var cartitem = await _context.ShoppingCart.FirstOrDefaultAsync(i => i.CartId == cartId);

            if (cartitem != null)
            {
                var items = _context.ShoppingCartItem.Where(i => i.CartId == cartId);
                foreach(var item in items)
				{
                    _context.ShoppingCartItem.Remove(item);
				}
                _context.ShoppingCart.Remove(cartitem);

                if (savechanges)
                    await _context.SaveChangesAsync(savechanges);
            }
        }

        public async Task DeleteShoppingCartItemAsync(ShoppingCartItemDto shoppingCartItem, bool savechanges)
        {
            if (shoppingCartItem == null || shoppingCartItem.CartId <=0 || shoppingCartItem.ProductId <=0 || shoppingCartItem.CustomerId <=0)
                throw new ArgumentNullException(nameof(shoppingCartItem));
            var cartitem = await _context.ShoppingCartItem.FirstOrDefaultAsync(i => i.CartId == shoppingCartItem.CartId
                && i.ProductId == shoppingCartItem.ProductId && i.CustomerId == shoppingCartItem.CustomerId);

            if (cartitem != null)
            {
                _context.ShoppingCartItem.Remove(cartitem);

                if (savechanges)
                    await _context.SaveChangesAsync(savechanges);
            }
        }

        public async Task<IEnumerable<ShoppingCartItemDto>> GetShoppingCartItemsAsync(int? customerId, int? storeId = null, int? productId = null)
        {
            IQueryable<ShoppingCartItem> items = _context.ShoppingCartItem.Include(c=>c.Product);

            if (customerId.HasValue)
                items = items.Where(i => i.CustomerId == customerId);

            //filter shopping cart items by store
            if (storeId.HasValue)
                items = items.Where(item => item.CartId == storeId);

            //filter shopping cart items by product
            if (productId.HasValue)
                items = items.Where(item => item.ProductId == productId);
            // var result = ;
            List<ShoppingCartItemDto> result = _mapper.Map<List<ShoppingCartItemDto>>(items.ToList());
            foreach (var item in items)
            {
                var match = result.FirstOrDefault(i => i.CartId == item.CartId && i.ProductId == item.ProductId
                                                    && i.CustomerId == item.CustomerId);
                if (match != null)
                    match.SubTotal = item.Product.UnitPrice * item.Quantity;
            }
            return result;
        }


        public async Task<(decimal,decimal)> GetSubTotalAsync(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            decimal subTotal;            

            var product = await _productService.GetProductByIdAsync(shoppingCartItem.ProductId);

            subTotal = product.UnitPrice * shoppingCartItem.Quantity;
           
            return (subTotal,product.UnitPrice);
        }       
    }
}
