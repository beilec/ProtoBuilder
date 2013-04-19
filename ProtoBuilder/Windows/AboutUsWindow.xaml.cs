using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ProtoBuilder.Windows {
    /// <summary>
    /// Interaction logic for AboutUsWindow.xaml
    /// </summary>
    public partial class AboutUsWindow {
        public AboutUsWindow() {
            InitializeComponent();
            Application.Current.NavigationFailed += Application_NavigationFailed;
        }

        private void Application_NavigationFailed(object sender, NavigationFailedEventArgs e) {
            if (e.Exception is System.Net.WebException) {
                MessageBox.Show("Site " + e.Uri + " not available :(");
                e.Handled = true;
            }
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(Link.NavigateUri.AbsoluteUri);
        }

        private void Linkpdev_OnClick(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start(Linkpdev.NavigateUri.AbsoluteUri);
        }
    }
}
