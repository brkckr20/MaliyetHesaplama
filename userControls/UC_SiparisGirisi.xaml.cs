using MaliyeHesaplama.helpers;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{ //sipariş işlemlerine başlandı ve devam edilecek - 03.11.2025
    public partial class UC_SiparisGirisi : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        int Id = 0, CompanyId = 0;
        public UC_SiparisGirisi()
        {
            InitializeComponent();
            BaslangicVerileri();
        }
        void BaslangicVerileri()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis));
        }

        void Temizle()
        {
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.Siparis));
            dpTarih.SelectedDate = DateTime.Now;
            dpTermin.SelectedDate = DateTime.Now;
            CompanyId = 0; Id = 0; txtYetkili.Text = string.Empty; txtFirmaUnvan.Text = string.Empty; txtVade.Text = string.Empty;

        }
        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {
            Temizle();
        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            var dict1 = new Dictionary<string, object>
            {
                {"Id",this.Id },{"ReceiptNo",txtFisNo.Text},{"ReceiptDate",dpTarih.SelectedDate.Value},{"CompanyId",this.CompanyId},{"Authorized",txtYetkili.Text},{"DuaDate",dpTermin.SelectedDate.Value},{"Maturity",txtVade.Text} ,{"ReceiptType",Convert.ToInt32(Enums.Receipt.Siparis)}                
            }; 
            this.Id = _orm.Save("Receipt", dict1);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi.");
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

        private void btnRapor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFirmaListesi_Click(object sender, RoutedEventArgs e)
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                this.CompanyId = win.Id;
                txtFirmaUnvan.Text = win.FirmaUnvan;
            }
        }
    }
}
