using System;

namespace CrispArchitecture.Application.Specifications.Products
{
    public class ProductParams : BaseSpecificationParams
    {
        public Guid? ProductGroupId { get; set; }
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
        
        private string _search;
    }
}