using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Coswalt.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ButtonState == MouseButtonState.Pressed) {
                DependencyObject originalSource = (DependencyObject)e.OriginalSource;

                if (FindParent<Menu>(originalSource) != null ||
                    FindParent<MenuItem>(originalSource) != null ||
                    FindParent<Button>(originalSource) != null) {
                    return;
                }

                try {
                    this.DragMove();
                } catch (InvalidOperationException) {
                }
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null) {
                if (parent is T parentAsT)
                    return parentAsT;
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
        }
    }
}