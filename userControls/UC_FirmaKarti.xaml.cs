using MaliyeHesaplama.helpers;
using Syncfusion.UI.Xaml.Grid;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_FirmaKarti : UserControl
    {
        private MiniOrm _orm;
        private int Id = 0;
        public UC_FirmaKarti()
        {
            InitializeComponent();
            _orm = new MiniOrm();
        }
        private void btnKayit_Click(object sender, RoutedEventArgs e)
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
            Bildirim.Bilgilendirme("Veri kayıt işlemi başarıyla gerçekleştirildi.");
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (_orm.Delete("Company", Id, true) > 0)
            {
                Temizle();
            }
        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
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

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {
            KayitlariGetir("Önceki");
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
                Bildirim.Bilgilendirme("Gösterilecek başka bir kayıt bulunamadı!");
            }
        }

        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {
            KayitlariGetir("Sonraki");
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
            Temizle();
        }
    }
}
