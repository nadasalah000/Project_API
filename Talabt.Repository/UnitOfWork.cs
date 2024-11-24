using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;
using Talabt.Repository.Data;

namespace Talabt.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(MyDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if(! _repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, Repository);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
                
        }
    }
}
