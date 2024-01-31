using Microsoft.EntityFrameworkCore;

namespace Elima.Common.EntityFramework;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {

    }
}
