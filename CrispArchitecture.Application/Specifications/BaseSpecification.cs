using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CrispArchitecture.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; private set; }

        public BaseSpecification()
        {
        }
        
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void SetIncludes(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            Includes = includes;
        }
    }
}