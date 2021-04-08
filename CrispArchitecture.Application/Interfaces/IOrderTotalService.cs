using System.Collections.Generic;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Orders;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Application.Interfaces
{
    public interface IOrderTotalService
    {
        Task<double> GetOrderTotal(ICollection<LineItem> lineItems);

        Task<double> GetOrderTotalFromUpdate(ICollection<LineItem> lineItems = null,
            ICollection<LineItemCommandDto> lineItemDtos = null);
    }
}