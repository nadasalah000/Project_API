using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;
using Talabt.Core.Specifications;
using Talabt.Repository.Data;

namespace Talabt.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MyDbContext _dbContext;

        public GenericRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }

        

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task Add(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }


        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }
    }
}
