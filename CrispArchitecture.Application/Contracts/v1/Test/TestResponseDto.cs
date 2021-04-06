using System;

namespace CrispArchitecture.Application.Contracts.v1.Test
{
    public class TestResponseDto
    {
        public Guid Id { get; set; }
        public string TestValue1 { get; set; }
        public int TestValue2 { get; set; }
        public string TestEmail { get; set; }
    }
}