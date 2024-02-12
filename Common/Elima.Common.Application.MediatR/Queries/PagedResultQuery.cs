using Elima.Common.Application.Dtos;
using Elima.Common.Results;
using System;
using System.Collections.Generic;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class PagedResultQuery<TResponse> : PagedResultRequestDto, IPagedQuery<TResponse>, IPagedResulQuery<TResponse>
{
}
