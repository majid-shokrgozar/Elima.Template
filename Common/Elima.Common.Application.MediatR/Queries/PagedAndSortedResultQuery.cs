using Elima.Common.Application.Dtos;
using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class PagedAndSortedResultQuery<TResponse> : PagedAndSortedResultRequestDto, IPagedQuery<TResponse>, IPagedAndSortedResultQuery<TResponse>
{
}
