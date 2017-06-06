using Gallery.Common.ExtendedTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Tests.Helpers
{
    public class DirectoryHelperTests
    {
        public void TestComputerPrincipal()
        {
            var principalContext = new PrincipalContext(ContextType.Domain);
            var principal = ComputerPrincipalEx.FindByIdentity(principalContext, "MACHINENAME");
            foreach (PropertyValueCollection property in principal.GetAllProperties())
            {
                Debug.WriteLine(String.Format("{0}: {1}", property.PropertyName, property.Value));
            }
        }
    }
}
