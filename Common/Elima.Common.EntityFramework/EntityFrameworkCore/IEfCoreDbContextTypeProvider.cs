using System;

namespace Elima.Common.EntityFramework.EntityFrameworkCore;

public interface IEfCoreDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);
}
