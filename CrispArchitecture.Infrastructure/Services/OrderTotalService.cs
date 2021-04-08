using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CrispArchitecture.Application.Contracts.v1.Orders;
using CrispArchitecture.Application.Interfaces;
using CrispArchitecture.Application.Specifications.Products;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Infrastructure.Services
{
    public class OrderTotalService : IOrderTotalService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public OrderTotalService(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<double> GetOrderTotal(ICollection<LineItem> lineItems)
        {
            double total = 0;

            foreach (var lineItem in lineItems)
            {
                var product = await _productRepository.GetAsync(new ProductsSpecification(lineItem.ProductId));

                total += product.Price * lineItem.Amount;
            }

            return total;
        }

        public async Task<double> GetOrderTotalFromUpdate(ICollection<LineItem> lineItems = null,
            ICollection<LineItemCommandDto> lineItemDtos = null)
        {
            List<LineItem> lineItemsList = new List<LineItem>();
            
            if (lineItems != null)
            {
                lineItemsList.AddRange(lineItems);
            }
            
            if (lineItemDtos != null)
            {
                lineItemsList.AddRange(_mapper.Map<ICollection<LineItem>>(lineItemDtos));
            }
            
            return await GetOrderTotal(lineItemsList);
        }
    }
}