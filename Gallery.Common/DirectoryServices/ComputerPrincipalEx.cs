using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.DirectoryServices
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("computer")]
    public class ComputerPrincipalEx : ComputerPrincipal
    {
        //Must implement the base constructors.
        public ComputerPrincipalEx(PrincipalContext context)
            : base(context)
        {
        }

        public ComputerPrincipalEx(PrincipalContext context, string samAccountName, string password, bool enabled)
            : base(context, samAccountName, password, enabled)
        {
        }

        //Must implement all find methods we want to use on this principal.
        public static new ComputerPrincipalEx FindByIdentity(PrincipalContext context, string identityValue)
        {
            return (ComputerPrincipalEx)FindByIdentityWithType(context, typeof(ComputerPrincipalEx), identityValue);
        }

        //Method to provide a simple access from the principal to the DirectoryEntry
        public DirectoryEntry DirectoryEntry
        {
            get
            {
                return GetUnderlyingObject() as DirectoryEntry;
            }
        }

        public ICollection GetAllProperties()
        {
            return DirectoryEntry.Properties.Values;
        }

        public IEnumerable<TElement> GetCustomElements<TElement>() where TElement : DirectoryElement, new()
        {
            if (DirectoryEntry == null)
            {
                yield break;
            }
            foreach (DirectoryEntry child in DirectoryEntry.Children)
            {
                TElement element = new TElement();
                yield return element.GetFromDirectoryEntry(child) as TElement;
            }
        }

        [DirectoryProperty("userAccountControl")]
        public UserAccountControl UserAccountControl
        {
            get
            {
                return (UserAccountControl)ExtensionGet("userAccountControl")[0];
            }

            set
            {
                //Check if we need to cast value to (int).
                ExtensionSet("userAccountControl", value);
            }
        }
    }
}
