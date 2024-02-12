using Elima.Common.Results;
using MediatR;

namespace Elima.Common.Application.MediatR.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
