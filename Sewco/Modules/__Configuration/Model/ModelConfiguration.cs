using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Data;
using System.IO;


namespace Sewco.Modules.Configuration
{
    #region Languages_old
    /*public class clLanguages
    {
        public Dictionary<string, string> dicLanguage = new Dictionary<string, string>();
        XDocument docReadLanguage;
        //string sLanguageFile = @"C:\Users\GVE\Google Drive\Projecten_lokaal\Habraken\Sewco\Sewco\Sewco\bin\Debug\Languages\ProfilesEditor_Dutch.xml";

        public string Test;
        public void getLanguage(string sLanguageFile)
        {
            docReadLanguage = XDocument.Load(sLanguageFile);
            dicLanguage = docReadLanguage.Descendants("Definition")
                                                                .ToDictionary(x => x.Attribute("Key").Value,
                                                                x => x.Attribute("Value").Value);
        }

    }*/

    /*public class LanguageConverter : IValueConverter
    {
        public static clLanguages Languages = new clLanguages();
        private bool xLanguageSet = false;

       
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {   
            if(xLanguageSet == false)
            {
                SetLanguage(@"C:\Users\GVE\Google Drive\Projecten_lokaal\Habraken\Sewco\Sewco\Sewco\bin\Debug\Languages\ProfilesEditor_Dutch.xml");
                xLanguageSet = true;
            }
            return Languages.dicLanguage[(string)parameter];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(string sLanguage)
        {
            Languages.getLanguage(sLanguage);
        }
    }*/
    #endregion

    public static class clLanguages
    {
        public static Dictionary<string, string> dicLanguage = new Dictionary<string, string>();
        static XDocument docReadLanguage;

        public static void getLanguage(string sLanguageFile)
        {
            bool xErrorLoadingFile  = false;
            //bool xErrorReadingFile  = false;
            try
            {
                docReadLanguage = XDocument.Load(sLanguageFile);
            } catch
            {
                xErrorLoadingFile   = true;
                // Uitlezen is niet gelukt. Eventuele melding geven?
            }
            try
            {
                if (!xErrorLoadingFile)
                {
                    dicLanguage = docReadLanguage.Descendants("Definition")
                                                                        .ToDictionary(  x => x.Attribute("Key").Value,
                                                                                        x => x.Attribute("Value").Value);
                }
            }
            catch
            {
                //xErrorReadingFile   = true;
                // Uitlezen is niet gelukt. Eventuele melding geven?
            }
        }
        public static string getName(string sName)
        {
            try
            {
                return clLanguages.dicLanguage[sName];
            }
            catch
            {
                return sName;
            }
        }
    }


        
        // Configuration class. Contains a few configuration inits. 
    public static class clConfiguration
    {
        // Verdere instellingen, lees uit de database. 
        private static string sSewcoPath        = Directory.GetCurrentDirectory();
        
        public static string sSelectedLanguage  = sSewcoPath + @"\Languages\";
    }

    public class ModelConfiguration
    {
    
    }

}
