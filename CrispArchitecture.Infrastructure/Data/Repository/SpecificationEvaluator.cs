using System.Linq;
using CrispArchitecture.Application.Specifications;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Infrastructure.Data.Repository
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
            ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.Includes != null)
            {
                query = specification.Includes(query);
            }

            return query;
        }
    }
}