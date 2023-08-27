using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MindstormSimulator.MVVM.Model;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindstormSimulator.MVVM.ViewModel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditorPage : Page
    {
        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16.7) // 60 fps = 16.7
        };

        private Rectangle rectangle;
        private double deltaY = -2;

        public EditorPage(FileManager.Project project)
        {
            this.InitializeComponent();
            timer.Tick += Run;
            timer.Start();

            rectangle = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = new SolidColorBrush(Colors.Red)
            };

            Canvas.SetTop(rectangle, 200);
            Canvas.SetLeft(rectangle, 100);
            CanvasPort.Children.Add(rectangle);
        }

        private void Run(object sender, object e)
        {
            // Move the rectangle up by updating the Canvas.Top property
            double currentTop = Canvas.GetTop(rectangle);
            Canvas.SetTop(rectangle, currentTop + deltaY);

            // Check if the rectangle has moved out of the canvas
            if (currentTop + deltaY + rectangle.Height < 0)
            {
                // Reset the rectangle's position to the bottom of the canvas
                Canvas.SetTop(rectangle, CanvasPort.ActualHeight - rectangle.Height);
            }
        }
    }
}
