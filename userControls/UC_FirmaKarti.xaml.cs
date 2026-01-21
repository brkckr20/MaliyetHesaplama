using DocumentFormat.OpenXml.Wordprocessing;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_FirmaKarti : System.Windows.Controls.UserControl, IPageCommands
    {
        private MiniOrm _orm;
        private int Id = 0;
        private bool IsOwnerCompany = false;
        private byte[] imageBytes;
        public UC_FirmaKarti(bool _isOwnerCompany)
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            _orm = new MiniOrm();
            IsOwnerCompany = _isOwnerCompany;
            grdResim.Visibility = IsOwnerCompany ? Visibility.Visible : Visibility.Collapsed;
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
                Bildirim.Uyari2("Gösterilecek başka bir kayıt bulunamadı!");
            }
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
                    {"IsOwner", IsOwnerCompany },
                };
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    dict.Add("Image", imageBytes);
                }
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
            if (this.Id == 0)
            {
                Bildirim.Uyari2("Rapor alabilmek için lütfen bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Firma Kartı", Id);
                win.ShowDialog();
            }
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
            wins.winFirmaListesi win = new wins.winFirmaListesi(IsOwnerCompany);
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

        private void btnResimEkle_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                imageBytes = File.ReadAllBytes(filePath);
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                imgSirketResmi.Source = bitmap;
            }
        }
    }
}
