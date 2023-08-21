using Mindstorm_Simulator.MVVM.Model;
using Mindstorm_Simulator.MVVM.ViewModel;
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

namespace Mindstorm_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FileManager.LoadProjectFiles();
            Management.Setup(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(FileManager.SaveProjectFiles() == Code.ERROR)
            {
                e.Cancel = true;
                // keep program open to perform operations (such as logging), then clode
            }

        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            FileManager.PickProjectFile();
        }
    }
}
