using Gallery.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Service
{
    static class Program
    {
        static void Main()
        {
            Service service = new Service();
            ServiceManager.Run(service, Environment.GetCommandLineArgs());
        }
    }
}
