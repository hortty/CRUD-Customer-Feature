using Customers.Domain.Interfaces;
using Customers.Domain.Entities;
using Customers.Infrastructure.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Customers.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(DataContext dbContext) : base(dbContext)
    {
        
    }

    public IQueryable<Customer> GetPagedByName(string name)
    {
        return _dbSet.Where(x => x.Name.Contains(name) || x.Name.Contains(name.ToLower()));
    }

    public Task<Customer?> ExistsByCpfCnpj(string cpfCnpj)
    {
        return _dbSet.Where(x => x.CpfCnpj == cpfCnpj).FirstOrDefaultAsync();
    }

    public Task<Customer?> ExistsByEmail(string email)
    {
        return _dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    public Task<Customer?> ExistsByStateRegistry(string stateRegistry)
    {
        return _dbSet.Where(x => x.StateRegistration == stateRegistry).FirstOrDefaultAsync();
    }
}
