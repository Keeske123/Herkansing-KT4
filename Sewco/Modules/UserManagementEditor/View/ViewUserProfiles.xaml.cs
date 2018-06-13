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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sewco.Modules.UserManagementEditor
{
    /// <summary>
    /// Interaction logic for ViewUserProfiles.xaml
    /// </summary>
    public partial class ViewUserProfiles : System.Windows.Controls.UserControl
    {
        LinqToSQLDataContext db;
        private string selectedProfile;
        string con, saveType;
        private bool isDeleting;

        public ViewUserProfiles()
        {
            InitializeComponent();

            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing-KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True";

            db = new LinqToSQLDataContext(con);
            LoadProfiles();
        }

        private void btnCancelUserProfile_Click(object sender, RoutedEventArgs e)
        {
            ResetValues();
        }

        private void btnSaveUserProfile_Click(object sender, RoutedEventArgs e)
        {
            switch (saveType)
            {
                case "New":
                    SaveNewProfile();

                    break;
                case "Edit":
                    SaveEditProfile();
                    break;
            }
        }

        private void btnEditUserProfile_Click(object sender, RoutedEventArgs e)
        {
            if (cbSearchProfiles.SelectedItem != null || cbSearchProfiles.SelectedItem != null)
            {
                saveType = "Edit";
                selectedProfile = cbSearchProfiles.Text;

                btnSaveUserProfile.IsEnabled = true;
                btnCancelUserProfile.IsEnabled = true;
                btnNewUserProfile.IsEnabled = false;
                btnEditUserProfile.IsEnabled = false;
                btnDeleteUserProfile.IsEnabled = false;

                gbSearch.IsEnabled = false;

                gbAddEditUserProfiles.IsEnabled = true;
            }
        }

        private void btnNewUserProfile_Click(object sender, RoutedEventArgs e)
        {
            saveType = "New";

            btnSaveUserProfile.IsEnabled = true;
            btnCancelUserProfile.IsEnabled = true;
            btnNewUserProfile.IsEnabled = false;
            btnEditUserProfile.IsEnabled = false;
            btnDeleteUserProfile.IsEnabled = false;

            gbSearch.IsEnabled = false;
            
            gbAddEditUserProfiles.IsEnabled = true;
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelUsers();
            Framecontent.Source = null;
            Framecontent.Source = new Uri("/Modules/UserManagementEditor/View/ViewUsers.xaml", UriKind.Relative);
        }

        private void btnDeleteUserProfile_Click(object sender, RoutedEventArgs e)
        {

            if (cbSearchProfiles.SelectedItem != null || cbSearchProfiles.SelectedItem != null)
                if (System.Windows.MessageBox.Show("Do you really want to DELETE this Userprofile?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    DeleteProfile(cbSearchProfiles.SelectedItem.ToString());
                else
                    System.Windows.MessageBox.Show("Select a User First");

        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!isDeleting) //failsafe
            {
                db = new LinqToSQLDataContext(con);

                var selectProfile =
                    from q in db.tbl_UserProfiles
                    where q.Userprofile == cbSearchProfiles.SelectedItem.ToString()
                    select q;

                var selectUsers =
                    (from q in db.tbl_Users
                     where q.Rights == cbSearchProfiles.SelectedItem.ToString()
                     select new { Name = q.Name, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                lvUsers.Items.Clear();

                foreach (var item in selectUsers)
                {
                    lvUsers.Items.Add(String.Format("Name: {0}, Operatortag: {1}, \nActive: {2}, Cardcode: None Assigned\n", item.Name, item.Operatortag, item.Active.ToString()));
                }

                foreach (var q in selectProfile)
                {
                    tbUserProfile.Text = q.Userprofile;

                    checkProductions.IsChecked = q.Productions;
                    checkMaintenance.IsChecked = q.Maintenance;
                    checkProductDef.IsChecked = q.Products;
                    checkUsers.IsChecked = q.Users;
                    checkMachineConfig.IsChecked = q.MachineConfig;
                    checkReprint.IsChecked = q.Reprint;
                    checkReset.IsChecked = q.Reset;
                    checkFind.IsChecked = q.Find;
                    checkDesktop.IsChecked = q.Desktop;
                    checkMaterials.IsChecked = q.Materials;
                    checkProductProfile.IsChecked = q.ProductProfile;
                    checkLabelEditor.IsChecked = q.LabelEditor;
                    checkBobbinMonitor.IsChecked = q.BobbinMonitor;
                    checkBobbinTracing.IsChecked = q.BobbinTracer;
                }
            }
            else
            {
                isDeleting = false; //Done deleting the Profile
            }
        }


        private void SaveEditProfile()
        {
            var query = (from t in db.tbl_UserProfiles
                         where t.Userprofile == cbSearchProfiles.Text
                         select t).FirstOrDefault();

            query.Userprofile = tbUserProfile.Text;
            query.Productions = checkProductions.IsChecked.Value;
            query.Maintenance = checkMaintenance.IsChecked.Value;
            query.Products = checkProductDef.IsChecked.Value;
            query.Users = checkUsers.IsChecked.Value;
            query.MachineConfig = checkMachineConfig.IsChecked.Value;
            query.Reprint = checkReprint.IsChecked.Value;
            query.Reset = checkReset.IsChecked.Value;
            query.Find = checkFind.IsChecked.Value;
            query.Desktop = checkDesktop.IsChecked.Value;
            query.Materials = checkMaterials.IsChecked.Value;
            query.ProductProfile = checkProductProfile.IsChecked.Value;
            query.LabelEditor = checkLabelEditor.IsChecked.Value;
            query.BobbinMonitor = checkBobbinMonitor.IsChecked.Value;
            query.BobbinTracer = checkBobbinTracing.IsChecked.Value;

            db.SubmitChanges();

            ResetValues();
        }

        private void SaveNewProfile()
        {
            try
            {
                using (LinqToSQLDataContext db = new LinqToSQLDataContext(con))
                {
                    tbl_UserProfile tbl = new tbl_UserProfile
                    {
                        Userprofile = tbUserProfile.Text,
                        Productions = checkProductions.IsChecked.Value,
                        Maintenance = checkMaintenance.IsChecked.Value,
                        Products = checkProductDef.IsChecked.Value,
                        Users = checkUsers.IsChecked.Value,
                        MachineConfig = checkMachineConfig.IsChecked.Value,
                        Reprint = checkReprint.IsChecked.Value,
                        Reset = checkReset.IsChecked.Value,
                        Find = checkFind.IsChecked.Value,
                        Desktop = checkDesktop.IsChecked.Value,
                        Materials = checkMaterials.IsChecked.Value,
                        ProductProfile = checkProductProfile.IsChecked.Value,
                        LabelEditor = checkLabelEditor.IsChecked.Value,
                        BobbinMonitor = checkBobbinMonitor.IsChecked.Value,
                        BobbinTracer = checkBobbinTracing.IsChecked.Value
                    };

                    db.tbl_UserProfiles.InsertOnSubmit(tbl);
                    db.SubmitChanges();
                }

                ResetValues();
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show("Something went wrong While Adding this user");
            }
        }

        private void DeleteProfile(string selectedProfile)
        {
            try
            {
                var db = new LinqToSQLDataContext(con);

                var selectProfile = from sp in db.tbl_UserProfiles
                                    where sp.Userprofile == selectedProfile
                                    select sp;

                foreach (var item in selectProfile)
                {
                    db.tbl_UserProfiles.DeleteOnSubmit(item);
                }

                db.SubmitChanges();
                isDeleting = true;
                ResetValues();
            }
            catch
            {
                System.Windows.MessageBox.Show("Something went wrong while Deleting the Profile");
            }
        }


        private void ResetValues()
        {
            LoadProfiles();

            btnSaveUserProfile.IsEnabled = false;
            btnCancelUserProfile.IsEnabled = false;
            btnNewUserProfile.IsEnabled = true;
            btnEditUserProfile.IsEnabled = true;
            btnDeleteUserProfile.IsEnabled = true;

            tbUserProfile.Text = "";

            checkProductions.IsChecked = false;
            checkMaintenance.IsChecked = false;
            checkProductDef.IsChecked = false;
            checkUsers.IsChecked = false;
            checkMachineConfig.IsChecked = false;
            checkReprint.IsChecked = false;
            checkReset.IsChecked = false;
            checkFind.IsChecked = false;
            checkDesktop.IsChecked = false;
            checkMaterials.IsChecked = false;
            checkProductProfile.IsChecked = false;
            checkLabelEditor.IsChecked = false;
            checkBobbinMonitor.IsChecked = false;
            checkBobbinTracing.IsChecked = false;

            gbAddEditUserProfiles.IsEnabled = false;
            gbSearch.IsEnabled = true;
            tbUserProfile.IsEnabled = false;

            selectedProfile = "";
        }

        private void LoadProfiles()
        {
            cbSearchProfiles.Text = "";
            cbSearchProfiles.Items.Clear();

            var query =
                   from q in db.tbl_UserProfiles
                   select q.Userprofile;

            foreach (var q in query)
            {
                cbSearchProfiles.Items.Add(q);
            }
        }
    }
}