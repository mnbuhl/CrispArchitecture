using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Test
{
    public class CreateTestDto
    {
        public string TestValue1 { get; set; }
        public int TestValue2 { get; set; }
        [EmailAddress]
        public string TestEmail { get; set; }
    }
}