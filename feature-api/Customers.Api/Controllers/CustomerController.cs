using Microsoft.AspNetCore.Mvc;
using Customers.Domain.Dtos;
using Customers.Domain.Interfaces;

namespace Customers.Api.Controllers
{
    [Route("Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // [Authorize]
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            try
            {
                GetCustomerDto getCustomerDto = new GetCustomerDto { Id = id };

                return Ok(await _customerService.ListById<GetCustomerDto, FoundCustomerDto>(getCustomerDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _customerService.ListAll<FoundCustomerDto>());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("paginated/{page}/{pageSize}")]
        public async Task<IActionResult> GetPaginated(
            [FromRoute] int page = 1, 
            [FromRoute] int pageSize = 20)
        {
            try
            {
                GetCustomerDto getCustomerDto = new GetCustomerDto { Page = page, PageSize = pageSize };

                return Ok(await _customerService.ListPaged<GetCustomerDto, FoundCustomerDto>(getCustomerDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("paginated-by-name/{name}/{page}/{pageSize}")]
        public async Task<IActionResult> GetPaginatedByName(
            [FromRoute] string name,
            [FromRoute] int page = 1, 
            [FromRoute] int pageSize = 20)
        {
            try
            {
                GetCustomerDto getCustomerDto = new GetCustomerDto { Name = name, Page = page, PageSize = pageSize };

                return Ok(await _customerService.GetPagedByName(getCustomerDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [Authorize]
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> Remove([FromRoute] DeleteCustomerDto deleteCustomerDto)
        {
            try
            {
                return Ok(await _customerService.Delete<DeleteCustomerDto, DeletedCustomerDto>(deleteCustomerDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto customerPostDto)
        {
            try
            {
                return Ok(await _customerService.Create<CreateCustomerDto, CreatedCustomerDto>(customerPostDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [Authorize]
        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                return Ok(await _customerService.Update<UpdateCustomerDto, UpdatedCustomerDto>(updateCustomerDto));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
