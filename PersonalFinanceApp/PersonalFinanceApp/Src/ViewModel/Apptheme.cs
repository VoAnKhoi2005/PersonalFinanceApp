using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PersonalFinanceApp.Src.ViewModel
{
    class Apptheme
    {
        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = themeUri };
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Theme);
        }
    }
}
