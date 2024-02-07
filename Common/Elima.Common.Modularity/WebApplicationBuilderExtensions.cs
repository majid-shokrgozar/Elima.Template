using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace Elima.Common.Modularity;

public static class WebApplicationBuilderExtensions
{
    public async static Task<TStartupModule> AddApplicationAsync<TStartupModule>(
    [NotNull] this WebApplicationBuilder builder)
        where TStartupModule : IElimaModule
    {
        return await ApplicationFactory.CreateAsync<TStartupModule>(new ServiceConfigurationContext(
            builder.Services,
            builder.Environment,
            builder.Configuration,
            builder.Host,
            builder.Logging,
            builder.Metrics
            ));
    }

    public async static Task<TStartupModule> InitializeApplicationAsync<TStartupModule>(
    [NotNull] this WebApplication webApplication)
        where TStartupModule : IElimaModule
    {
        return await ApplicationFactory.Initialize<TStartupModule>(new ApplicationInitializationContext(
            webApplication,
            webApplication.Services,
            webApplication.Configuration,
            webApplication.Environment,
            webApplication.Lifetime,
            webApplication.Logger
            ));
    }


}
