using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace PBTrainer.Utilities
{
    public class Settings
    {
        // Our isolated storage settings
        IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        public const string DrillDurationSettingKeyName = "DrillDuration";
        public const string WarningSettingKeyName = "Warning";
        public const string ResetSettingKeyName = "Reset";

        // The default value of our settings
        public TimeSpan DrillDurationSettingDefault = TimeSpan.FromMinutes(1);
        public TimeSpan WarningSettingDefault = TimeSpan.FromSeconds(10);
        public TimeSpan ResetSettingDefault = TimeSpan.FromSeconds(30);

        public Settings()
        {
            isolatedStore = IsolatedStorageSettings.ApplicationSettings;
        }

        public void save()
        {
            isolatedStore.Save();
        }

        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            // If the key exists, retrieve the value.
            if (isolatedStore.Contains(Key))
            {
                value = (valueType)isolatedStore[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (isolatedStore.Contains(Key))
            {
                // If the value has changed
                if (isolatedStore[Key] != value)
                {
                    // Store the new value
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }
    }
}
