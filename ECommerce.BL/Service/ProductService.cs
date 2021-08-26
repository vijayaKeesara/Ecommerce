using AutoMapper;
using Ecommerce.DA.Domain;
using Ecommerce.DA.Domain.Models;
using ECommerce.BL.Dto;
using ECommerce.BL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
	public class ProductService :  IProductService
	{
		private readonly IDbContext _context;
		private readonly IMapper _mapper;

		public ProductService(IMapper mapper, IDbContext context)
		{
			_context = context;
			_mapper = mapper;
		}

		public virtual async Task<Product> AddAsync(Product product, bool savechanges = false)
		{
			await _context.Products.AddAsync(product);
			if (savechanges)
				await _context.SaveChangesAsync();
			return product;
		}

		public virtual async Task<Product> UpdateAsync(Product product, bool savechanges = false)
		{
			_context.Products.Update(product);
			if (savechanges)
				await _context.SaveChangesAsync();
			return product;
		}

		public async Task<ProductDto> GetProductByIdAsync(int productId)
		{
			var p = await _context.Products.FirstOrDefaultAsync(item => item.ProductId == productId);
			return _mapper.Map<ProductDto>(p);
		}

		public async Task<IEnumerable<ProductDto>> GetProductsAsync(bool includeOutOfStock = false)
		{
			IEnumerable<Product> products;
			if (!includeOutOfStock)
				products = await _context.Products.Where(i => i.UnitsInStock > 0).ToListAsync();
			else
				products = await _context.Products.ToListAsync();

			return _mapper.Map<List<ProductDto>>(products);
		}

		public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string name, string category, bool includeOutOfStock = false)
		{
			IQueryable<Product> products;
			if (!includeOutOfStock)
				products = _context.Products.Where(i => i.UnitsInStock > 0);
			else
				products = _context.Products;

			if (!string.IsNullOrEmpty(name))
				products = products.Where(item => item.Name.Contains(name));
			if (!string.IsNullOrEmpty(category))
				products = products.Where(item => item.Category.Contains(category));
			var temp = await products.ToListAsync();
			return _mapper.Map<List<ProductDto>>(temp);
		}

		public async Task<List<ShoppingCartItemDto>> CheckStockAvailabilityAsync(List<ShoppingCartItemDto> cartItems)
		{
			List<int> productIds = cartItems.Select(i => i.ProductId).ToList();
			var temp = _context.Products.Where(item => productIds.Contains(item.ProductId));

			var products = from prod in _context.Products.Select(i=> new ShoppingCartItemDto
							{ 
								ProductId = i.ProductId,
								Quantity = i.UnitsInStock

							}).AsEnumerable()
						   join p in cartItems on prod.ProductId equals p.ProductId
						   where prod.Quantity >= p.Quantity
						   select p;

			return  products.ToList();
		}

		public virtual async Task<bool> UpdateProductQuantityAsync(int productId, int quantity,string TypeOfAction,bool saveChanges = false)
		{
			var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == productId);
			if (product != null )
			{
				if (TypeOfAction == "Add")
					product.UnitsInStock += quantity;
				else if(TypeOfAction == "Remove" && product.UnitsInStock >= quantity)
					product.UnitsInStock -= quantity;
				_context.Products.Update(product);
				if (saveChanges)
					await _context.SaveChangesAsync(true);
				return true;
			}
			return false;
		}
	}
}
