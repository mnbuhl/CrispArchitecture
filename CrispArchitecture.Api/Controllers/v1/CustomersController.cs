using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Customers;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customerService.GetCustomerAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerResponseDto>(customer));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Customer> customers = await _customerService.GetAllCustomerAsync();
            return Ok(_mapper.Map<List<CustomerResponseDto>>(customers));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerRequest);

            bool created = await _customerService.CreateCustomerAsync(customer);
            
            if (!created)
                return BadRequest();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, customer.Id.ToString());
            var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

            return Created(locationUri, customerResponse);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerToUpdate = await _customerService.GetCustomerAsync(id);

            if (customerToUpdate == null)
                return NotFound();

            _mapper.Map(customerRequest, customerToUpdate);
            
            bool updated = await _customerService.UpdateCustomerAsync(customerToUpdate);

            if (!updated)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _customerService.DeleteCustomerAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}