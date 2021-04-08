using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Orders;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Application.Specifications.Orders;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderTotalService _orderTotalService;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IMapper mapper, IGenericRepository<Order> orderRepository, IOrderTotalService orderTotalService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderTotalService = orderTotalService;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _orderRepository.GetAsync(new OrdersWithLineItemsAndProductsSpecification(id));

            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderResponseDto>(order));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string sort)
        {
            IList<Order> orders = await _orderRepository.GetAllAsync(new OrdersSpecification(sort));
            return Ok(_mapper.Map<List<OrderResponseDto>>(orders));
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var order = _mapper.Map<Order>(orderRequest);
            order.Total = await _orderTotalService.GetOrderTotal(order.LineItems);

            bool created = await _orderRepository.CreateAsync(order);

            if (!created)
                return BadRequest();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, order.Id.ToString());
            var orderResponse = _mapper.Map<OrderResponseDto>(order);
        
            return Created(locationUri, orderResponse);
        }
        
        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var orderToUpdate = await _orderRepository.GetAsync(new OrdersWithLineItemsAndProductsSpecification(id));

            if (orderToUpdate == null)
                return NotFound();

            orderToUpdate.Total =
                await _orderTotalService.GetOrderTotalFromUpdate(orderToUpdate.LineItems, orderRequest.LineItems);
        
            _mapper.Map(orderRequest, orderToUpdate);

            bool updated = await _orderRepository.UpdateAsync(orderToUpdate);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        
        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _orderRepository.DeleteAsync(id);
        
            if (!deleted)
                return NotFound();
        
            return NoContent();
        }
    }
}