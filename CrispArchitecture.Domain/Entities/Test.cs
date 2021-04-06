using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Domain.Entities
{
    public class Test
    {
        [Key]
        public Guid Id { get; set; }

        public string TestValue1 { get; set; }
        public int TestValue2 { get; set; }
        public string TestEmail { get; set; }

        public ICollection<TestOwner> TestOwners { get; set; }
    }
}