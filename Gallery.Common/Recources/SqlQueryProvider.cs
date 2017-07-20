using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gallery.Common.Recources
{
    public class SqlQueryProvider<T>
    {
        const string Declaration = "Declaration";
        const string QueryPattern = @"--<(\w*)>--\r\n(.*)\r\n--</\1>--";
        const string FileNamePattern = @"\w*(?=\.sql)";

        public static readonly Dictionary<string, string> Queries = new Dictionary<string, string>();

        static SqlQueryProvider()
        {
            CreateDictionary();
        }

        static void CreateDictionary()
        {
            Assembly assembly = typeof(T).Assembly;

            EmbeddedResourceProvider embeddedResourceProvider = new EmbeddedResourceProvider(assembly);
            foreach (string manifestResourceName in embeddedResourceProvider.ManifestResourceNames)
            {
                Match fileNameMatch = Regex.Match(manifestResourceName, FileNamePattern);
                if(fileNameMatch.Success)
                {
                    string text = embeddedResourceProvider.GetText(manifestResourceName, Encoding.UTF8);
                    Regex regex = new Regex(QueryPattern, RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(text);
                    foreach (Match match in matchCollection)
                    {
                        if (match.Success)
                        {
                            string key = Convert.ToString(match.Groups[1]);
                            if (key == Declaration)
                            {
                                continue;
                            }
                            string value = Convert.ToString(match.Groups[2]);
                            Queries.Add(fileNameMatch.Value + "/" + key, value);
                        }
                    }
                }
            }
        }
    }
}
