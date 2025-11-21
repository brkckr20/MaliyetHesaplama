using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_IplikKarti : UserControl, IPageCommands
    {
        public UC_IplikKarti()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
        }
        int Id = 0, PrefixId;
        // ÖZELLİK KODLAMALAR TAMAMLANDI - KARTIN KAYIT İŞLEMLERİ VE KONTROLLER DEVAM EDİLECEK 21.11.2025

        public void Geri()
        {

        }

        public void Ileri()
        {

        }

        public void Kaydet()
        {

        }

        public void Listele()
        {

        }

        public void Sil()
        {

        }

        public void Yazdir()
        {

        }

        public void Yeni()
        {

        }

        private void btnIplikNo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik No", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                txtIpNo.Text = win.Explanation;
            }
        }

        private void btnIplikCinsi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik Cinsi", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                txtIpCinsi.Text = win.Explanation;
            }
        }

        private void btnIplikKompozisyon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik Kompozisyon", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                txtKompozisyon.Text = win.Explanation;
            }
        }

        private void btnKodu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Iplik);
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                string number = (win.Number + 1).ToString("D3");
                txtKodu.Text = win.Prefix + number;
                lblKumasAdi.Text = win.NameX;
                PrefixId = win.Id;
            }
        }
    }
}
