using Gallery.Service.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$
{
    [RunInstaller(true)]
    [System.ComponentModel.DesignerCategory("")]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
			Type type = this.GetType();
            AssemblyName assemblyName = type.Assembly.GetName();

            ServiceProcessInstaller serviceProcessInstaller = ServiceManager.GetServiceProcessInstaller();
            Installers.Add(serviceProcessInstaller);

            ServiceInstaller serviceInstaller = ServiceManager.GetServiceInstaller(assemblyName.Name);
            Installers.Add(serviceInstaller);
        }
    }
}
