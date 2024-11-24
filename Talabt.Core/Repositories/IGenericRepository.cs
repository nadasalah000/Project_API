using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Specifications;

namespace Talabt.Core.Repositories
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        #region WithOut Specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion
        #region With Specification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
        #endregion
        Task Add(T item);

        void Delete(T item);
         void Update(T item);
    }
}
