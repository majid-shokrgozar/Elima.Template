using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elima.Common.Modularity;

public class ApplicationInitializationContext
{

    public WebApplication ApplicationBuilder { get; private set; }

    /// <summary>
    /// The application's configured services.
    /// </summary>
    public IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// The application's configured <see cref="IConfiguration"/>.
    /// </summary>
    public IConfiguration Configuration { get; private set; }

    /// <summary>
    /// The application's configured <see cref="IWebHostEnvironment"/>.
    /// </summary>
    public IWebHostEnvironment Environment { get; private set; }

    /// <summary>
    /// Allows consumers to be notified of application lifetime events.
    /// </summary>
    public IHostApplicationLifetime Lifetime { get; private set; }

    /// <summary>
    /// The default logger for the application.
    /// </summary>
    public ILogger Logger { get; }

    public ApplicationInitializationContext(
        WebApplication applicationBuilder, 
        IServiceProvider serviceProvider, 
        IConfiguration configuration, 
        IWebHostEnvironment environment, 
        IHostApplicationLifetime lifetime, 
        ILogger logger)
    {
        ApplicationBuilder = applicationBuilder;
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Environment = environment;
        Lifetime = lifetime;
        Logger = logger;
    }

}
