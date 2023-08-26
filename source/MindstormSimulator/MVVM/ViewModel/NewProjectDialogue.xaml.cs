using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MindstormSimulator.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindstormSimulator.MVVM.ViewModel
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewProjectDialogue : Window
    {
        MainWindow MainWindow;
        public NewProjectDialogue(MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.MainWindow = mainWindow;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            FileManager.Project project = new FileManager.Project();
            project.ProjectID = FileManager.ID;
            project.ProjectName = NameBox.Text;
            project.Directory = DirectoryBox.Text;
            project.ProjectType = (FileManager.Project.Type)TypeBox.SelectedIndex;
            project.ProjectLanguage = (FileManager.Project.Language)LanguageBox.SelectedIndex;

            FileManager.Projects.Add(project);
            MainWindow.LoadProject(project);

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Directory_Click(object sender, RoutedEventArgs e)
        {
            DirectoryBox.Text = await FileManager.OpenFolderDialogue(this);
        }
    }
}
