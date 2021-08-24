using Ecommerce.DA.Domain;
using ECommerce.BL.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class, new()
    {
        protected readonly ShopingDatabaseContext _shopingDatabaseContext;

        public EntityService(ShopingDatabaseContext shopingDatabaseContext)
        {
            _shopingDatabaseContext = shopingDatabaseContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _shopingDatabaseContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool saveChanges = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _shopingDatabaseContext.AddAsync(entity);
                if (saveChanges)
                    await _shopingDatabaseContext.SaveChangesAsync(saveChanges);

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }

            try
            {
                _shopingDatabaseContext.Update(entity);
                if (saveChanges)
                    await _shopingDatabaseContext.SaveChangesAsync(saveChanges);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated {ex.Message}");
            }
        }
    }
}
