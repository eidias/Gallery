using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.BaseTypes
{
    public abstract class DirectoryElement
    {
        [DirectoryProperty("objectClass")]
        public object[] ObjectClass { get; set; }
        [DirectoryProperty("objectGUID")]
        public byte[] ObjectGuid { get; set; }
        [DirectoryProperty("whenCreated")]
        public DateTime? Created { get; set; }
        [DirectoryProperty("whenChanged")]
        public DateTime? Modified { get; set; }

        protected void SetMappedProperties(DirectoryEntry directoryEntry)
        {
            foreach (var property in GetDirectoryProperties())
            {
                directoryEntry.Properties[GetDirectoryPropertyAttribute(property).SchemaAttributeName].Value = property.GetValue(this, null);
            }
            directoryEntry.CommitChanges();
        }

        public DirectoryElement GetFromDirectoryEntry(DirectoryEntry directoryEntry)
        {
            foreach (var property in GetDirectoryProperties())
            {
                //Calling SetValue will automatically convert to the property type.
                property.SetValue(this, directoryEntry.Properties[GetDirectoryPropertyAttribute(property).SchemaAttributeName].Value, null);
            }
            return this;
        }

        IEnumerable<PropertyInfo> GetDirectoryProperties()
        {
            //We only look for properties when there is an attribute available.
            return GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(DirectoryPropertyAttribute), false).Any());
        }

        DirectoryPropertyAttribute GetDirectoryPropertyAttribute(PropertyInfo propertyInfo)
        {
            //The enumeration is already filtered for properties containing DirectoryPropertyAttributes.
            return propertyInfo.GetCustomAttributes(typeof(DirectoryPropertyAttribute), false).Single() as DirectoryPropertyAttribute;
        }
    }
}
