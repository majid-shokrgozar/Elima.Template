using Autofac.Core;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using DigiPay.Template.FirstModule.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.BuildingBlocks.Application;

[DependsOn(typeof(DigiPayFirstModuleApplicationModule))]
public class DigiPayBuildingBlocksApplicationModule : ElimaAutofacModule
{
}
