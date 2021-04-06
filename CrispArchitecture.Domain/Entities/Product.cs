using System;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
    }
}