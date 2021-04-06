using System;
using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Domain.Entities
{
    public class Test
    {
        [Key]
        public Guid Id { get; set; }

        public string TestValue1 { get; set; }
        public int TestValue2 { get; set; }

        [EmailAddress]
        public string TestEmail { get; set; }
    }
}