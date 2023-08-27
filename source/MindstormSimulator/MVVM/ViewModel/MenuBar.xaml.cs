using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindstormSimulator.MVVM.ViewModel
{
    public sealed partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            this.InitializeComponent();
        }

        private async void Help_Click(object sender, RoutedEventArgs e)
        {
            Uri webpageUri = new Uri("https://github.com/JRH3221/Mindstorms-Simulator/blob/main/README.md"); // Replace with the URL you want to open

            await Launcher.LaunchUriAsync(webpageUri);
        }
    }
}
