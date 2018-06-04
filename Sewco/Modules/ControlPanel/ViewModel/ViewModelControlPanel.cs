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
using Sewco.Resources.Helper_classes;
using System.Windows;

using Sewco.Modules.Header;
using Sewco.Modules.ProductionModule;
using Sewco.Modules.CoversEditor;
using Sewco.Modules.MaterialsEditor;
using Sewco.Modules.ProductsEditor;
using Sewco.Modules.ProjectsEditor;
using Sewco.Modules.TypesEditor;
using Sewco.Modules.ProfilesEditor;
using Sewco.Modules.UserManagementEditor;

namespace Sewco.Modules.ControlPanel
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

    public class ViewModelControlPanel : ObservableObject
    {
        public ViewModelControlPanel()
        {
            header = new clHeader();
            header.sModuleName = clLanguages.getName("__HomeScreen");

            xShowMainMenuButtons = true;

            TestCommand                         = new RelayCommand(
                                                    param => doSomething(),
                                                    param => true);
            ShowHomeScreenCommand               = new RelayCommand(
                                                   param => showScreen("HomeScreen"),
                                                   param => true);
            ShowProductSelectionScreenCommand   = new RelayCommand(
                                                   param => showScreen("ProductSelection"),
                                                   param => true);
            showProductionScreenCommand         = new RelayCommand(
                                                   param => showScreen("Production"),
                                                   param => xShowProductionButton);
            ShowUserManagementScreenCommand     = new RelayCommand(
                                                   param => showScreen("UserManagement"),
                                                   param => true);
            ShowStatisticsScreenCommand         = new RelayCommand(
                                                   param => showScreen("Statistics"),
                                                   param => true);
            ShowConfigurationScreenCommand      = new RelayCommand(
                                                   param => showScreen("Configuration"),
                                                   param => true);
            ShowLabelScreenCommand              = new RelayCommand(
                                                   param => showScreen("Label"),
                                                   param => true);
            ShowDBProjectsScreenCommand         = new RelayCommand(
                                                   param => showScreen("DBProjects"),
                                                   param => true);
            ShowDBProductsScreenCommand         = new RelayCommand(
                                                   param => showScreen("DBProducts"),
                                                   param => true);
            ShowDBProfilesScreenCommand         = new RelayCommand(
                                                   param => showScreen("DBProfiles"),
                                                   param => true);
            ShowDBMaterialsScreenCommand        = new RelayCommand(
                                                   param => showScreen("DBMaterials"),
                                                   param => true);
            ShowDBTypesScreenCommand            = new RelayCommand(
                                                   param => showScreen("DBTypes"),
                                                   param => true);
            ShowDBCoversScreenCommand           = new RelayCommand(
                                                   param => showScreen("DBCovers"),
                                                   param => true);

            ShutdownApplication                 = new RelayCommand(
                                                   param => App.Current.Shutdown(),
                                                   param => true);

            

            // Check if sLanguage is filled with right data. If not or translation file does not exist: set default.
            if (sLanguage == null || sLanguage == "" || !File.Exists(clConfiguration.sSelectedLanguage + sLanguage + ".xml"))
            {
                sLanguage = "English";
            }

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
            }
            else
            {
                clLanguages.getLanguage(clConfiguration.sSelectedLanguage + sLanguage + ".xml");     // Initialize language dictionary
            }
        }


        private bool _xShowMainMenuButtons;
        private bool _xShowDatabaseMenu;
        private bool _xShowProductionButton;

        public static  UIElement _ucCurrentPage { get; set; } = new UIElement();

        public ObservableCollection<string> obcLanguageOptions { get; set; } = new ObservableCollection<string>();
        public static LinqToSQLDataContext DBDataClass;
        public static bool xValidDatabaseConnection = false;
        public string sLanguage;
        internal bool xShowMenu;

        public RelayCommand TestCommand { get; set; }
        public RelayCommand ShowHomeScreenCommand { get; set; }
        public RelayCommand ShowProductSelectionScreenCommand { get; set; }
        public RelayCommand showProductionScreenCommand { get; set; }
        public RelayCommand ShowUserManagementScreenCommand { get; set; }
        public RelayCommand ShowStatisticsScreenCommand { get; set; }
        public RelayCommand ShowConfigurationScreenCommand { get; set; }
        public RelayCommand ShowLabelScreenCommand { get; set; }
        public RelayCommand ShowDBProjectsScreenCommand { get; set; }
        public RelayCommand ShowDBProductsScreenCommand { get; set; }
        public RelayCommand ShowDBProfilesScreenCommand { get; set; }
        public RelayCommand ShowDBMaterialsScreenCommand { get; set; }
        public RelayCommand ShowDBTypesScreenCommand { get; set; }
        public RelayCommand ShowDBCoversScreenCommand { get; set; }

        public RelayCommand ShutdownApplication { get; set; }
        
        public clHeader header { get; set; }
        public UIElement ucCurrentPage
        {
            get
            {
                return _ucCurrentPage;
            }
            set
            {
                _ucCurrentPage = value;
                OnPropertyChanged("ucCurrentPage");
            }
        }





        public bool xShowMainMenuButtons
        {
            get
            {
                return _xShowMainMenuButtons;
            }
            set
            {
                _xShowMainMenuButtons = value;
                OnPropertyChanged("xShowMainMenuButtons");
            }
        }
        public bool xShowDatabaseMenu
        {
            get
            {
                return _xShowDatabaseMenu;
            }
            set
            {
                _xShowDatabaseMenu = value;
                OnPropertyChanged("xShowDatabaseMenu");
            }
        }
        public bool xShowProductionButton
        {
            get
            {
                return _xShowProductionButton;
            }
            set
            {
                _xShowProductionButton = value;
                OnPropertyChanged("xShowProductionButton");
            }
        }


        private void doSomething()
        {
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
                    System.Windows.MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
                    return false;
                }
            }
        }
        public void showScreen(string _argsScreen)
        {
            xShowMainMenuButtons    = false;
            xShowDatabaseMenu       = false;
            xShowProductionButton   = false;
            switch (_argsScreen)
            {
                case "HomeScreen":
                    ucCurrentPage = new ViewHomeScreen();
                    xShowMainMenuButtons = true;
                    header.sModuleName = clLanguages.getName("__HomeScreen");
                    break;
                case "ProductSelection":
                    ucCurrentPage = new ViewProductSelection();
                    header.sModuleName = clLanguages.getName("__ProductSelection");
                    xShowProductionButton = true;
                    break;
                case "Production":
                    ucCurrentPage = new ViewProduction();
                    header.sModuleName = clLanguages.getName("__Production");
                    break;
                case "UserManagement":
                    ucCurrentPage = new ViewUsers();
                    header.sModuleName = clLanguages.getName("__Usermanagement");
                    break;
                case "Statistics":
//                    ucCurrentPage = new ViewStatistics();
                    break;
                case "Configuration":
//                    ucCurrentPage = new ViewConfiguration();
                    break;
                case "Label":
//                    ucCurrentPage = new ViewLabel();
                    break;
                case "DBProjects":
                    ucCurrentPage = new ViewProjectsEditor();
                    header.sModuleName = clLanguages.getName("__ProjectsEditor");
                    break;
                case "DBProducts":
                    ucCurrentPage = new ViewProductsEditor();
                    header.sModuleName = clLanguages.getName("_ProductsEditor");
                    break;
                case "DBProfiles":
                    ucCurrentPage = new ViewProfilesEditor();
                    header.sModuleName = clLanguages.getName("__ProfilesEditor");
                    break;
                case "DBMaterials":
                    ucCurrentPage = new ViewMaterialsEditor();
                    header.sModuleName = clLanguages.getName("__MaterialsEditor");
                    break;
                case "DBTypes":
                    ucCurrentPage = new ViewTypesEditor();
                    header.sModuleName = clLanguages.getName("__TypesEditor");
                    break;
                case "DBCovers":
                    ucCurrentPage = new ViewCoversEditor();
                    header.sModuleName = clLanguages.getName("__CoversEditor");
                    break;

            }
        }


        public static void reloadDatabase()
        {
            DBDataClass = new LinqToSQLDataContext();   // Create a instance of the database 
        }
    }
}
