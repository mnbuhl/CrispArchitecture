using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Products;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductResponseDto>(product));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
            return Ok(_mapper.Map<List<ProductResponseDto>>(products));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productRequest);

            await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.SaveAsync();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, product.Id.ToString());
            var productResponse = _mapper.Map<ProductResponseDto>(product);

            return Created(locationUri, productResponse);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productToUpdate = await _unitOfWork.ProductRepository.GetAsync(id);

            if (productToUpdate == null)
                return NotFound();

            _mapper.Map(productRequest, productToUpdate);
            _unitOfWork.ProductRepository.Update(productToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.ProductRepository.DeleteAsync(id);
            bool deleted = await _unitOfWork.SaveAsync() > 0;

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}