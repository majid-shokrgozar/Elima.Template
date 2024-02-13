using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class PagedAndSortedResultQuery<TResponse> : BasePagedAndSortedResultRequestDto, IPagedQuery<TResponse>, IPagedAndSortedResultQuery<TResponse>
{
}
