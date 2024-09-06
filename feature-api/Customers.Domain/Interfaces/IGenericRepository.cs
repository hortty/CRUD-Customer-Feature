using Customers.Domain.Entities;

namespace Customers.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        public Task<TEntity> Create(TEntity entity);

        public Task<TEntity> Update(TEntity entity);

        public Task<TEntity> Delete(TEntity entity);

        public Task<int> Count();

        public Task<int> Count(IQueryable<TEntity> query);

        public Task<IEnumerable<TEntity>> ListAll();

        public Task<TEntity> ListById(TEntity entity);

        public Task<IEnumerable<TEntity>> ListPaged(int page = 1, int pageSize = 20);

        public Task<IEnumerable<TEntity>> ListPaged(IQueryable<TEntity> query, int page = 1, int pageSize = 20);
    }
}
