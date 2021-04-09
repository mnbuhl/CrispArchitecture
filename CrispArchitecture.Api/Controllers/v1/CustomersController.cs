using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Customers;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Application.Specifications;
using CrispArchitecture.Application.Specifications.Customers;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(IGenericRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customerRepository.GetAsync(new CustomersWithOrdersSpecification(id));

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerResponseDto>(customer));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseSpecificationParams parameters)
        {
            int totalItems = await _customerRepository.CountAsync();
            IList<Customer> customers = await _customerRepository.GetAllAsync(new CustomersSpecification(parameters));
            
            var data = _mapper.Map<List<CustomerResponseDto>>(customers);
            
            var paginationResult =
                new Pagination<CustomerResponseDto>(parameters.PageIndex, parameters.PageSize, totalItems, data);
            
            return Ok(paginationResult);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerRequest);

            bool created = await _customerRepository.CreateAsync(customer);

            if (!created)
                return BadRequest();

            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, customer.Id.ToString());
            var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

            return Created(locationUri, customerResponse);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerToUpdate = await _customerRepository.GetAsync(new CustomersWithOrdersSpecification(id));

            if (customerToUpdate == null)
                return NotFound();

            _mapper.Map(customerRequest, customerToUpdate);

            bool updated = await _customerRepository.UpdateAsync(customerToUpdate);

            if (!updated)
                return BadRequest();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _customerRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}