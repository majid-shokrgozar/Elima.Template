using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Elima.Common.Modularity;

public class ServiceConfigurationContext
{
    /// <summary>
    /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    public IServiceCollection Services { get; private set; }


    /// <summary>
    /// Provides information about the web hosting environment an application is running.
    /// </summary>
    public IWebHostEnvironment Environment { get; private set; }

    /// <summary>
    /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    public ConfigurationManager Configuration { get; private set; }

    /// <summary>
    /// </summary>
    public ConfigureHostBuilder Host { get; private set; }

    /// <summary>
    /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
    /// </summary>
    public ILoggingBuilder Logging { get; private set; }

    /// <summary>
    /// Allows enabling metrics and directing their output.
    /// </summary>
    public IMetricsBuilder Metrics { get; private set; }


    public IDictionary<string, object?> Items { get; }

    public object? this[string key]
    {
        get => Items[key] ?? default;
        set => Items[key] = value;
    }

    public ServiceConfigurationContext(
        [NotNull] IServiceCollection services,
        IWebHostEnvironment environment,
        ConfigurationManager configuration,
        ConfigureHostBuilder host,
        ILoggingBuilder logging,
        IMetricsBuilder metrics
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logging);
        ArgumentNullException.ThrowIfNull(metrics);
        Services = services;
        Environment = environment;
        Configuration = configuration;
        Logging = logging;
        Metrics = metrics;
        Items = new Dictionary<string, object?>();
        Host = host;
    }
}
