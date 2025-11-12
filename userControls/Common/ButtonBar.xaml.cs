using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls.Common
{
    public partial class ButtonBar : UserControl
    {
        public IPageCommands CommandTarget { get; set; }
        public ButtonBar()
        {
            InitializeComponent();
        }
        private void Yeni_Click(object sender, RoutedEventArgs e) => CommandTarget?.Yeni();
        private void Kaydet_Click(object sender, RoutedEventArgs e) => CommandTarget?.Kaydet();
        private void Sil_Click(object sender, RoutedEventArgs e) => CommandTarget?.Sil();
        private void Yazdir_Click(object sender, RoutedEventArgs e) => CommandTarget?.Yazdir();
        private void Geri_Click(object sender, RoutedEventArgs e) => CommandTarget?.Geri();
        private void Ileri_Click(object sender, RoutedEventArgs e) => CommandTarget?.Ileri();
        private void Listele_Click(object sender, RoutedEventArgs e) => CommandTarget?.Listele();
    }
}
