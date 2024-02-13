using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public record LimitedResultQuery<TResponse> : BaseLimitedResultRequestDto, IListQuery<TResponse>, ILimitedResultQuery<TResponse>
{
}

