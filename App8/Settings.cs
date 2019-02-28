using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace App8
{
 public    class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "andi";
        private static readonly string SettingsDefault = string.Empty;

        private const string SettingsKeyPass = "andi1";
        private static readonly string SettingsDefaultPass = string.Empty;
        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }
        public static string GeneralSettingsPass
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKeyPass, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKeyPass, value);
            }
        }

    }
}