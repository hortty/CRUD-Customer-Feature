using AutoMapper;
using Customers.Domain.Interfaces;
using Customers.Domain.Entities;
using Customers.Domain.Dtos;

namespace Customers.Application.Services
{
    public class CustomerService : GenericService<Customer, ICustomerRepository>, ICustomerService
    {
        private ICustomerRepository _repository;
        private IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedEnvelopDto<FoundCustomerDto>> GetPagedByName(GetCustomerDto getCustomerDto)
        {
            IQueryable<Customer> query = _repository.GetPagedByName(getCustomerDto.Name);

            var qtde = await _repository.Count(query);

            var page = getCustomerDto.Page;
            var pageSize = getCustomerDto.PageSize;

            var entitiesList = await _repository.ListPaged(query, page, pageSize);

            var outputDto = _mapper.Map<List<FoundCustomerDto>>(entitiesList);

            int pages = (int)Math.Ceiling((float)qtde / pageSize);

            return new PagedEnvelopDto<FoundCustomerDto>
            {
                Items = outputDto,
                Pages = pages,
                TotalCount = await _repository.Count()
            };
        }

        public override async Task<TOutputDto> Create<TInputDto, TOutputDto>(TInputDto inputDto)
        {
            // List<Task<Customer?>> tasks = new List<Task<Customer?>>();

            var cpfCnpj = inputDto.GetType().GetProperty("CpfCnpj")?.GetValue(inputDto);
            var stateRegistration = inputDto.GetType().GetProperty("StateRegistration")?.GetValue(inputDto);
            var email = inputDto.GetType().GetProperty("Email")?.GetValue(inputDto);

            if(cpfCnpj is null || email is null)
                throw new Exception($"{nameof(cpfCnpj)} ou {nameof(email)} não podem ser nulos!");

            bool stateRegistryExists = false;
            bool emailExists = await _repository.ExistsByEmail((string)email) != null;
            bool cpfCnpjExists = await _repository.ExistsByCpfCnpj((string)cpfCnpj) != null;

            if(stateRegistration is not null && !string.IsNullOrWhiteSpace((string)stateRegistration))
            {
                stateRegistryExists = 
                    await _repository.ExistsByStateRegistry((string)stateRegistration) != null;
            }

            if(emailExists)
            {
                throw new Exception($"Já existem clientes com o seguinte email: {(string)email}");
            }

            if(cpfCnpjExists)
            {
                throw new Exception($"Já existem clientes com o seguinte CPF ou CNPJ: {(string)cpfCnpj}");
            }

            if(stateRegistryExists)
            {
                throw new Exception($"Já existem clientes com o seguinte registro: {stateRegistration}");
            }

            return await base.Create<TInputDto, TOutputDto>(inputDto);
        }

    }
}