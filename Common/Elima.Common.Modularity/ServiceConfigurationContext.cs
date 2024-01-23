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
        ILoggingBuilder logging, 
        IMetricsBuilder metrics)
    {
        ArgumentNullException.ThrowIfNull(services,nameof(services));
        ArgumentNullException.ThrowIfNull(environment,nameof(environment));
        ArgumentNullException.ThrowIfNull(configuration,nameof(configuration));
        ArgumentNullException.ThrowIfNull(logging,nameof(logging));
        ArgumentNullException.ThrowIfNull(metrics, nameof(metrics));
        Services = services;
        Environment = environment;
        Configuration = configuration;
        Logging = logging;
        Metrics = metrics;
        Items = new Dictionary<string, object?>();
    }
}
