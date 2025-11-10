using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_FirmaKarti : UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0;
        public UC_FirmaKarti()
        {
            InitializeComponent();
            ButtonBar.CommandTarget = this;
            _orm = new MiniOrm();
        }
        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
           
        }
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("Company", Id);
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("Company", Id);
            }

            if (record != null)
            {
                Id = record.Id;
                txtFirmaKodu.Text = record.CompanyCode;
                txtFirmaUnvan.Text = record.CompanyName;
                txtAdres1.Text = record.AddressLine1;
                txtAdres2.Text = record.AddressLine2;
                txtAdres3.Text = record.AddressLine3;
            }
            else
            {
                Bildirim.Bilgilendirme2("Gösterilecek başka bir kayıt bulunamadı!");
            }
        }

        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void Temizle()
        {
            Id = 0;
            txtFirmaKodu.Text = string.Empty;
            txtFirmaUnvan.Text = string.Empty;
            txtAdres1.Text = string.Empty;
            txtAdres2.Text = string.Empty;
            txtAdres3.Text = string.Empty;
        }

        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void cmSonNoAktar_Click(object sender, RoutedEventArgs e)
        {
            txtFirmaKodu.Text = _orm.GetLastCompanyCode();
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            if (txtFirmaKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    { "Id",Id },
                    {"CompanyCode", txtFirmaKodu.Text },
                    {"CompanyName", txtFirmaUnvan.Text },
                    {"AddressLine1", txtAdres1.Text },
                    {"AddressLine2", txtAdres2.Text },
                    {"AddressLine3", txtAdres3.Text },
                };
                Id = _orm.Save("Company", dict);
                Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Bildirim.Uyari2("Firma kodu boş bırakılamaz!");
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Company", Id, true) > 0)
            {
                Temizle();
            }
        }

        public void Yazdir()
        {
            Bildirim.Bilgilendirme2("Rapor Ekranı açılacak");
        }

        public void Ileri()
        {
            KayitlariGetir("Sonraki");
        }

        public void Geri()
        {
            KayitlariGetir("Önceki");
        }

        public void Listele()
        {
            wins.winFirmaListesi win = new wins.winFirmaListesi();
            win.ShowDialog();
            if (win.FirmaKodu != null)
            {
                Id = win.Id;
                txtFirmaKodu.Text = win.FirmaKodu;
                txtFirmaUnvan.Text = win.FirmaUnvan;
                txtAdres1.Text = win.Adres1;
                txtAdres2.Text = win.Adres2;
                txtAdres3.Text = win.Adres3;
            }
        }
    }
}
