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
using System.Windows.Shapes;

namespace Mindstorm_Simulator.MVVM.View
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        FileManager.Project project;
        MainWindow _mainWindow;
        public NewProjectWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Directory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            project = new FileManager.Project(
                DirectoryBox.Text,
                NameBox.Text,
                null,
                (FileManager.Project.Type)TypeBox.SelectedIndex,
                (FileManager.Project.Language)LanguageBox.SelectedIndex);

            FileManager.KnownProjects.Add(project);
            _mainWindow.SetandOpenProject(project);

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
