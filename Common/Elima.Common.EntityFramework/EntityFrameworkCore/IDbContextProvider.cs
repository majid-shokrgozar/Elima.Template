using System;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{

    Task<TDbContext> GetDbContextAsync();
}
