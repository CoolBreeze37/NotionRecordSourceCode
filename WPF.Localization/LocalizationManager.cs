using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Localization
{
    public static class LocalizationManager
    {
        private static ResourceManager _resourceManager = new ResourceManager("WPF.Localization.Resources.Lang", typeof(LocalizationManager).Assembly);

        public static string GetString(string key)
        {
            return _resourceManager.GetString(key);
        }

        public static void ChangeLanguage(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);
        }
    }
}
