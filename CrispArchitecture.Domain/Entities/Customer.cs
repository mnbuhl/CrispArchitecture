using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; } 
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}