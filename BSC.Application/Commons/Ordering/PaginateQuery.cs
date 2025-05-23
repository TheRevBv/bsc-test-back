using BSC.Application.Commons.Bases.Request;

namespace BSC.Application.Commons.Ordering
{
    public static class PaginateQuery
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, BasePaginationRequest request)
        {
            return queryable.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        }
    }
}