using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Sewco.Modules.UserManagementEditor
{
    /// <summary>
    /// Interaction logic for ViewUsers.xaml
    /// </summary>
    public partial class ViewUsers : System.Windows.Controls.UserControl
    {
        LinqToSQLDataContext db = new LinqToSQLDataContext();
        string con, saveType, name;
        object item;
        OpenFileDialog ofd;

        public ViewUsers()
        {
            InitializeComponent();

            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C: \Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing - KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True";

            ViewModelUsers ViewModelUsers = new ViewModelUsers();
            DataContext = ViewModelUsers;
            var query =
                (from q in db.tbl_Users
                 select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

            dgUsers.ItemsSource = query;
            var query1 =
                    from q in db.tbl_UserProfiles
                    select q.Userprofile;

            foreach (var q in query1)
            {
                cbRights.Items.Add(q);
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                gbAddEditUsers.IsEnabled = true;
                btnSaveUser.IsEnabled = true;
                btnCancelUser.IsEnabled = true;
                btnNewUser.IsEnabled = false;
                btnEditUser.IsEnabled = false;
                btnDeleteUser.IsEnabled = false;
                saveType = "New";
            }
            catch (Exception) when (cbRights.HasItems == false)
            {
                System.Windows.MessageBox.Show("Something went wrong");
            }

            if (!cbRights.HasItems)
            {
                var rights =
                    from r in db.tbl_UserProfiles
                    select r.Userprofile;

                foreach (var r in rights)
                {
                    cbRights.Items.Add(r);
                }
            }
        }

        private void btnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            gbAddEditUsers.IsEnabled = false;
            btnSaveUser.IsEnabled = false;
            btnCancelUser.IsEnabled = false;
            btnNewUser.IsEnabled = true;
            btnEditUser.IsEnabled = true;
            btnDeleteUser.IsEnabled = true;

            tbMoreName.Text = "";
            tbMoreTag.Text = "";
            tbMoreRights.Text = "";
            tbMoreCardCode.Text = "";
            checkMoreActive.IsChecked = false;

            checkProductions.IsChecked = false;
            checkMachineConfig.IsChecked = false;
            checkProductDef.IsChecked = false;
            checkUsers.IsChecked = false;
            checkMaintenance.IsChecked = false;
            checkReprint.IsChecked = false;
            checkReset.IsChecked = false;
            checkFind.IsChecked = false;
            checkDesktop.IsChecked = false;
            checkMaterials.IsChecked = false;
            checkProductProfile.IsChecked = false;
            checkLabelEditor.IsChecked = false;
            checkBobbinMonitor.IsChecked = false;
            checkBobbinTracing.IsChecked = false;

            tbName.Text = "";
            tbOperatortag.Text = "";
            tbCardcode.Text = "";
            cbRights.Items.Clear();
            checkActive.IsChecked = false;

        }

        private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            switch (saveType)
            {
                case "New":
                    #region Save New User
                    //System.Windows.Forms.MessageBox.Show("");
                    //byte[] newImageArray = File.ReadAllBytes(imgUser.Source.ToString().Remove(0, 8));
                    var bmp = imgUser.Source as BitmapImage;

                    int height = bmp.PixelHeight;
                    int width = bmp.PixelWidth;
                    int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

                    byte[] bits = new byte[height * stride];
                    bmp.CopyPixels(bits, stride, 0);
                    try
                    {
                        using (LinqToSQLDataContext db = new LinqToSQLDataContext(con))
                        {
                            tbl_User tbl = new tbl_User
                            {
                                Name = tbName.Text,
                                Rights = cbRights.Text,
                                Operatortag = tbOperatortag.Text,
                                Active = checkActive.IsChecked.Value,
                                CardCode = tbCardcode.Text,
                                img = bits
                            };
                            db.tbl_Users.InsertOnSubmit(tbl);
                            db.SubmitChanges();
                        }
                    }
                    catch (Exception exc)
                    {
                        System.Windows.MessageBox.Show(exc.ToString(), "Something went wrong");
                    }
                    finally
                    {
                        ResetAfterSave();
                    }

                    #endregion

                    break;

                case "Edit":
                    #region save Edit User

                    byte[] editImageArray = File.ReadAllBytes(imgUser.Source.ToString());

                    var table =
                        (from t in db.tbl_Users
                         where t.Name == tbName.Text
                         select t).FirstOrDefault();

                    table.Name = tbName.Text;
                    table.Operatortag = tbOperatortag.Text;
                    table.Rights = cbRights.Text;
                    table.Active = checkActive.IsChecked.Value;
                    table.CardCode = tbCardcode.Text;
                    table.img = editImageArray;

                    db.SubmitChanges();

                    var refresh =
                        (from q in db.tbl_Users
                         select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                    dgUsers.ItemsSource = refresh;
                    #endregion

                    ResetAfterSave();
                    break;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "" || tbSearch.Text != null)
            {
                var query = (from q in db.tbl_Users
                             select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                dgUsers.ItemsSource = null;
                dgUsers.ItemsSource = query;
            }
            else
            {
                var query =
                (from q in db.tbl_Users
                 select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                dgUsers.ItemsSource = query;
            }

        }

        private void btnUserProfiles_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelUserProfiles();
            Framecontent.Source = new Uri("/Modules/UserManagementEditor/View/ViewUserProfiles.xaml", UriKind.Relative);
        }

        private void dgUsers_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Random rdnm = new Random();
                switch (rdnm.Next(0, 2))
                { 
                    case 0:
                        imgMoreUser.Source = new BitmapImage(new Uri(@"C:\Users\keese_000\Desktop\Sewco\Sewco2\Sewco\Sewco\Resources\Images\Profile Test Images\Kristin_Profile2.jpg"));
                        break;
                    case 1:
                        imgMoreUser.Source = new BitmapImage(new Uri(@"C:\Users\keese_000\Desktop\Sewco\Sewco2\Sewco\Sewco\Resources\Images\Profile Test Images\untitled-20.jpg"));
                        break;
                }

                
                item = dgUsers.SelectedItem;
                this.name = (dgUsers.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

                var query =
                    from t in db.tbl_Users
                    where t.Name == name
                    select t;


                foreach (var q in query)
                {
                    tbMoreName.Text = q.Name;
                    tbMoreRights.Text = q.Rights;
                    tbMoreTag.Text = q.Operatortag;
                    checkMoreActive.IsChecked = q.Active;
                    tbMoreCardCode.Text = q.CardCode;

                    //byte[] loadImageArray = File.ReadAllBytes(q.img.ToString());
                    //imgUser.Source = BitmapImageFromBytes(loadImageArray);
                }

                var table =
                    from q in db.tbl_UserProfiles
                    where q.Userprofile == tbMoreRights.Text
                    select q;
            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show("NOPE");
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Do You Want to Delete this User?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    //var query =
                    //from t in db.tbl_Users
                    //where t.Name == name
                    //select t;

                    //foreach (var item in query)
                    //{
                    //    db.tbl_Users.DeleteOnSubmit(item);
                    //}
                    //db.SubmitChanges();

                    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C: \Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing - KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True");
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbl_Users WHERE Name='" + name + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    var table =
                        (from q in db.tbl_Users
                         select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                    dgUsers.ItemsSource = table;
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show(exc.ToString());
                }

                #region comments
                //using (LinqToSQLDataContext db = new LinqToSQLDataContext())
                //{

                //    StringWriter sw = new StringWriter();
                //    db.Log = sw;

                //    var query =
                //        from t in db.tbl_Users
                //        where t.Name == name
                //        select t;

                //    var i = query.First();
                //    db.tbl_Users.DeleteOnSubmit(i);

                //    string sql = sw.ToString();

                //    System.Windows.MessageBox.Show(sql);

                //    db.SubmitChanges();

                //    var refresh =
                //                (from q in db.tbl_Users
                //                 select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                //    dgUsers.ItemsSource = refresh;

                //    //dus nu zou tie moeten verwijdert zijn


                //}

                #endregion
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog print = new System.Windows.Controls.PrintDialog();
            print.ShowDialog();
            print.PrintVisual(this, "Entire Screen");
            print.PrintVisual(gridMoreDetails, "More Grid");
        }

        private void imgUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("This One");
            ofd = new OpenFileDialog();
            ofd.Title = "Select a Picture";
            ofd.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
    "Portable Network Graphic (*.png)|*.png";

            try
            {

                ofd.ShowDialog();
                imgUser.Source = new BitmapImage(new Uri(ofd.FileName));
                System.Windows.Forms.MessageBox.Show(imgUser.Source.ToString());

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text != "" || tbSearch.Text != null)
            {
                
                List<tbl_User> t = db.tbl_Users.Where(x => x.Name.Contains(tbSearch.Text)).ToList();
                dgUsers.ItemsSource = t;
            }
            else
            {
                var query =
                (from q in db.tbl_Users
                 select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                dgUsers.ItemsSource = query;
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            //enabling required fields for Editing
            try
            {
                tbName.Text = tbMoreName.Text;
                cbRights.Text = tbMoreRights.Text;
                tbOperatortag.Text = tbMoreTag.Text;
                checkActive.IsChecked = checkMoreActive.IsChecked.Value;
                tbCardcode.Text = tbMoreCardCode.Text;

                gbAddEditUsers.IsEnabled = true;
                btnSaveUser.IsEnabled = true;
                btnCancelUser.IsEnabled = true;
                btnNewUser.IsEnabled = false;
                btnEditUser.IsEnabled = false;
                btnDeleteUser.IsEnabled = false;

                saveType = "Edit";
            }
            catch
            {
                System.Windows.MessageBox.Show("Some Field is no Value", "Error");
            }
        }

        public void ResetAfterSave()
        {
            gbAddEditUsers.IsEnabled = false;
            btnSaveUser.IsEnabled = false;
            btnCancelUser.IsEnabled = false;
            btnNewUser.IsEnabled = true;
            btnEditUser.IsEnabled = true;
            btnDeleteUser.IsEnabled = true;

            tbName.Text = "";
            tbOperatortag.Text = "";
            tbCardcode.Text = "";
            cbRights.Items.Clear();
            checkActive.IsChecked = false;
            saveType = "";
        }

        public static BitmapImage BitmapImageFromBytes(byte[] bytes)
        {
            BitmapImage image = null;
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                image = new BitmapImage();
                image.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.StreamSource.Seek(0, SeekOrigin.Begin);
                image.EndInit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
            return image;
        }
    }
}