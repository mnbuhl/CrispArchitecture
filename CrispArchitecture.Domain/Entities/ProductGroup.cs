using System.Collections.Generic;

namespace CrispArchitecture.Domain.Entities
{
    public class ProductGroup : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}