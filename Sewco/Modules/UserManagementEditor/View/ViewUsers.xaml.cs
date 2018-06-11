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
        string con, saveType, name;
        object item;
        OpenFileDialog ofd;

        public ViewUsers()
        {
            InitializeComponent();

            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\keese_000\Desktop\AFSTUDEER STAGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\KT4\REDO\Herkansing KT4\Herkansing - KT4\Sewco\UsermanagementDB.mdf;Integrated Security=True";

            ViewModelUsers ViewModelUsers = new ViewModelUsers();
            DataContext = ViewModelUsers;
            db = new LinqToSQLDataContext(con);

            LoadUsers();

            
        }

        

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            gbAddEditUsers.Visibility = Visibility.Visible;
        }

        private void btnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.MessageBox.Show("Do you really want to Cancel these changes?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                ResetValues();
        }

        private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnUserProfiles_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void ResetValues()
        {
            tbName.Clear();
            tbOperatortag.Clear();
            tbCardcode.Clear();
            cbRights.Text = "";
            checkActive.IsChecked = false;
            gbAddEditUsers.Visibility = Visibility.Hidden;
        }

        private void LoadUsers()
        {
            lvUsers.Items.Clear();
            try
            {
                var tempDB = new LinqToSQLDataContext(con);

                var query =
                    from q in tempDB.tbl_Users
                     select q;

                foreach (var item in query)
                {
                    string isActive;

                    if (!item.Active)
                    {
                        isActive = "Not active";
                    }
                    else
                    {
                        isActive = "Active";
                    }

                    lvUsers.Items.Add(String.Format(item.Name.ToUpper() + ", " + item.Operatortag + "\n" + item.Rights + "\n" + isActive));
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Something went wrong while Loading users into the Listview");
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