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
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductResponseDto>(product));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Product> products = await _productService.GetAllProductsAsync();
            return Ok(_mapper.Map<List<ProductResponseDto>>(products));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(productRequest);

            bool created = await _productService.CreateProductAsync(product);

            if (!created)
                return BadRequest();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, product.Id.ToString());
            var productResponse = _mapper.Map<ProductResponseDto>(product);

            return Created(locationUri, productResponse);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductCommandDto productRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productToUpdate = await _productService.GetProductAsync(id);

            if (productToUpdate == null)
                return NotFound();

            _mapper.Map(productRequest, productToUpdate);

            bool updated = await _productService.UpdateProductAsync(productToUpdate);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}