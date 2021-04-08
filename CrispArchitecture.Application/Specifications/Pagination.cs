using System.Collections.Generic;

namespace CrispArchitecture.Application.Specifications
{
    public class Pagination<T> where T : class
    {
        public int PageIndex { get; }
        public int PageSize { get; }
        public int Count { get; }
        public IList<T> Data { get; }

        public Pagination(int pageIndex, int pageSize, int count, IList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}