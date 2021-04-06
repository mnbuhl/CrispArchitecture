using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.TestOwner;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestOwnersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestOwnersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var testOwner = await _unitOfWork.TestOwnerRepository.GetAsync(id);

            if (testOwner == null)
                return NotFound();

            return Ok(_mapper.Map<TestOwnerResponseDto>(testOwner));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<TestOwner> testOwners = await _unitOfWork.TestOwnerRepository.GetAllAsync();
            return Ok(_mapper.Map<List<TestOwnerResponseDto>>(testOwners));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestOwnerDto testOwnerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testOwner = _mapper.Map<TestOwner>(testOwnerRequest);

            await _unitOfWork.TestOwnerRepository.PostAsync(testOwner);
            await _unitOfWork.TestRepository.PostAsync(testOwner.Test);
            await _unitOfWork.SaveAsync();
            
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            string locationUri = baseUrl + Request.Path.Value + "/" + testOwner.Id;

            var testOwnerResponse = _mapper.Map<TestOwnerResponseDto>(testOwner);

            return Created(locationUri, testOwnerResponse);
        }
        
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestOwnerDto updateTestOwnerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testOwnerToUpdate = await _unitOfWork.TestOwnerRepository.GetAsync(id);

            if (testOwnerToUpdate == null)
                return NotFound();

            _mapper.Map(updateTestOwnerRequest, testOwnerToUpdate);
            _unitOfWork.TestOwnerRepository.Update(testOwnerToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.TestOwnerRepository.DeleteAsync(id);
            bool deleted = await _unitOfWork.SaveAsync() > 0;

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}