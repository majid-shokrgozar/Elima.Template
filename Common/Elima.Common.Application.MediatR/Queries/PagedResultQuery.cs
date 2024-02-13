using Elima.Common.Results;
using System;
using System.Collections.Generic;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class PagedResultQuery<TResponse> : BasePagedResultRequestDto, IPagedQuery<TResponse>
{
}
