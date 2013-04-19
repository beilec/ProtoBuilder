using System.Windows;
using ProtoBuilder.Model;

namespace ProtoBuilder.Windows {
    /// <summary>
    /// Interaction logic for ProtocolPropertiesWindow.xaml
    /// </summary>
    public partial class ProtocolPropertiesWindow {
        public ProtocolPropertiesWindow(Protocol protocol) {
            InitializeComponent();
            DataContext = protocol;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
