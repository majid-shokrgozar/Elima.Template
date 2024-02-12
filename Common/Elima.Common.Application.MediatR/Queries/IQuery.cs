using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
