using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.ProductGroups;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IGenericRepository<ProductGroup> _productGroupRepository;
        private readonly IMapper _mapper;

        public ProductGroupsController(IGenericRepository<ProductGroup> productGroupRepository, IMapper mapper)
        {
            _productGroupRepository = productGroupRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductGroupCommandDto productGroupRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productGroup = _mapper.Map<ProductGroup>(productGroupRequest);

            bool created = await _productGroupRepository.CreateAsync(productGroup);

            if (!created)
                return BadRequest();

            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, productGroup.Id.ToString());
            var productGroupResponse = _mapper.Map<ProductGroupResponseDto>(productGroup);

            return Created(locationUri, productGroupResponse);
        }
    }
}