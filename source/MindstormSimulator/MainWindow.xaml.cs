using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MindstormSimulator.MVVM.Model;
using MindstormSimulator.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindstormSimulator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        EditorPage _EditorPage;
        public MainWindow()
        {
            this.InitializeComponent();

            Setup();
        }

        // control of the project is passed to EditorPage from here
        public void LoadProject(FileManager.Project project)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            _EditorPage = new EditorPage(project);
            MainFrame.Content = _EditorPage;
        }

        private async void Setup()
        {
            if(await FileManager.LoadProjectFiles())
            {
                foreach(var project in FileManager.Projects)
                {
                    ListButton listButton = new ListButton(project, this);
                    RecentStack.Children.Add(listButton);
                }
            }
            
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            await FileManager.OpenFileDialogue(this);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewProjectDialogue newProjectDialogue = new(this);
            newProjectDialogue.AppWindow.Resize(new Windows.Graphics.SizeInt32(500, 400));
            newProjectDialogue.Activate();
        }

        private async void Window_Closed(object sender, WindowEventArgs args)
        {
            if (await FileManager.SaveProjectFiles())
            {
                return;
            }
        }
    }
}
