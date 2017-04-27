using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.IoC
{
    class HostContainer : Container
    {
        public HostContainer()
            : base()
        {
            serviceContainer.Register<System.Windows.Forms.Form, Forms.FormHost>();
        }
    }
}
