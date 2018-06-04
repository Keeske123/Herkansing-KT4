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

namespace Sewco.Modules.TypesEditor
{
    /// <summary>
    /// Interaction logic for PageProductsEditor.xaml
    /// </summary>
    public partial class ViewTypesEditor : UserControl
    {
        public ViewTypesEditor()
        {
            InitializeComponent();
            ViewModelTypes ViewModelTypes = new ViewModelTypes();
            DataContext = ViewModelTypes;
        }
    }
}
