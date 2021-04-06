using System;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}