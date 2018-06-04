using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Data;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Sewco.Modules.Configuration
{

    public class clLanguageConverter : IValueConverter
    {
        private bool xLanguageSet = false;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (xLanguageSet == false)
            {
                SetLanguage();
                xLanguageSet = true;
            }
            return clLanguages.getName((string)parameter);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage()
        {
            clLanguages.getLanguage(clConfiguration.sSelectedLanguage);
        }
    }

    public class ViewModelConfiguration : ObservableObject
    {
        public ObservableCollection<string> obcLanguageOptions { get; set; } = new ObservableCollection<string>();
        public static LinqToSQLDataContext DBDataClass;
        public static bool xValidDatabaseConnection = false;
        public string sLanguage;

        public ViewModelConfiguration()
        {
       
            // Check if sLanguage is filled with right data. If not or translation file does not exist: set default.
            if (sLanguage == null || sLanguage == "" || !File.Exists(clConfiguration.sSelectedLanguage + sLanguage + ".xml"))
            {
                sLanguage = "English";
            }

            //obcLanguageOptions.Add("English");
            //obcLanguageOptions.Add("Dutch");

            if (databaseConnectionIsValid())
            {
                xValidDatabaseConnection = true;
                reloadDatabase();

                // After reload database, otherwise linq is not filled.
                try
                {
                    // Get settings from database
                    var settingsQuery = from x in DBDataClass.Settings select x;

                    foreach (var q in settingsQuery)
                    {
                        sLanguage = q.sLanguage;
                    }
                    clLanguages.getLanguage(clConfiguration.sSelectedLanguage + sLanguage + ".xml");     // Initialize language dictionary
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Query settings failed");
                }
            } else
            {
                clLanguages.getLanguage(clConfiguration.sSelectedLanguage + sLanguage + ".xml");     // Initialize language dictionary
                //MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
            }
            

        }

        public static void reloadDatabase()
        {
            DBDataClass = new LinqToSQLDataContext();   // Create a instance of the database 
        }

        private bool databaseConnectionIsValid()        // Check if there is a valid database connection. 
        {
            using (var checkConnection = new LinqToSQLDataContext())
            {
                try
                {
                    checkConnection.Connection.Open();  // Open connection, if not possible, it will generate an exception.
                    checkConnection.Connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
                    return false;
                }
            }
        }

    }
}
