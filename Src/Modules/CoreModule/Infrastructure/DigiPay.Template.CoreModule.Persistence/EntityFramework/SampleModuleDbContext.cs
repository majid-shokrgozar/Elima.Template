﻿using Elima.Common.EntityFramework.Auditing;
using Elima.Common.EntityFramework.Data;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using DigiPay.Template.CoreModule.Domain.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DigiPay.Template.CoreModule.Persistence.EntityFramework;

public class SampleModuleDbContext : EfCoreDbContext<SampleModuleDbContext>
{
    public DbSet<Sample> Samples { get; set; }
    public SampleModuleDbContext(
        DbContextOptions<SampleModuleDbContext> options,
        ILogger<SampleModuleDbContext> logger,
        IAuditPropertySetter auditPropertySetter,
        IDataFilter dataFilter) 
        : base(options, logger, auditPropertySetter, dataFilter)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DigiPayCoreModulePersistenceModule).Assembly);
    }
}
