using Mindstorm_Simulator.MVVM.Model;
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

namespace Mindstorm_Simulator.Theme
{
    /// <summary>
    /// Interaction logic for ProjectButton.xaml
    /// </summary>
    public partial class ProjectButton : UserControl
    {
        public ProjectButton(FileManager.Project project)
        {
            InitializeComponent();

            ProjectName.Text = project.ProjectName;
            ProjectDirectory.Text = project.Directory;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
