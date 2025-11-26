using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_IplikKarti : UserControl, IPageCommands
    {
        int Id = 0, PrefixId, FCYarnNoId, FCYarnCinsiId, FCYarnCompositionId, CodeId;
        string YarnNo, YarnCinsi, YarnComposition, YarnName, CombinedCode;
        MiniOrm _orm = new MiniOrm();
        public UC_IplikKarti()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            UpdateYarnName();
        }
        private void UpdateYarnName()
        {
            string organik = chckIpBoyali.IsChecked == true ? "Organik" : "";
            YarnName = $"{lblIplikAdi.Text} {YarnNo} {YarnCinsi} {YarnComposition} {organik}";
            CombinedCode = $"{FCYarnNoId}{FCYarnCinsiId}{FCYarnCompositionId}{(organik == "Organik" ? "1" : "0")}";
        }
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("Inventory", Id, "Type = 2");
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("Inventory", Id, "Type = 2");
            }

            if (record != null)
            {
                Id = record.Id;
                txtKodu.Text = record.InventoryCode;
                lblIplikAdi.Text = record.InventoryName;
                FCYarnNoId = record.InventoryNo;
                txtIpNo.Text = _orm.GetById<dynamic>("FeatureCoding", FCYarnNoId).Explanation.ToString();
                FCYarnCinsiId = record.InventoryCinsi;
                txtIpCinsi.Text = _orm.GetById<dynamic>("FeatureCoding", FCYarnCinsiId).Explanation.ToString();
                FCYarnCompositionId = record.InventoryComposition;
                txtKompozisyon.Text = _orm.GetById<dynamic>("FeatureCoding", FCYarnCompositionId).Explanation.ToString();
                chckIpBoyali.IsChecked = record.IsOrganic;
                chckKullanimda.IsChecked = record.IsUse;
            }
            else
            {
                Bildirim.Uyari2("Gösterilecek başka bir kayıt bulunamadı!");
            }
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
            var dict = new Dictionary<string, object>
            {
                {"Id", Id },{"InventoryName",YarnName},{"InventoryCode", txtKodu.Text},{"Unit",string.Empty},{"Type", Convert.ToInt32(Enums.Inventory.Iplik)}, {"SubType","İplik"},{"IsUse",chckKullanimda.IsChecked},{"IsPrefix", false}, {"CombinedCode", CombinedCode},{"IsStock", true},{"InventoryNo", FCYarnNoId},{"InventoryCinsi", FCYarnCinsiId},{"InventoryComposition", FCYarnCompositionId},{"IsOrganic",chckIpBoyali.IsChecked},{"Explanation",txtAciklama.Text}
            };
            if (txtKodu.Text.Trim() == string.Empty || FCYarnNoId == 0 || FCYarnCinsiId == 0 || FCYarnCompositionId == 0)
            {
                Bildirim.Uyari2("Kırmızı ile yazılmış alanlar boş bırakılamaz!");
                return;
            }
            var inventoryCode = _orm.GetInventoryCodeByCombinedCode(CombinedCode);
            if (!string.IsNullOrEmpty(inventoryCode) && this.Id == 0)
            {
                Bildirim.Uyari2($"Belirtmiş olduğunuz özelliklere göre daha önceden bir iplik kartı tanımlaması yapılmış.\nLütfen : {inventoryCode}' nolu kaydı kontrol ediniz.");
                return;
            }
            Id = _orm.Save("Inventory", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı");
            //lblIplikAdi.Text = this.Id != 0 ? YarnName : ""; - bu alan detaylıca incelenmelidir. - aynı adı arka arkaya yazdırabiliyor.
            _orm.Save("Numerator", new Dictionary<string, object> { { "Id", PrefixId }, { "Number", Convert.ToInt32(txtKodu.Text.Substring(3, 3)) } });
        }

        public void Listele()
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                this.Id = win.Id;
                txtKodu.Text = win.Code;
                lblIplikAdi.Text = win.Name;
                YarnName = win.Name;
                var _inventoryFields = _orm.GetById<dynamic>("Inventory", Id);
                var _yarnNo = _orm.GetById<dynamic>("FeatureCoding", _inventoryFields.InventoryNo);
                txtIpNo.Text = _yarnNo.Explanation.ToString();
                YarnNo = _yarnNo.Explanation.ToString();
                FCYarnNoId = _yarnNo.Id;
                var _yarnCinsi = _orm.GetById<dynamic>("FeatureCoding", _inventoryFields.InventoryCinsi);
                txtIpCinsi.Text = _yarnCinsi.Explanation.ToString();
                YarnCinsi = _yarnCinsi.Explanation.ToString();
                FCYarnCinsiId = _yarnCinsi.Id;
                var _yarnComposition = _orm.GetById<dynamic>("FeatureCoding", _inventoryFields.InventoryComposition);
                txtKompozisyon.Text = _yarnComposition.Explanation.ToString();
                YarnComposition = _yarnComposition.Explanation.ToString();
                FCYarnCompositionId = _yarnComposition.Id;
                UpdateYarnName();
                btnKodu.IsEnabled = false;
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Inventory", Id, true) > 0)
            {
                Bildirim.Bilgilendirme2("Kayıt silme işlemi başarılı!");
                Temizle();
            }
        }

        public void Yazdir()
        {
            if (Id == 0)
            {
                Bildirim.Uyari2("Rapor görüntüleyebilmek için lütfen bir kayıt seçiniz!");
                return;
            }
            wins.winRaporSecimi win = new wins.winRaporSecimi("İplik Kartı", Id);
            win.ShowDialog();
        }

        public void Yeni()
        {
            Temizle();
        }

        private void chckIpBoyali_Checked(object sender, RoutedEventArgs e)
        {
            UpdateYarnName();
        }

        private void btnIplikNo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik No", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                FCYarnNoId = win.Id;
                txtIpNo.Text = win.Explanation;
                YarnNo = win.Explanation;
                UpdateYarnName();
            }
        }

        private void btnIplikCinsi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik Cinsi", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                FCYarnCinsiId = win.Id;
                txtIpCinsi.Text = win.Explanation;
                YarnCinsi = win.Explanation;
                UpdateYarnName();
            }
        }

        private void btnIplikKompozisyon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("İplik Kompozisyon", Convert.ToInt32(Enums.Inventory.Iplik));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                FCYarnCompositionId = win.Id;
                txtKompozisyon.Text = win.Explanation;
                YarnComposition = win.Explanation;
                UpdateYarnName();
            }
        }

        private void btnKodu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Iplik);
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                this.CodeId = win.Id;
                string number = (win.Number + 1).ToString("D3");
                txtKodu.Text = win.Prefix + number;
                lblIplikAdi.Text = win.NameX;
                PrefixId = win.Id;
            }
        }
        void Temizle()
        {
            Id = 0;
            txtKodu.Text = string.Empty;
            lblIplikAdi.Text = string.Empty;
            txtIpNo.Text = string.Empty;
            txtIpCinsi.Text = string.Empty;
            txtKompozisyon.Text = string.Empty;
            chckIpBoyali.IsChecked= false;
            txtAciklama.Text = string.Empty;
            chckKullanimda.IsChecked = true;
            PrefixId = 0;
            FCYarnNoId = 0;
            FCYarnCinsiId = 0;
            FCYarnCompositionId = 0;
            YarnCinsi = string.Empty;
            YarnName = string.Empty;
            YarnComposition = string.Empty;
            CombinedCode = string.Empty;
            YarnNo = string.Empty;
        }
    }
}
