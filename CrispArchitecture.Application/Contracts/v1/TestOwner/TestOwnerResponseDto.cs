using System;
using CrispArchitecture.Application.Contracts.v1.Test;

namespace CrispArchitecture.Application.Contracts.v1.TestOwner
{
    public class TestOwnerResponseDto
    {
        public Guid Id { get; set; }
        
        public TestResponseDto Test { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}