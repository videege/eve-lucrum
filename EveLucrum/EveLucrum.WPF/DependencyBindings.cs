using EveLucrum.ApplicationServices;
using EveLucrum.Data;
using EveLucrum.Domain;
using EveLucrum.Infrastructure.Market;
using Ninject.Modules;

namespace EveLucrum.WPF
{
    public class DependencyBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILucrumContext>().To<LucrumContextEF>().InThreadScope();
            Bind<IAPIService>().To<APIService>();
            Bind<IMarketReader>().To<EveCentralReader>();
            Bind<IMarketService>().To<MarketService>();
        }
    }
}