using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrispArchitecture.Domain.Entities
{
    public class TestOwner
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TestId { get; set; }

        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}