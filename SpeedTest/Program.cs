using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedTest.Forms;
using SpeedTest.IoC;

namespace SpeedTest
{
    static class Program
    {
        static Container container;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                container = new HostContainer();
            else
                container = new ClientContainer(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.GetService<Form>());
        }
    }
}
