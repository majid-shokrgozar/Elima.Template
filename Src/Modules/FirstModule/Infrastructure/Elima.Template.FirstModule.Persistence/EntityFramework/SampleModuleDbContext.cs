using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Template.FirstModule.Domain.Samples;
using Microsoft.EntityFrameworkCore;

namespace Elima.Template.FirstModule.Persistence.EntityFramework;

public class SampleModuleDbContext : EfCoreDbContext<SampleModuleDbContext>
{
    public DbSet<Sample> Samples { get; set; }
    public SampleModuleDbContext(DbContextOptions<SampleModuleDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElimaFirstModulePersistenceModule).Assembly);
    }
}
