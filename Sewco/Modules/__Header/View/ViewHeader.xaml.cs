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

namespace Sewco.Modules.Header
{
    /// <summary>
    /// Interaction logic for ViewHeader.xaml
    /// </summary>
    public partial class ViewHeader : UserControl
    {
        //public ViewModelHeader ViewModelHeader = new ViewModelHeader();
        public ViewHeader()
        {
            InitializeComponent();
            //DataContext = ViewModelHeader;
        }
    }
}
