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
using System.Windows.Shapes;
using Sewco.Modules.ProfilesEditor;
using Sewco.Modules.ProjectsEditor;
using Sewco.Modules.MaterialsEditor;
using Sewco.Modules.ProductsEditor;
using Sewco.Modules.TypesEditor;
using Sewco.Modules.CoversEditor;

namespace Sewco.Modules.Configuration
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class ViewConfiguration : Window
    {
        
        public ViewConfiguration()
        {
            InitializeComponent();
            ViewModelConfiguration ViewModelConfiguration = new ViewModelConfiguration();
        }

        private void PageProjectsEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelProjects();
            FrameContent.Source = new Uri("/Modules/ProjectsEditor/View/ViewProjectsEditor.xaml", UriKind.Relative);
            //FrameContent.DataContext = new ViewModelProjects();
        }

        private void PageProfilesEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext    = new ViewModelProfilesEditor();
            FrameContent.Source = new Uri("/Modules/ProfilesEditor/View/ViewProfilesEditor.xaml", UriKind.Relative);
        }

        private void PageMaterialsEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelMaterials();
            FrameContent.Source = new Uri("/Modules/MaterialsEditor/View/ViewMaterialsEditor.xaml", UriKind.Relative);
        }

        private void PageProductsEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelProducts();
            FrameContent.Source = new Uri("/Modules/ProductsEditor/View/ViewProductsEditor.xaml", UriKind.Relative);
        }

        private void PageTypesEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelTypes();
            FrameContent.Source = new Uri("/Modules/TypesEditor/View/ViewTypesEditor.xaml", UriKind.Relative);
        }
        private void PageCoversEditor_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModelCovers();
            FrameContent.Source = new Uri("/Modules/CoversEditor/View/ViewCoversEditor.xaml", UriKind.Relative);
        }

    }
}
