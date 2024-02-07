using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.Auditing;
using Elima.Common.EntityFramework.Data;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Elima.Common.EntityFramework;

public static class DependencyServiceExtentions
{
    public static IServiceCollection AddAuditPropertySetter(this IServiceCollection services)
    {
        services.AddScoped<IAuditPropertySetter, AuditPropertySetter>();
        return services;
    }

    public static IServiceCollection AddDataFilter(this IServiceCollection services,Action<DataFilterOptions> option)
    {
        services.AddSingleton(typeof(IDataFilter), typeof(DataFilter));

        services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

        services.Configure<DataFilterOptions>(option);
        return services;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, Type context,params Type[] moreContext)
    {
        services.AddScoped<IUnitOfWork>((serviceProvider) =>
        {
            var list = new List<IEfCoreDbContext>();

            Type[] contexts = [context,.. moreContext];

            foreach (var item in contexts)
            {
                if (serviceProvider.GetService(item) is IEfCoreDbContext dbContext)
                    list.Add(dbContext);
            }

            return new UnitOfWork(list);
        });

        return services;
    }
}
