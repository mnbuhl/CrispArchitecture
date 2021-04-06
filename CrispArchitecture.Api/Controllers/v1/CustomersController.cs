using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
    }
}