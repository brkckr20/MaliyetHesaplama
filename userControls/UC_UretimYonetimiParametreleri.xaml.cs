using MaliyeHesaplama.helpers;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_UretimYonetimiParametreleri : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        public UC_UretimYonetimiParametreleri()
        {
            InitializeComponent();
            GetDatas();
        }

        void GetDatas()
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            txtOperasyonTipi.Text = data.ReceteOperasyonTipleri;
            chckReceteOlacak.IsChecked = Convert.ToBoolean(data.KumasRecetesiOlacak);
            foreach (ComboBoxItem item in cmbKurSecimi.Items)
            {
                if (item.Tag?.ToString() == data.BazAlinacakKur)
                {
                    cmbKurSecimi.SelectedItem = item;
                    break;
                }
            }
            txtDovizKurlari.Text = data.DovizKurlari;
        }
        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object>()
            {
                { "Id", 1},
                { "ReceteOperasyonTipleri", txtOperasyonTipi.Text },
                { "KumasRecetesiOlacak", chckReceteOlacak.IsChecked },
                { "BazAlinacakKur", (cmbKurSecimi.SelectedItem as ComboBoxItem)?.Tag?.ToString() },
                { "DovizKurlari", txtDovizKurlari.Text },
            };
            _orm.Save("ProductionManagementParams", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarıyla gerçekleştirildi.");
        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
