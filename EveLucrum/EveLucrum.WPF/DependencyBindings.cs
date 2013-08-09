using EveLucrum.ApplicationServices;
using EveLucrum.Data;
using EveLucrum.Domain;
using Ninject.Modules;

namespace EveLucrum.WPF
{
    public class DependencyBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILucrumContext>().To<LucrumContextEF>().InThreadScope();
            Bind<IAPIService>().To<APIService>().InThreadScope();
        }
    }
}