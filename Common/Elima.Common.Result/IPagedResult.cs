using System;
using System.Text;

namespace Elima.Common.Results;

public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
{

}
