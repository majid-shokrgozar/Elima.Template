using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public record PagedAndSortedResultQuery<TResponse> : BasePagedAndSortedResultRequestDto, IPagedQuery<TResponse>, IPagedAndSortedResultQuery<TResponse>
{
}
