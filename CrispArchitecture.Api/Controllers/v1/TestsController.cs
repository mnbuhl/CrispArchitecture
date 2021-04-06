using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.Test;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var test = await _unitOfWork.TestRepository.GetAsync(id);

            if (test == null)
                return NotFound();

            return Ok(_mapper.Map<TestResponseDto>(test));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Test> tests = await _unitOfWork.TestRepository.GetAllAsync();
            return Ok(_mapper.Map<List<TestResponseDto>>(tests));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestDto testRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var test = _mapper.Map<Test>(testRequest);

            await _unitOfWork.TestRepository.PostAsync(test);
            await _unitOfWork.SaveAsync();
            
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            string locationUri = baseUrl + Request.Path.Value + "/" + test.Id;

            var testResponse = _mapper.Map<TestResponseDto>(test);

            return Created(locationUri, testResponse);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestDto updateTestRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testToUpdate = await _unitOfWork.TestRepository.GetAsync(id);

            if (testToUpdate == null)
                return NotFound();

            _mapper.Map(updateTestRequest, testToUpdate);
            _unitOfWork.TestRepository.Update(testToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.TestRepository.DeleteAsync(id);
            bool deleted = await _unitOfWork.SaveAsync() > 0;

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}