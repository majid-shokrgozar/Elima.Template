using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}

public interface IListQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, ListResult<TResponse>>
    where TQuery : IListQuery<TResponse>
{
}

public interface IPagedQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, PagedResult<TResponse>>
    where TQuery : IPagedQuery<TResponse>
{
}
