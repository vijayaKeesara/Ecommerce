using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BL.Service.Interface
{
    public interface IEntityService<TEntity> where TEntity : class, new()
    {
        Task<TEntity> AddAsync(TEntity entity, bool saveChanges = false);

        Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = false);
    }
}
