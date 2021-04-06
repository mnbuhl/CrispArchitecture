using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Api.Helpers;
using CrispArchitecture.Application.Contracts.v1.Orders;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrispArchitecture.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(id);

            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderResponseDto>(order));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Order> orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return Ok(_mapper.Map<List<OrderResponseDto>>(orders));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _mapper.Map<Order>(orderRequest);

            double total = 0;
            
            foreach (var item in order.LineItems)
            {
                var product = await _unitOfWork.ProductRepository.GetAsync(item.ProductId);

                if (product == null)
                {
                    total = -1;
                    break;
                }

                total += product.Price * item.Amount;
            }
            
            if (total <= 0)
                return BadRequest();
            
            order.Total = total;

            await _unitOfWork.OrderRepository.CreateAsync(order);
            await _unitOfWork.SaveAsync();
            
            string locationUri = new LocationUri().GetLocationUri(HttpContext.Request, order.Id.ToString());
            var orderResponse = _mapper.Map<OrderResponseDto>(order);

            return Created(locationUri, orderResponse);
        }
        
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderCommandDto orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderToUpdate = await _unitOfWork.OrderRepository.GetAsync(id);

            if (orderToUpdate == null)
                return NotFound();

            _mapper.Map(orderRequest, orderToUpdate);
            
            double total = 0;
            
            foreach (var item in orderToUpdate.LineItems)
            {
                var product = await _unitOfWork.ProductRepository.GetAsync(item.ProductId);

                if (product == null)
                {
                    total = -1;
                    break;
                }

                total += product.Price * item.Amount;
            }
            
            if (total <= 0)
                return BadRequest();
            
            orderToUpdate.Total = total;
            
            _unitOfWork.OrderRepository.Update(orderToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
        
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.OrderRepository.DeleteAsync(id);
            bool deleted = await _unitOfWork.SaveAsync() > 0;

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}