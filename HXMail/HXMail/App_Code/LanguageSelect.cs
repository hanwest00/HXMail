using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.IO;

namespace HXMail.App_Code
{
    public class LanguageSelect
    {


        public static CultureInfo _CurrentCulture;

        public static void ChangeLanguage(Languages lang)
        {
            _CurrentCulture = new CultureInfo("zh-Cn");
            if (lang == Languages.en)
                _CurrentCulture = new CultureInfo("en-Us");
            Thread.CurrentThread.CurrentCulture = _CurrentCulture;
           //ResourceManager rm = new ResourceManager("HXMail.Resource.Lang", Assembly.GetExecutingAssembly());
        }

        public static string getResc(string name)
        {
            if (_CurrentCulture == null)
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-Cn");
            ResourceManager rm = new ResourceManager("HXMail.Resource.Lang", Assembly.GetExecutingAssembly());
            return rm.GetString(name, Thread.CurrentThread.CurrentCulture);
        }

        public enum Languages
        { 
           cn,
           en
        }
    }
}
