using System.Reflection;

namespace Elima.Common.Modularity;

public abstract class ElimaModule:IElimaModule
{
    public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context) { 

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
