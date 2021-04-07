using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Orders;
using CrispArchitecture.Application.Interfaces;
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
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _orderService.GetOrderAsync(id);

            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderResponseDto>(order));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Order> orders = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<List<OrderResponseDto>>(orders));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var order = _mapper.Map<Order>(orderRequest);

            bool created = await _orderService.CreateOrderAsync(order);

            if (!created)
                return BadRequest();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, order.Id.ToString());
            var orderResponse = _mapper.Map<OrderResponseDto>(order);
        
            return Created(locationUri, orderResponse);
        }
        
        [HttpPut("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var orderToUpdate = await _orderService.GetOrderAsync(id);
        
            if (orderToUpdate == null)
                return NotFound();
        
            _mapper.Map(orderRequest, orderToUpdate);

            bool updated = await _orderService.UpdateOrderAsync(orderToUpdate);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        
        [HttpDelete("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool deleted = await _orderService.DeleteOrderAsync(id);
        
            if (!deleted)
                return NotFound();
        
            return NoContent();
        }
    }
}