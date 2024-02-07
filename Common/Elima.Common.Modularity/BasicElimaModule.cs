using System.Reflection;

namespace Elima.Common.Modularity;

public abstract class BasicElimaModule
{

    public Assembly ThisAssembly => this.GetType().Assembly;

    public Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {

        return Task.CompletedTask;
    }

    public virtual Task PostConfigureServicesAsync(ServiceConfigurationContext context)
    {

        return Task.CompletedTask;
    }

    public virtual Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        return Task.CompletedTask;
    }

    internal static void CheckElimaModuleType(Type moduleType)
    {
        if (!IsElimaModule(moduleType))
        {
            throw new ArgumentException("Given type is not an Elima module: " + moduleType.AssemblyQualifiedName);
        }
    }

    public static bool IsElimaModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return
            typeInfo.IsClass &&
            !typeInfo.IsAbstract &&
            !typeInfo.IsGenericType &&
            typeof(IElimaModule).GetTypeInfo().IsAssignableFrom(type);
    }
}

