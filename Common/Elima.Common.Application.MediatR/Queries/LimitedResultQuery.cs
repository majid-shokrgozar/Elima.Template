using Elima.Common.Application.Dtos;
using System;

namespace Elima.Common.Application.MediatR.Queries;

[Serializable]
public class LimitedResultQuery<TResponse> : LimitedResultRequestDto, IListQuery<TResponse>, ILimitedResultQuery<TResponse>
{
}
