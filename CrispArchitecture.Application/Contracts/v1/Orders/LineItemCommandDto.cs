using System;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Orders
{
    public class LineItemCommandDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}