using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace EveLucrum.WPF
{
    public static class DependencyResolver
    {
        private static IKernel kernel = new StandardKernel(new DependencyBindings());

        public static T Get<T>() where T : class
        {
            return kernel.Get<T>();
        }
    }
}
