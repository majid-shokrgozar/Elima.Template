using Autofac.Core;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Elima.Template.FirstModule.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.BuildingBlocks.Application;

[DependsOn(typeof(ElimaFirstModuleApplicationModule))]
public class ElimaBuildingBlocksApplicationModule : ElimaAutofacModule
{
}
