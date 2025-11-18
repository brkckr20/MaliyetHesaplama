using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_RenkKarti : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        int _colorType, Id = 0, CompanyId = 0;
        bool _isVariant; // boya renk mi - varyant mı?
        //İLK KAYIT BAŞARILI BİR ŞEKİLDE YAPILDI, DİĞER ALANLARIN KAYDI KONTROL EDİLEREK KAYIT EDİLECEK - 18-11-2025
        public UC_RenkKarti(bool isVariant)
        {
            InitializeComponent();
            LoadData();
            _isVariant = isVariant;
        }
        void LoadData()
        {
            ButtonBar.PageCommands = this;
            _orm.LoadCurrenciesFromDbToCombobox(cmbDovizListesi);
            dpOkeyTarihi.SelectedDate = DateTime.Now;
            dpTalepTarihi.SelectedDate = DateTime.Now;
            rbKumas.IsChecked = true;
        }
        public void Geri()
        {

        }

        public void Ileri()
        {

        }

        public void Kaydet()
        {
            if (txtKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    {"Id",Id },{"Type",_colorType},{"Code",txtKodu.Text},{"Name", txtAdi.Text},{"CompanyId", CompanyId}, {"ParentId",0}, {"Date", DateTime.Now},{"RequestDate", dpTalepTarihi.SelectedDate.Value},{"ConfirmDate", dpOkeyTarihi.SelectedDate.Value},{"Price",txtFiyat.Text},{"Forex", cmbDovizListesi.SelectedItem.ToString()},{"IsParent",_isVariant},{"IsUse",Convert.ToBoolean(chckKullanimda.IsChecked)},{"Explanation",txtAciklama.Text},{"EmployeeId",0},{"PantoneNo",txtPantoneNo.Text} // kayit işleminde hata verdi - kontrol edilecek - 18.11.2025
                };
                Id = _orm.Save("Color", dict);
                Bildirim.Bilgilendirme2("Veri kayıt işlemi başarıyla gerçekleştirildi.");
            }
            else
            {
                Bildirim.Uyari2("Kod alanı boş bırakılamaz!");
            }
        }

        public void Listele()
        {
            wins.winRenkListesi win = new wins.winRenkListesi(_isVariant);
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                this.Id = win.Id;
                this.CompanyId = win.CompanyId;
                SetColorType(win.Type);
                chckKullanimda.IsChecked = win.IsUse;
                txtKodu.Text = win.Kodu;
                txtAdi.Text = win.Adi;
                txtAciklama.Text = win.Explanation;
                txtRenk.Text = string.Empty;
                txtPantoneNo.Text = win.Pantone;
                dpTalepTarihi.SelectedDate = win.TalepTarihi;
                dpOkeyTarihi.SelectedDate = win.OkeyTarihi;
                txtFiyat.Text = win.Fiyat.ToString();
                cmbDovizListesi.Text = win.Doviz;
            }
        }
        void SetColorType(int type)
        {
            switch (type)
            {
                case 1:
                    rbKumas.IsChecked = true;
                    break;
                case 2:
                    rbKumas.IsChecked = true;
                    break;
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Color", Id, true)>0)
            {
                Temizle();
            };            
        }

        public void Yazdir()
        {
            if (Id != 0)
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Renk Kartı", Id);
                win.ShowDialog();
            }
            else
            {
                Bildirim.Uyari2("Rapor görüntüleyebilmek için lütfen bir kayır seçiniz!");
            }
        }

        public void Yeni()
        {
            Temizle();
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

        void Temizle()
        {
            this.Id = 0;
            this.CompanyId = 0;
            rbKumas.IsChecked = true;
            chckKullanimda.IsChecked = true;
            txtKodu.Text = string.Empty;
            txtAdi.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            txtRenk.Text = string.Empty;
            txtPantoneNo.Text = string.Empty;
            txtFirmaUnvan.Text = string.Empty;
            dpTalepTarihi.SelectedDate = DateTime.Now;
            dpOkeyTarihi.SelectedDate = DateTime.Now;
            txtFiyat.Text = string.Empty;
            cmbDovizListesi.SelectedIndex = -1;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == rbKumas)
                _colorType = 1;
            else if (sender == rbIplik)
                _colorType = 2;
        }
    }
}
