using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Sewco.Modules.Header;
using Sewco.Modules.ProductionModule;
using Sewco.Modules.CoversEditor;
using Sewco.Modules.MaterialsEditor;
using Sewco.Modules.ProductsEditor;
using Sewco.Modules.ProjectsEditor;
using Sewco.Modules.TypesEditor;
using Sewco.Modules.ProfilesEditor;
using Sewco.Resources;
using Sewco.Modules.UserManagementEditor;

namespace Sewco.Modules.ControlPanel
{
    /// <summary>
    /// Interaction logic for ViewControlPanel.xaml
    /// </summary>
    /// 
    public partial class ViewControlPanel : Window
    {
        public static ViewModelControlPanel ViewModelControlPanel   = new ViewModelControlPanel();
        public static ViewModelProduction viewModelProduction       = new ViewModelProduction();

        //ViewConfiguration ucViewConfiguration       = new ViewConfiguration();
        ViewHomeScreen ucViewHomeScreen = new ViewHomeScreen();
        ViewHeader ucHeader = new ViewHeader();
        ViewCoversEditor ucCoversEditor = new ViewCoversEditor();
        ViewMaterialsEditor ucMaterialsEditor = new ViewMaterialsEditor();
        ViewProfilesEditor ucProfilesEditor = new ViewProfilesEditor();
        ViewProductsEditor ucProductsEditor = new ViewProductsEditor();
        ViewProjectsEditor ucProjectsEditor = new ViewProjectsEditor();
        ViewTypesEditor ucTypesEditor = new ViewTypesEditor();
        ViewUsers ucUserEditor = new ViewUsers();

        UserProfiles userprofiles = new UserProfiles();

        System.Windows.Threading.DispatcherTimer dispatcherTimer    = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer2   = new System.Windows.Threading.DispatcherTimer();
        public static bool xDatabasePopup;
        public static bool xDatabasePopup2;
      
        public ViewControlPanel()
        {
            InitializeComponent();

            DataContext = ViewModelControlPanel;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100ms
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer_Tick2);
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100ms
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            xDatabasePopup = false;
            ViewModelControlPanel.xShowDatabaseMenu = (xDatabasePopup || xDatabasePopup2);
        }
        private void dispatcherTimer_Tick2(object sender, EventArgs e)
        {
            dispatcherTimer2.Stop();
            xDatabasePopup2 = false;
            ViewModelControlPanel.xShowDatabaseMenu = (xDatabasePopup || xDatabasePopup2);
        }
        private void DataBaseEditor_enter(object sender, RoutedEventArgs e)
        {
            xDatabasePopup = true;
            ViewModelControlPanel.xShowDatabaseMenu = (xDatabasePopup || xDatabasePopup2);
        }
        private void DataBaseEditor_leave(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
        }
        private void DataBasePopup_enter(object sender, RoutedEventArgs e)
        {
            xDatabasePopup2 = true;
            ViewModelControlPanel.xShowDatabaseMenu = (xDatabasePopup || xDatabasePopup2);
        }
        private void DataBasePopup_leave(object sender, RoutedEventArgs e)
        {
            dispatcherTimer2.Start();
        }
        private void deletePopup()
        {
            xDatabasePopup = false;
            xDatabasePopup2 = false;
            ViewModelControlPanel.xShowDatabaseMenu = (xDatabasePopup || xDatabasePopup2);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new LinqToSQLDataContext();
                tbl_User t = null;

                t = db.tbl_Users.Single(p => p.Operatortag == tbUsername.Text && p.Active == true);

                if (t != null)
                {
                    var query =
                        from q in db.tbl_UserProfiles
                        where q.Userprofile == t.Rights
                        select q.Userprofile;

                    foreach (var q in query)
                    {
                        userprofiles.SetRights(q.ToString());
                    }                    

                    #region rights

                    if (!userprofiles.xMachineConfig)
                    {
                        btnConfiguration.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnConfiguration.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xProductions)
                    {
                        btnProductions.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnProductions.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xUsers)
                    {
                        btnUserManagement.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnUserManagement.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xFind)
                    {
                        btnStatistics.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnStatistics.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xProducts)
                    {
                        btnDatabase.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnDatabase.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xLabelEditor)
                    {
                        btnLabelEditor.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnLabelEditor.Visibility = Visibility.Visible;
                    }

                    if (!userprofiles.xDesktop)
                    {
                        btnDesktop.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btnDesktop.Visibility = Visibility.Visible;
                    }

                    #endregion

                }
                else
                {
                    btnConfiguration.Visibility = Visibility.Collapsed;
                    btnDatabase.Visibility = Visibility.Collapsed;
                    btnLabelEditor.Visibility = Visibility.Collapsed;
                    btnProductions.Visibility = Visibility.Collapsed;
                    btnStatistics.Visibility = Visibility.Collapsed;
                    btnUserManagement.Visibility = Visibility.Collapsed;
                    btnDesktop.Visibility = Visibility.Collapsed;
                    MessageBox.Show("User isn't active");
                }
            }
            catch
            {
                btnConfiguration.Visibility = Visibility.Collapsed;
                btnDatabase.Visibility = Visibility.Collapsed;
                btnLabelEditor.Visibility = Visibility.Collapsed;
                btnProductions.Visibility = Visibility.Collapsed;
                btnStatistics.Visibility = Visibility.Collapsed;
                btnUserManagement.Visibility = Visibility.Collapsed;
                btnDesktop.Visibility = Visibility.Collapsed;

                MessageBox.Show("No User Detected or User isn't active");
            }

        }
        private void tbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
