using AutoMapper;
using Customers.Domain.Interfaces;
using Customers.Domain.Entities;
using Customers.Domain.Dtos;

namespace Customers.Application.Services
{
    public class GenericService<TEntity, TRepository> : IGenericService
        where TEntity : EntityBase
        where TRepository : IGenericRepository<TEntity>
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;

        public GenericService(TRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TOutputDto> Create<TInputDto, TOutputDto>(TInputDto inputDto) 
        where TInputDto : class 
        where TOutputDto : class
        {
            var entity = _mapper.Map<TEntity>(inputDto);

            var createdEntity = await _repository.Create(entity);

            var outputDto = _mapper.Map<TOutputDto>(createdEntity);

            return outputDto;
        }

        public async Task<TOutputDto> Delete<TInputDto, TOutputDto>(TInputDto inputDto)
        where TInputDto : class 
        where TOutputDto : class
        {
            var entity = _mapper.Map<TEntity>(inputDto);

            var deletedEntity = await _repository.Delete(entity);

            var outputDto = _mapper.Map<TOutputDto>(deletedEntity);

            return outputDto;
        }

        public virtual async Task<IEnumerable<TOutputDto>> ListAll<TOutputDto>()
        where TOutputDto : class
        {
            var entitiesList = await _repository.ListAll();

            var outputDto = _mapper.Map<IEnumerable<TOutputDto>>(entitiesList);

            return outputDto;
        }

        public async Task<TOutputDto> ListById<TInputDto, TOutputDto>(TInputDto inputDto)
        where TInputDto : class 
        where TOutputDto : class
        {
            var entity = _mapper.Map<TEntity>(inputDto);

            var foundEntity = await _repository.ListById(entity);

            var outputDto = _mapper.Map<TOutputDto>(foundEntity);

            return outputDto;
        }

        public async Task<PagedEnvelopDto<TOutputDto>> ListPaged<TInputDto, TOutputDto>(TInputDto inputDto)
        where TInputDto : GetPagedBaseDto
        where TOutputDto : class
        {
            var qtde = await _repository.Count();

            var pageSizeProperty = inputDto.GetType().GetProperty("PageSize");
            var pageProperty = inputDto.GetType().GetProperty("Page");

            if (pageSizeProperty == null 
            || pageProperty == null
            || pageSizeProperty.GetValue(inputDto) == null 
            || pageProperty.GetValue(inputDto) == null)
            {
                throw new ArgumentException("O DTO de entrada não contém as propriedades pageSize ou page");
            }

            var pageSize = (int)pageSizeProperty.GetValue(inputDto)!;
            var page = (int)pageProperty.GetValue(inputDto)!;

            var entitiesList = await _repository.ListPaged(page, pageSize);

            var outputDto = _mapper.Map<List<TOutputDto>>(entitiesList);

            int pages = (int)Math.Ceiling((float)qtde / pageSize);

            return new PagedEnvelopDto<TOutputDto>
            {
                Items = outputDto,
                Pages = pages,
                TotalCount = await _repository.Count()
            };
        }

        public async Task<TOutputDto> Update<TInputDto, TOutputDto>(TInputDto inputDto)
        where TInputDto : class 
        where TOutputDto : class
        {
            var entity = _mapper.Map<TEntity>(inputDto);

            var updatedEntity = await _repository.Update(entity);

            var outputDto = _mapper.Map<TOutputDto>(updatedEntity);

            return outputDto;
        }
    }
}
