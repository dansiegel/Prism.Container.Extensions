using Prism.Modularity;

namespace Shiny.Prism.Modularity
{
    internal interface IShinyPrismModuleInitializer
    {
        IModule LoadShinyModule(IModuleInfo moduleInfo);
    }
}