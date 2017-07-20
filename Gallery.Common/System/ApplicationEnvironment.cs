using Gallery.Common.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gallery.Common.System
{
    public sealed class ApplicationEnvironment
    {
        readonly string name;
        readonly int priority;

        ApplicationEnvironment(string name, int priority)
        {
            this.name = name;
            this.priority = priority;

            string pattern = String.Format("-{0}$", name);

            ConnectionStrings = ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>()
                    .Where(x => Regex.IsMatch(x.Name, pattern, RegexOptions.IgnoreCase))
                    .Select(x => new { Name = Regex.Replace(x.Name, pattern, String.Empty, RegexOptions.IgnoreCase), x.ConnectionString })
                    .ToDictionary(x => x.Name, x => x.ConnectionString);

            NameValueCollection nameValueCollection = ConfigurationManager.AppSettings;
            //Flattens the collection. This ensures that keys with multiple values are always treated as single key, value pair.
            AppSettings = nameValueCollection.AllKeys.SelectMany(nameValueCollection.GetValues, (key, value) => new KeyValuePair<string, string>(key, value))
                    .Where(x => Regex.IsMatch(x.Key, pattern, RegexOptions.IgnoreCase))
                    .ToDictionary(x => Regex.Replace(x.Key, pattern, String.Empty, RegexOptions.IgnoreCase), x => x.Value);
        }

        public CryptographyProvider CryptographyProvider { get; private set; }
        public Dictionary<string, string> ConnectionStrings { get; private set; }
        public Dictionary<string, string> AppSettings { get; private set; }

        static ApplicationEnvironment()
        {
            //To be able to "lookup" an environment we need a collection where we can perform the lookup on.
            List<ApplicationEnvironment> applicationEnvironment = new List<ApplicationEnvironment> { Local, Test, Integration, Production };

            //Path for 32-bit: %windir%\Microsoft.NET\Framework\[version]\config\machine.config
            //Path for 64-bit: %windir%\Microsoft.NET\Framework64\[version]\config\machine.config 
            Configuration machineConfiguration = ConfigurationManager.OpenMachineConfiguration();

            //We are looking for an appSetting with the key "ApplicationEnvironment" in the machine.config file.
            KeyValueConfigurationElement environmentSetting = machineConfiguration.AppSettings.Settings["ApplicationEnvironment"];
            if (environmentSetting != null)
            {
                //As we have an implicit operator overload we can compare strings with ApplicationEnvironments.
                Current = applicationEnvironment.Find(x => String.Equals(x, environmentSetting.Value, StringComparison.InvariantCultureIgnoreCase));
            }

            KeyValueConfigurationElement enciphermentThumbprintSetting = machineConfiguration.AppSettings.Settings["EnciphermentThumbprint"];
            //This server is not prepared for config value encryption.
            if (enciphermentThumbprintSetting == null)
            {
                return;
            }

            X509Certificate2 certificate = CryptographyProvider.GetCertificateByThumbprint(enciphermentThumbprintSetting.Value, false, StoreLocation.LocalMachine);
            if (certificate == null)
            {
                throw new InvalidOperationException($"Certificate with thumbprint '{enciphermentThumbprintSetting.Value}' was not found in the store 'LocalMachine'.");
            }
            //The key exchange that corresponds to certificate thumbprint must be stored in the machine config also.
            KeyValueConfigurationElement keyExchangeSetting = machineConfiguration.AppSettings.Settings["KeyExchange"];
            byte[] keyExchange = Convert.FromBase64String(keyExchangeSetting.Value);

            //The application must have the right to access the private key of the certificate with the given thumbprint.
            AsymmetricAlgorithm asymmetricAlgorithm = certificate.PrivateKey;
            if (asymmetricAlgorithm == null)
            {
                throw new InvalidOperationException($"Access to private key for certificate with thumbprint '{enciphermentThumbprintSetting.Value}' was denied.");
            }

            Current.CryptographyProvider = new CryptographyProvider(asymmetricAlgorithm, keyExchange);
        }

        public static ApplicationEnvironment Current { get; internal set; }

        public static readonly ApplicationEnvironment Local = new KeyValuePair<string, int>("Local", 10);
        public static readonly ApplicationEnvironment Test = new KeyValuePair<string, int>("Test", 20);
        public static readonly ApplicationEnvironment Integration = new KeyValuePair<string, int>("Integration", 30);
        public static readonly ApplicationEnvironment Production = new KeyValuePair<string, int>("Production", 40);

        public static implicit operator ApplicationEnvironment(KeyValuePair<string, int> applicationEnvironment)
        {
            return new ApplicationEnvironment(applicationEnvironment.Key, applicationEnvironment.Value);
        }

        public static implicit operator string(ApplicationEnvironment applicationEnvironment)
        {
            //This works as the implicit operator has access to the private members.
            return applicationEnvironment.name;
        }

        public static bool operator <(ApplicationEnvironment left, ApplicationEnvironment right)
        {
            return left.priority < right.priority;
        }

        public static bool operator >(ApplicationEnvironment left, ApplicationEnvironment right)
        {
            return left.priority > right.priority;
        }
    }
}
