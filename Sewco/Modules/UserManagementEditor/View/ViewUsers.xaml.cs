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
        LinqToSQLDataContext db;
        string con, saveType, selectedUser;
        object item;
        OpenFileDialog ofd;

        public ViewUsers()
        {
            InitializeComponent();

            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing-KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True";

            ViewModelUsers ViewModelUsers = new ViewModelUsers();
            DataContext = ViewModelUsers;
            db = new LinqToSQLDataContext(con);

            LoadUsers();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadUsers();

            if (tbSearch.Text != "" || tbSearch.Text != null)
            {
                IEnumerable<string> lv = lvUsers.Items.Cast<string>();

                var selectUsers = from q in lv
                                  where q.ToLower().Split(',')[0].Remove(0, 6).Contains(tbSearch.Text.ToLower())
                                  select q;

                string[] results = selectUsers.ToArray();

                lvUsers.Items.Clear();

                foreach (var item in results)
                {
                    lvUsers.Items.Add(String.Format(item.ToString()));
                }
            }
            else
            {
                LoadUsers();
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            LoadRights();

            gbAddEditUsers.Visibility = Visibility.Visible;
            saveType = "New";

            btnNewUser.IsEnabled = false;
            btnEditUser.IsEnabled = false;
            btnDeleteUser.IsEnabled = false;
            btnSaveUser.IsEnabled = true;
            btnCancelUser.IsEnabled = true;
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem != null)
            {
                saveType = "Edit";
                LoadRights();

                gbAddEditUsers.Visibility = Visibility.Visible;
                btnNewUser.IsEnabled = false;
                btnEditUser.IsEnabled = false;
                btnDeleteUser.IsEnabled = false;
                btnSaveUser.IsEnabled = true;
                btnCancelUser.IsEnabled = true;

                var db = new LinqToSQLDataContext(con);

                var selectUser = (from q in db.tbl_Users
                                  where q.Name == lvUsers.SelectedItem.ToString().Split(',')[0].Remove(0, 6)
                                  select q).SingleOrDefault();

                tbOperatortag.Text = selectUser.Operatortag;
                tbName.Text = selectUser.Name;
                cbRights.SelectedItem = selectUser.Rights;
                checkActive.IsChecked = selectUser.Active;
                tbCardcode.Text = selectUser.CardCode;
            }
            else
            {
                System.Windows.MessageBox.Show("Select a User First");
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (lvUsers.SelectedItem != null)
            {
                if (System.Windows.MessageBox.Show("Do you really want to DELETE this User?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    gbAddEditUsers.IsEnabled = true;
                    string selectedUser = lvUsers.SelectedItem.ToString().Split(',')[0].Remove(0, 6);

                    DeleteUser(selectedUser);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Select a User First");
            }
        }

        private void btnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Do you really want to Cancel these changes?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                ResetValues();
        }

        private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        {

            switch (saveType)
            {
                case "New":
                    NewUser();
                    break;

                case "Edit":
                    EditUser(this.selectedUser);
                    break;
            }
        }

        private void btnUserProfiles_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelUserProfiles();
            Framecontent.Source = null;
            Framecontent.Source = new Uri("/Modules/UserManagementEditor/View/ViewUserProfiles.xaml", UriKind.Relative);
        }

        private void NewUser()
        {
            bool result = true;
            try
            {
                var db = new LinqToSQLDataContext(con);

                var selectTag = from q in db.tbl_Users
                                where q.Operatortag == tbOperatortag.Text
                                select q;

                if (selectTag.Count() > 0)
                {
                    System.Windows.MessageBox.Show("This Tag is already Taken");
                    result = false;
                }
                if (result)
                {
                    tbl_User user = new tbl_User
                    {
                        Name = tbName.Text,
                        Rights = cbRights.Text,
                        Active = checkActive.IsChecked.Value,
                        Operatortag = tbOperatortag.Text,
                        CardCode = tbCardcode.Text
                    };

                    db.tbl_Users.InsertOnSubmit(user);

                    db.SubmitChanges();
                    result = true;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Something went wrong While Adding this user");
                result = false;
            }
            finally
            {
                ResetValues();
            }
        }

        private void EditUser(string selectedUser)
        {
            bool result = true;
            try
            {
                var db = new LinqToSQLDataContext(con);

                var selectTag = from q in db.tbl_Users
                                where q.Operatortag == tbOperatortag.Text
                                select q;

                var selectUsername = (from q in db.tbl_Users
                                      where q.Name == selectedUser
                                      select q).SingleOrDefault();

                if (selectTag.Count() > 0)
                {
                    System.Windows.MessageBox.Show("This Tag is already Taken");
                    result = false;
                }

                if (result)
                {
                    selectUsername.Name = tbName.Text;
                    selectUsername.Operatortag = tbOperatortag.Text;
                    selectUsername.Rights = cbRights.Text;
                    selectUsername.Active = checkActive.IsChecked.Value;

                    db.SubmitChanges();
                    ResetValues();
                    result = true;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Something went wrong While Updating this user");
                result = false;
            }
            finally
            {
                LoadUsers();
            }
        }

        private void DeleteUser(string selectedUser)
        {
            try
            {
                var db = new LinqToSQLDataContext(con);

                var selectUser = from q in db.tbl_Users
                                 where q.Name == selectedUser
                                 select q;

                foreach (var item in selectUser)
                {
                    db.tbl_Users.DeleteOnSubmit(item);
                }


                db.SubmitChanges();

                ResetValues();
            }
            catch
            {
                System.Windows.MessageBox.Show("Something went wrong while Deleting User");
            }
        }

        public void ResetValues()
        {
            LoadUsers();
            tbName.Clear();
            cbRights.Items.Clear();
            tbOperatortag.Clear();
            tbCardcode.Clear();
            cbRights.Text = "";
            checkActive.IsChecked = false;

            gbAddEditUsers.Visibility = Visibility.Hidden;

            btnNewUser.IsEnabled = true;
            btnEditUser.IsEnabled = true;
            btnDeleteUser.IsEnabled = true;
            btnSaveUser.IsEnabled = false;
            btnCancelUser.IsEnabled = false;
        }

        private void LoadUsers()
        {
            lvUsers.Items.Clear();
            try
            {
                var tempDB = new LinqToSQLDataContext(con);

                var query =
                (from q in tempDB.tbl_Users
                 select new { Name = q.Name, Rights = q.Rights, Operatortag = q.Operatortag, Active = q.Active, Cardcode = q.CardCode }).ToList();

                foreach (var item in query)
                {
                    lvUsers.Items.Add(String.Format("Name: {0}, Rights: {1}, \nOperatortag: {2}, Active: {3}, \nCardcode: None Assigned\n", item.Name, item.Rights, item.Operatortag, item.Active.ToString()));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Something went wrong while Loading users into the Listview");
            }
        }

        private void LoadRights()
        {
            try
            {
                cbRights.Items.Clear();

                var db = new LinqToSQLDataContext(con);

                var selectProfile = from q in db.tbl_UserProfiles
                                    select q.Userprofile;

                foreach (var item in selectProfile)
                {
                    cbRights.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Something went wrong while loading User Profiles to Combobox");
            }
        }


        //public static BitmapImage BitmapImageFromBytes(byte[] bytes)
        //{
        //    BitmapImage image = null;
        //    MemoryStream stream = null;
        //    try
        //    {
        //        stream = new MemoryStream(bytes);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
        //        image = new BitmapImage();
        //        image.BeginInit();
        //        MemoryStream ms = new MemoryStream();
        //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        image.StreamSource = ms;
        //        image.StreamSource.Seek(0, SeekOrigin.Begin);
        //        image.EndInit();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        stream.Close();
        //        stream.Dispose();
        //    }
        //    return image;
        //}
    }
}