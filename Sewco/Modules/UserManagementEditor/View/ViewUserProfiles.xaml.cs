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
        LinqToSQLDataContext db = new LinqToSQLDataContext();
        string con, saveType;

        public ViewUserProfiles()
        {
            InitializeComponent();

            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C: \Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing - KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True";

            var query =
                    from q in db.tbl_UserProfiles
                    select q.Userprofile;

            foreach (var q in query)
            {
                cbSearch.Items.Add(q);
            }
        }

        private void btnCancelUserProfile_Click(object sender, RoutedEventArgs e)
        {
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
            tbUserProfile.IsEnabled = false;

        }

        private void btnSaveUserProfile_Click(object sender, RoutedEventArgs e)
        {
            switch (saveType)
            {
                case "New":
                    #region Save New Profile

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

                        #region Reset after Save

                        cbSearch.Items.Clear();
                        var refresh =
                            from t in db.tbl_UserProfiles
                            select t.Userprofile;
                        foreach (var item in refresh)
                        {
                            cbSearch.Items.Add(item.ToString());
                        }

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
                        tbUserProfile.IsEnabled = false;

                        #endregion
                    }
                    catch (Exception exc)
                    {
                        System.Windows.MessageBox.Show(exc.ToString());
                    }

                    #endregion
                    break;
                case "Edit":
                    #region Edit

                    var query =
                        (from t in db.tbl_UserProfiles
                         where t.Userprofile == tbUserProfile.Text
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

                    var table =
                        from q in db.tbl_UserProfiles
                        select q.Userprofile;

                    //clear out old selection
                    cbSearch.Items.Clear();

                    //fill combobox with new/updated version of the database
                    foreach (var q in table)
                    {
                        cbSearch.Items.Add(q);
                    }

                    #endregion

                    #region Reset after Save

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
                    tbUserProfile.IsEnabled = false;
                    #endregion
                    break;
            }
        }

        private void btnEditUserProfile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Needs Some Fixing");
            saveType = "Edit";
            btnSaveUserProfile.IsEnabled = true;
            btnCancelUserProfile.IsEnabled = true;
            btnNewUserProfile.IsEnabled = false;
            btnEditUserProfile.IsEnabled = false;
            btnDeleteUserProfile.IsEnabled = false;
            gbAddEditUserProfiles.IsEnabled = true;
        }

        private void btnNewUserProfile_Click(object sender, RoutedEventArgs e)
        {
            saveType = "New";

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

            btnSaveUserProfile.IsEnabled = true;
            btnCancelUserProfile.IsEnabled = true;
            btnNewUserProfile.IsEnabled = false;
            btnEditUserProfile.IsEnabled = false;
            btnDeleteUserProfile.IsEnabled = false;

            gbAddEditUserProfiles.IsEnabled = true;
            tbUserProfile.IsEnabled = true;
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelUsers();
            Framecontent.Source = null;
            Framecontent.Source = new Uri("/Modules/UserManagementEditor/View/ViewUsers.xaml", UriKind.Relative);
        }

        private void btnDeleteUserProfile_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Do You Want to Delete this User?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C: \Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing - KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True");
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbl_UserProfiles WHERE Userprofile='" + cbSearch + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.ToString());
                }                
            }
        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            db = new LinqToSQLDataContext();
            var query =
                from q in db.tbl_UserProfiles
                where q.Userprofile == cbSearch.SelectedItem.ToString()
                select q;

            var table =
                from t in db.tbl_Users
                where t.Rights == cbSearch.SelectedItem.ToString()
                select t;

            dgUsers.ItemsSource = table;

            foreach (var q in query)
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
    }
}