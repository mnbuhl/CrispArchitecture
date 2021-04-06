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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerResponseDto>(customer));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Customer> customers = await _unitOfWork.CustomerRepository.GetAllAsync();
            return Ok(_mapper.Map<List<CustomerResponseDto>>(customers));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerRequest);

            await _unitOfWork.CustomerRepository.CreateAsync(customer);
            await _unitOfWork.SaveAsync();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, customer.Id.ToString());
            var customerResponse = _mapper.Map<CustomerResponseDto>(customer);

            return Created(locationUri, customerResponse);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerCommandDto customerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerToUpdate = await _unitOfWork.CustomerRepository.GetAsync(id);

            if (customerToUpdate == null)
                return NotFound();

            _mapper.Map(customerRequest, customerToUpdate);
            _unitOfWork.CustomerRepository.Update(customerToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.CustomerRepository.DeleteAsync(id);
            bool deleted = await _unitOfWork.SaveAsync() > 0;

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}