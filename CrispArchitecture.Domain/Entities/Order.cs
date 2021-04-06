using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrispArchitecture.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<LineItem> LineItems { get; set; }
        public double Total { get; set; }
        public bool IsPaid { get; set; }

        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }
}