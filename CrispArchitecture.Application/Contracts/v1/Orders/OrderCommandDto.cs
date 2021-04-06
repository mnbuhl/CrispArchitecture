using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class OrderCommandDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public ICollection<LineItemCommandDto> LineItems { get; set; }
    }
}