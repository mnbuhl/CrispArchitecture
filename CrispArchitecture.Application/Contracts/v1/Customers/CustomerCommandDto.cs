using System.ComponentModel.DataAnnotations;

namespace CrispArchitecture.Application.Contracts.v1.Customers
{
    public class CustomerCommandDto
    {
        [Required]
        public string Name { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}