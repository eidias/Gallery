using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gallery.Common.Settings
{
    public class SettingsHelper
    {
        public void UpdateSettingType(BaseSetting setting)
        {
            Type settingType = setting.GetType();
            PropertyInfo[] propertyInfos = settingType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                if (typeof(IConvertible).IsAssignableFrom(propertyType))
                {
                    string settingName = String.Format("{0}.{1}", settingType.Name, propertyInfo.Name);
                    object settingValue = propertyInfo.GetValue(setting);
                    //DBSet.UserSettings.SaveOrUpdate(userProfileId, settingName, propertyType.FullName, settingValue);
                }
            }
            //Context.SaveChanges();
        }

        public bool TryGetUserSetting(Dictionary<string, object> settings, string settingName, out object value)
        {
            LazyInitializer.EnsureInitialized(ref settings, () =>
            {
                return new Dictionary<string, object>();

            });
            return settings.TryGetValue(settingName, out value);
        }

        public void PopulateSettingType(BaseSetting setting)
        {
            Type settingType = setting.GetType();
            PropertyInfo[] propertyInfos = settingType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                if (typeof(IConvertible).IsAssignableFrom(propertyType))
                {
                    object value;
                    if (TryGetUserSetting(null, $"{settingType.Name}.{propertyInfo.Name}", out value))
                    {
                        propertyInfo.SetValue(setting, value);
                        continue;
                    }
                    DefaultValueAttribute defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null)
                    {
                        propertyInfo.SetValue(setting, defaultValueAttribute.Value);
                    }
                }
            }
        }

        internal static Type GetType(string typeName)
        {
            Type type = null;
            try
            {
                type = Type.GetType(typeName, true);
            }
            catch
            {
                //Intentionally left blank.
            }
            return type;
        }

        internal static object CreateObject(Type type, string value)
        {
            //String objects have no parameterless constuctor
            if (type == typeof(string))
            {
                return value;
            }
            object obj = Activator.CreateInstance(type);
            try
            {
                obj = Convert.ChangeType(value, type);
            }
            catch
            {
                //Intentionally left blank.
            }
            return obj;
        }

        internal static Func<BaseSetting, object> ValueSelector = userSetting =>
        {
            Type type = GetType(userSetting.Type);
            if (type != null)
            {
                return CreateObject(type, userSetting.Value);
            }
            return null;
        };

        public Dictionary<string, object> Load(long userProfileId)
        {
            //return DbSet.Where(x => x.UserProfileId == userProfileId).ToDictionary(x => x.Name, ValueSelector);
            return new Dictionary<string, object>();
        }

        public BaseSetting SaveOrUpdate(string name, string type, object value, BaseSetting setting = null)
        {
            if (setting == null)
            {
                setting = new BaseSetting()
                {
                    Name = name,
                    Type = type
                };
                //DbSet.Add(userSetting);
            }
            //This is important e.g. for bool values to be properly converted.
            setting.Value = Convert.ToString(value);

            return setting;
        }
    }
}
