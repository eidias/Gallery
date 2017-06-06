using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.DirectoryServices
{
    public class DirectoryHelper<T> : IDisposable where T : new()
    {
        PrincipalContext principalContext;
        Principal principal;

        T MapDirectoryProperties(DirectoryEntry directoryEntry, T t)
        {
            foreach (var property in t.GetType().GetProperties())
            {
                //We only look for properties when there is an attribute available.
                var directoryProperty = property.GetCustomAttributes(typeof(DirectoryPropertyAttribute), false).SingleOrDefault() as DirectoryPropertyAttribute;
                if (directoryProperty != null)
                {
                    //Here we would possibly see a NullReferenceException if the specified property does not exist.
                    property.SetValue(t, directoryEntry.Properties[directoryProperty.SchemaAttributeName].Value, null);
                }
            }
            return t;
        }

        public DirectoryHelper(string identityValue)
        {
            principalContext = new PrincipalContext(ContextType.Domain);
            //All relevant principals (user, computer and group) derive from Principal.
            principal = Principal.FindByIdentity(principalContext, identityValue);
        }

        public void Dispose()
        {
            //Principal context implements IDisposable.
            principalContext.Dispose();
        }

        public IEnumerable<T> GetUnderlyingEntities()
        {
            //Some objects are not stored as principal but as child elements of the respective object
            foreach (DirectoryEntry child in GetDirectoryEntry().Children)
            {
                yield return MapDirectoryProperties(child, new T());
            }
        }

        public DirectoryEntry GetDirectoryEntry()
        {
            //Every principal also relates to a directoryentry.
            //We have to decide what way to go: Either with directroy entry and mapped properties or with custom principals and type converters...
            return principal.GetUnderlyingObject() as DirectoryEntry;
        }

        public DirectoryEntries GetChildren()
        {
            return GetDirectoryEntry().Children;
        }

        public ComputerPrincipal GetComputerPrincipal()
        {
            return principal as ComputerPrincipal;
        }

        public UserPrincipal GetUserPrincipal()
        {
            return principal as UserPrincipal;
        }

        public GroupPrincipal GetGroupPrincipal()
        {
            return principal as GroupPrincipal;
        }
    }
}
