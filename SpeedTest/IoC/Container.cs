using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;

namespace SpeedTest.IoC
{
    abstract class Container
    {
        public Container()
        {
            serviceContainer = new ServiceContainer();
        }

        readonly protected IServiceContainer serviceContainer;

        public T GetService<T>()
        {
            return serviceContainer.GetInstance<T>();
        }
    }
}
