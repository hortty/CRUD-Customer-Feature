using Customers.Domain.Dtos;

namespace Customers.Domain.Interfaces
{
    public interface ICustomerService : IGenericService
    {
        public Task<PagedEnvelopDto<FoundCustomerDto>> GetPagedByName(GetCustomerDto getCustomerDto);
    }
}