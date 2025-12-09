using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls.Common
{
    public partial class ButtonBar : UserControl
    {
        public IPageCommands CommandTarget { get; set; }
        public IPageCommands PageCommands { get; set; }

        public ButtonBar()
        {
            InitializeComponent();
        }
        private void Yeni_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Yeni();
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Kaydet();
        }

        private void Listele_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Listele();
        }

        private void Geri_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Geri();
        }

        private void Ileri_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Ileri();
        }

        private void Yazdir_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Yazdir();
        }

        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            PageCommands?.Sil();
        }
        public bool YeniEnabled
        {
            get => btnYeni.IsEnabled;
            set => btnYeni.IsEnabled = value;
        }

        public bool KaydetEnabled
        {
            get => Kayit_Click.IsEnabled;
            set => Kayit_Click.IsEnabled = value;
        }

        public bool ListeEnabled
        {
            get => btnListele.IsEnabled;
            set => btnListele.IsEnabled = value;
        }

        public bool SilEnabled
        {
            get => btnSil.IsEnabled;
            set => btnSil.IsEnabled = value;
        }

        public bool YazdirEnabled
        {
            get => btnYazdir.IsEnabled;
            set => btnYazdir.IsEnabled = value;
        }
        //private void Yeni_Click(object sender, RoutedEventArgs e) => CommandTarget?.Yeni();
        //private void Kaydet_Click(object sender, RoutedEventArgs e) => CommandTarget?.Kaydet();
        //private void Sil_Click(object sender, RoutedEventArgs e) => CommandTarget?.Sil();
        //private void Yazdir_Click(object sender, RoutedEventArgs e) => CommandTarget?.Yazdir();
        //private void Geri_Click(object sender, RoutedEventArgs e) => CommandTarget?.Geri();
        //private void Ileri_Click(object sender, RoutedEventArgs e) => CommandTarget?.Ileri();
        //private void Listele_Click(object sender, RoutedEventArgs e) => CommandTarget?.Listele();
    }
}
