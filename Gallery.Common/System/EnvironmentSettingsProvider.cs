using Gallery.Common.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.System
{
    public sealed class EnvironmentSettingsProvider : SettingsProvider
    {
        //Called the first time a setting is requested.
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(this.ApplicationName, config);
        }

        public override string ApplicationName
        {
            get
            {
                //Implementation according to http://msdn.microsoft.com/en-us/library/8eyb2ct1(v=vs.110).aspx 
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
            set
            {
                //Intentionally left blank
            }
        }

        /// <summary>
        /// The method is called for all settings when a setting is accessed for the first time.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection">Contains all defined settings (retrieved via reflection)</param>
        /// <returns></returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            string groupName = Convert.ToString(context["GroupName"]);
            SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
            foreach (SettingsProperty settingsProperty in collection)
            {
                SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(settingsProperty);

                SpecialSettingAttribute specialSettingAttribute = settingsProperty.Attributes[typeof(SpecialSettingAttribute)] as SpecialSettingAttribute;
                if (specialSettingAttribute.SpecialSetting == SpecialSetting.ConnectionString)
                {
                    settingsPropertyValue.PropertyValue = ApplicationEnvironment.Current.ConnectionStrings[settingsProperty.Name];
                    //TODO: Refactor this in a separate method.
                }
                if (specialSettingAttribute.SpecialSetting == SpecialSetting.WebServiceUrl)
                {
                    //TODO: Do we need to treat these settings somehow?
                }

                //Default naming scheme for settings: <GroupName>-<PropertyName>-<Environment>
                KeyValuePair<string, string> appSetting = ApplicationEnvironment.Current.AppSettings.SingleOrDefault(x => x.Key == groupName + "-" + settingsProperty.Name);
                if (appSetting.Key != null)
                {
                    settingsPropertyValue.PropertyValue = EvaluatePropertyValue(appSetting.Value);
                }
                settingsPropertyValueCollection.Add(settingsPropertyValue);
            }
            return settingsPropertyValueCollection;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            throw new NotImplementedException();
        }

        string EvaluatePropertyValue(string value)
        {
            string[] values = value.Split('#');

            //Handle encrypted config values.
            if (values.Length == 2 && values[0].Equals("AES", StringComparison.InvariantCultureIgnoreCase))
            {
                CryptographyProvider cryptographyProvider = ApplicationEnvironment.Current.CryptographyProvider;
                if (cryptographyProvider == null)
                {
                    string message = "An encrypted config value was found but no cryptography provider is available.";
                    throw new InvalidOperationException(message);
                }
                //TODO: How should we react if we cannot decrypt a value for strange reasons?
                byte[] encryptedBytes = Convert.FromBase64String(values[1]);
                byte[] plainBytes = cryptographyProvider.Decrypt(encryptedBytes).ToArray();
                return Encoding.UTF8.GetString(plainBytes);
            }
            //Handle all other values.
            return value;
        }
    }
}
