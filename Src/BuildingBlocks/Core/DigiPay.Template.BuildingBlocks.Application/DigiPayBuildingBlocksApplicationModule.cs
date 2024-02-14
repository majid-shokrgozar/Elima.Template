using Autofac.Core;
using DigiPay.Template.CoreModule.Application;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.BuildingBlocks.Application;

[DependsOn(typeof(DigiPayCoreModuleApplicationModule))]
public class DigiPayBuildingBlocksApplicationModule : ElimaAutofacModule
{
}
