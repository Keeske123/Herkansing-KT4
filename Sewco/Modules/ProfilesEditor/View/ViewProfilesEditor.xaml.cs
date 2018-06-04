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
using System.Data;
using System.Threading;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using Sewco.Resources.Helper_classes;

namespace Sewco.Modules.ProfilesEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ViewProfilesEditor : UserControl
    {
        public ViewProfilesEditor()
        {
            InitializeComponent();
            ViewModelProfilesEditor ViewModelProfilesEditor = new ViewModelProfilesEditor();
            DataContext = ViewModelProfilesEditor;
        }
    }
}
