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
        }
        public void Geri()
        {
            throw new NotImplementedException();
        }

        public void Ileri()
        {
            throw new NotImplementedException();
        }

        public void Kaydet()
        {
            if (txtKodu.Text != string.Empty)
            {
                var dict = new Dictionary<string, object>
                {
                    {"Id",Id },{"Type",_colorType},{"Code",txtKodu.Text},{"Name", txtAdi.Text},{"CompanyId", CompanyId}, {"ParentId",0}, {"Date", DateTime.Now},{"RequestDate", dpTalepTarihi.SelectedDate.Value},{"ConfirmDate", dpOkeyTarihi.SelectedDate.Value},{"Price",txtFiyat.Text},{"Forex", cmbDovizListesi.SelectedItem.ToString()},{"PantoneNo", string.Empty},{"IsParent",_isVariant},{"IsUse",chckKullanimda.IsChecked},{"Explanation",txtAciklama.Text},{"EmployeeId",0}
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
            throw new NotImplementedException();
        }

        public void Sil()
        {
            throw new NotImplementedException();
        }

        public void Yazdir()
        {
            throw new NotImplementedException();
        }

        public void Yeni()
        {
            MessageBox.Show("renk");
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
