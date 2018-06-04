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

namespace Sewco.Modules.ProductsEditor
{
    /// <summary>
    /// Interaction logic for PageProductsEditor.xaml
    /// </summary>
    public partial class ViewProductsEditor : UserControl
    {
        public ViewProductsEditor()
        {
            InitializeComponent();
            ViewModelProducts ViewModelProducts = new ViewModelProducts();
            DataContext = ViewModelProducts;
        }
    }
}
