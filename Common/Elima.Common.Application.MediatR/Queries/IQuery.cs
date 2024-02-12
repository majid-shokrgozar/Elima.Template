using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public interface IPagedQuery<TResponse> : IRequest<PagedResult<TResponse>>
{
}


public interface IListQuery<TResponse> : IRequest<ListResult<TResponse>>
{
}
