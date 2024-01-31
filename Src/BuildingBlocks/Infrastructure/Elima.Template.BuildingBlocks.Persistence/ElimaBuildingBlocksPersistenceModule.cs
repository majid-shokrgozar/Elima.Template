using Elima.Common.Modularity;
using Elima.Template.FirstModule.Persistence;

namespace Elima.Template.BuildingBlocks.Persistence;

[DependsOn(typeof(ElimaFirstModulePersistenceModule))]
public class ElimaBuildingBlocksPersistenceModule : ElimaModule
{
}
