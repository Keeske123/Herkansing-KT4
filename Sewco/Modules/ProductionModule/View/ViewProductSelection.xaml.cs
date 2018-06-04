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
using Sewco.Modules.ControlPanel;

namespace Sewco.Modules.ProductionModule
{
    /// <summary>
    /// Interaction logic for ViewProductSelection.xaml
    /// </summary>
    public partial class ViewProductSelection : UserControl
    {
        public ViewProductSelection()
        {
            InitializeComponent();
            ViewControlPanel.viewModelProduction = new ViewModelProduction();
            DataContext = ViewControlPanel.viewModelProduction;
        }
    }
}
