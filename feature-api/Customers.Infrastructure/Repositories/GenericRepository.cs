using Microsoft.EntityFrameworkCore;
using Customers.Domain.Interfaces;
using Customers.Domain.Entities;
using Customers.Infrastructure.Context;

namespace Customers.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly DataContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var createdEntity = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return createdEntity.Entity;
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            var foundEntity = await ListById(entity);

            if (foundEntity == null)
                throw new InvalidOperationException($"Entidade {nameof(entity)} não encontrada.");

            var removedEntity = _dbSet.Remove(foundEntity);
            await _dbContext.SaveChangesAsync();

            return removedEntity.Entity;
        }

        public async Task<IEnumerable<TEntity>> ListAll()
        {
            var foundEntities = await _dbSet
                .AsNoTracking()
                .ToListAsync();

            if (foundEntities.Equals(null))
                throw new InvalidOperationException($"Não foram encontrados registros.");

            return foundEntities;
        }

        public async Task<IEnumerable<TEntity>> ListPaged(int page = 1, int pageSize = 20)
        {

            if(page <= 0)
                page = 1;

            var foundEntities = await _dbSet
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (foundEntities.Equals(null))
                throw new InvalidOperationException($"Não foram encontrados registros.");

            return foundEntities;
        }

        public async Task<IEnumerable<TEntity>> ListPaged(
            IQueryable<TEntity> query, 
            int page = 1, 
            int pageSize = 20
        )
        {

            if(page <= 0)
                page = 1;

            var foundEntities = await query
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (foundEntities.Equals(null))
                throw new InvalidOperationException($"Não foram encontrados registros.");

            return foundEntities;
        }

        public async Task<TEntity> ListById(TEntity entity)
        {
            var foundEntity = await _dbSet.FindAsync(entity.Id);

            if (foundEntity == null)
                throw new InvalidOperationException($"Entidade {nameof(entity)} não encontrada.");

            return foundEntity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var foundEntity = await _dbSet.FindAsync(entity.Id);

            if (foundEntity == null)
            {
                throw new InvalidOperationException($"Entidade {nameof(entity)} não encontrada.");
            }
            else
            {
                _dbContext.Entry(foundEntity).State = EntityState.Detached;
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<int> Count()
        {
            var qtde = await _dbSet.CountAsync();

            return qtde;
        }

        public async Task<int> Count(IQueryable<TEntity> query)
        {
            var qtde = await query.CountAsync();

            return qtde;
        }

    }
}
