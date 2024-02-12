using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
