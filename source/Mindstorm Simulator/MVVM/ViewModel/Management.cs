using Mindstorm_Simulator.MVVM.Model;
using Mindstorm_Simulator.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mindstorm_Simulator.MVVM.ViewModel
{
    class Management
    {

        public static void Setup(MainWindow mainWindow) 
        {
            if (FileManager.KnownProjects.Count > 0)
            {
                mainWindow.RecentProjectBlock.Visibility = Visibility.Collapsed;

                foreach(var project in FileManager.KnownProjects)
                {
                    mainWindow.ProjectsStack.Children.Add(new ProjectButton(project));
                }
            }
        }
    }
}
