using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest.IoC
{
    class ClientContainer : Container
    {
        public ClientContainer(string[] args)
        {
            serviceContainer.Register<System.Windows.Forms.Form, Forms.FormClient>();
        }
    }
}
