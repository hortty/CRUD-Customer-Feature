using Customers.Domain.Dtos;

namespace Customers.Domain.Interfaces
{
    public interface IGenericService
    {
        public Task<TOutputDto> Create<TInputDto, TOutputDto>(TInputDto inputDto) where TInputDto: class where TOutputDto : class ;

        public Task<TOutputDto> Update<TInputDto, TOutputDto>(TInputDto inputDto) where TInputDto: class where TOutputDto : class;

        public Task<TOutputDto> Delete<TInputDto, TOutputDto>(TInputDto inputDto) where TInputDto: class where TOutputDto : class;

        public Task<IEnumerable<TOutputDto>> ListAll<TOutputDto>() where TOutputDto : class;

        public Task<TOutputDto> ListById<TInputDto, TOutputDto>(TInputDto inputDto) where TInputDto: class where TOutputDto : class;
    
        public Task<PagedEnvelopDto<TOutputDto>> ListPaged<TInputDto, TOutputDto>(TInputDto inputDto) where TInputDto: GetPagedBaseDto where TOutputDto : class;
    }
}
