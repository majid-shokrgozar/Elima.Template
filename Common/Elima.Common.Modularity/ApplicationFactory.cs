using System.Diagnostics.CodeAnalysis;

namespace Elima.Common.Modularity;

public static class ApplicationFactory
{
    public async static Task<TStartupModule> CreateAsync<TStartupModule>(ServiceConfigurationContext context)
        where TStartupModule : IElimaModule
    {
        
        var types = ModuleHelper.FindAllModuleTypes(typeof(TStartupModule));
        types.Reverse();
        foreach (var type in types)
        {
            var module = Create(type);
            await module.PreConfigureServicesAsync(context);
            await module.BaseConfigureServicesAsync(context);
            await module.ConfigureServicesAsync(context);
            await module.PostConfigureServicesAsync(context);
        }
        TStartupModule app = Create<TStartupModule>();
       
        return app;
    }

    public async static Task<TStartupModule> Initialize<TStartupModule>(ApplicationInitializationContext context)
       where TStartupModule : IElimaModule
    {

        var types = ModuleHelper.FindAllModuleTypes(typeof(TStartupModule));
        types.Reverse();
        foreach (var type in types)
        {
            var module = Create(type);
            await module.OnApplicationInitializationAsync(context);
        }
        TStartupModule app = Create<TStartupModule>();

        return app;
    }

    private static TStartupModule Create<TStartupModule>()
    {
        return (TStartupModule)Activator.CreateInstance(typeof(TStartupModule))!;
    }

    private static IElimaModule Create(Type type)
    {
        return (IElimaModule)Activator.CreateInstance(type)!;
    }
}
