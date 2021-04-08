using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Products;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Application.Specifications.Products;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productRepository.GetAsync(new ProductsSpecification(id));

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductResponseDto>(product));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string sort)
        {
            IList<Product> products = await _productRepository.GetAllAsync(new ProductsSpecification(sort));
            return Ok(_mapper.Map<List<ProductResponseDto>>(products));
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productRequest);

            bool created = await _productRepository.CreateAsync(product);

            if (!created)
                return BadRequest();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, product.Id.ToString());
            var productResponse = _mapper.Map<ProductResponseDto>(product);

            return Created(locationUri, productResponse);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productToUpdate = await _productRepository.GetAsync(new ProductsSpecification(id));

            if (productToUpdate == null)
                return NotFound();

            _mapper.Map(productRequest, productToUpdate);

            bool updated = await _productRepository.UpdateAsync(productToUpdate);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _productRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}