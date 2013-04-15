using System.Windows;
using ProtoBuilder.Model;

namespace ProtoBuilder.Windows {
    public partial class PacketEditWindow {
        public PacketEditWindow(Packet packet) {
            InitializeComponent();
            DataContext = packet;
            CbPacketType.ItemsSource = Packet.GetPacketTypes();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}
