using System;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class LineItemCommandDto
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}