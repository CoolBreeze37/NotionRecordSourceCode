using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Localization
{
    public class LocalizedStrings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static LocalizedStrings _instance = new LocalizedStrings();

        public static LocalizedStrings Instance => _instance;

        public string this[string key] => LocalizationManager.GetString(key);

        public void Refresh()
        {
            OnPropertyChanged("");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
