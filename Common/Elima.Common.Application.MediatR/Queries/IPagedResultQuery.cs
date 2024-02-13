using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface IPagedResultQuery<TResponse> : IRequest<Result<TResponse>>, IPagedResultRequest
{
}
