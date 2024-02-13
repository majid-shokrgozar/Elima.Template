using Elima.Common.Results;
using System;
using System.Collections.Generic;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public record PagedResultQuery<TResponse> : BasePagedResultRequestDto, IPagedQuery<TResponse>
{
}
