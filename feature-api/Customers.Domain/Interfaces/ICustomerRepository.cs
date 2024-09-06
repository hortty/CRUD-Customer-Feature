using Customers.Domain.Entities;

namespace Customers.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public IQueryable<Customer> GetPagedByName(string name);
        public Task<Customer?> ExistsByCpfCnpj(string cpfCnpj);
        public Task<Customer?> ExistsByEmail(string email);
        public Task<Customer?> ExistsByStateRegistry(string stateRegistry);
    }
}
