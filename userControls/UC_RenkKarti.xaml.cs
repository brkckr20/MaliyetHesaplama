using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_RenkKarti : System.Windows.Controls.UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        int _colorType, Id = 0, CompanyId = 0;
        bool _isVariant; // boyahane renk mi - varyant mı?

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
            KayitlariGetir("Önceki");
        }

        public void Ileri()
        {
            KayitlariGetir("");
        }

        public void Kaydet()
        {
            if (txtKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    {"Id",Id },{"Type",_colorType},{"Code",txtKodu.Text},{"Name", txtAdi.Text},{"CompanyId", CompanyId}, {"ParentId",0}, {"Date", DateTime.Now},{"RequestDate", dpTalepTarihi.SelectedDate.Value},{"ConfirmDate", dpOkeyTarihi.SelectedDate.Value},{"Price", Convert.ToDecimal(txtFiyat.Text.Replace(",", "."), CultureInfo.InvariantCulture)},{"Forex", cmbDovizListesi.SelectedItem.ToString()},{"IsParent",_isVariant},{"IsUse",Convert.ToBoolean(chckKullanimda.IsChecked)},{"Explanation",txtAciklama.Text},{"EmployeeId",0},{"PantoneNo",txtPantoneNo.Text}
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
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("Color", Id, "IsParent = 0");
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("Color", Id, "IsParent = 0");
            }

            if (record != null)
            {
                Id = record.Id;
                CompanyId = record.CompanyId;
                SetColorType(Convert.ToInt32(record.Type));
                chckKullanimda.IsChecked = record.IsUse;
                txtKodu.Text = record.Code;
                txtAdi.Text = record.Name;
                dynamic c = _orm.GetById<dynamic>("Company", record.CompanyId);
                txtFirmaUnvan.Text = c != null ? c.CompanyName : "";
                dpTalepTarihi.SelectedDate = record.RequestDate;
                dpOkeyTarihi.SelectedDate = record.RequestDate;
                txtPantoneNo.Text = record.PantoneNo;
                txtFiyat.Text = record.Price != null ? record.Price.ToString() : "0";
                cmbDovizListesi.SelectedItem = record.Forex;
                txtAciklama.Text = record.Explanation;
            }
            else
            {
                Bildirim.Bilgilendirme2("Gösterilecek başka bir kayıt bulunamadı!");
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
            if (_orm.Delete("Color", Id, true) > 0)
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
                Bildirim.Uyari2("Rapor görüntüleyebilmek için lütfen bir kayıt seçiniz!");
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
