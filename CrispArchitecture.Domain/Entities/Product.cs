using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrispArchitecture.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Guid ProductGroupId { get; set; }

        [ForeignKey(nameof(ProductGroupId))]
        public ProductGroup ProductGroup { get; set; }
    }
}