using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class LimitedResultQuery<TResponse> : BaseLimitedResultRequestDto, IListQuery<TResponse>, ILimitedResultQuery<TResponse>
{
}

