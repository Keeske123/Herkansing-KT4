using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace Sewco.Modules.ControlPanel
{
    class ModelControlPanel
    {
    }

    public static class clLanguages
    {
        public static Dictionary<string, string> dicLanguage = new Dictionary<string, string>();
        static XDocument docReadLanguage;

        public static void getLanguage(string sLanguageFile)
        {
            bool xErrorLoadingFile = false;
            //bool xErrorReadingFile  = false;
            try
            {
                docReadLanguage = XDocument.Load(sLanguageFile);
            }
            catch
            {
                xErrorLoadingFile = true;
                // Uitlezen is niet gelukt. Eventuele melding geven?
            }
            try
            {
                if (!xErrorLoadingFile)
                {
                    dicLanguage = docReadLanguage.Descendants("Definition")
                                                                        .ToDictionary(x => x.Attribute("Key").Value,
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
        private static string sSewcoPath = Directory.GetCurrentDirectory();

        public static string sSelectedLanguage = sSewcoPath + @"\Languages\";
    }

}
