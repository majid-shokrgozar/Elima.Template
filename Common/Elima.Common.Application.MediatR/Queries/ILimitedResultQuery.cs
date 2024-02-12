using Elima.Common.Application.Dtos;
using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Queries;

public interface ILimitedResultQuery<TResponse> : IRequest<Result<TResponse>>, ILimitedResultRequest
{
}
