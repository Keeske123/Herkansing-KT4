using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sewco.Modules.ControlPanel;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace Sewco.Modules.ProductionModule
{
    /// <summary>
    /// Interaction logic for PageProductsEditor.xaml
    /// </summary>
    public partial class ViewProduction : UserControl
    {
        public ViewProduction()
        {
            InitializeComponent();
            DataContext = ViewControlPanel.viewModelProduction;       
        }
    }
}
