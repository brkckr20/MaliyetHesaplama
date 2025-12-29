using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.models;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_KumasKarti : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        bool _receteOlacak = false;
        string _iplikTurleri;
        int Id = 0, PrefixId, DokumaCinsiId, DesenId;
        private DataTable table;
        public List<string> KalemIslemler { get; set; }

        public UC_KumasKarti()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            this.DataContext = this;
            BaslangicVerileri();
        }
        void BaslangicVerileri()
        {
            var _parametreler = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            _receteOlacak = _parametreler.KumasRecetesiOlacak;
            _iplikTurleri = _parametreler.ReceteOperasyonTipleri;
            KalemIslemler = _iplikTurleri
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();
            var firstRow = new InventoryReceipt();
        }
        void KayitlariGetir(string tip)
        {
            dynamic record = null;
            if (tip == "Önceki")
            {
                record = _orm.GetBeforeRecord<dynamic>("Inventory", Id);
            }
            else
            {
                record = _orm.GetNextRecord<dynamic>("Inventory", Id);
            }

            if (record != null)
            {
                Id = record.Id;
                //txtFirmaKodu.Text = record.CompanyCode;
                //txtFirmaUnvan.Text = record.CompanyName;
                //txtAdres1.Text = record.AddressLine1;
                //txtAdres2.Text = record.AddressLine2;
                //txtAdres3.Text = record.AddressLine3;
            }
            else
            {
                Bildirim.Bilgilendirme2("Gösterilecek başka bir kayıt bulunamadı!");
            }
        }
        private void btnKumasKodu_Click(object sender, RoutedEventArgs e)
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Kumas);
            win.ShowDialog();
            string number = (win.Number + 1).ToString("D3");
            txtKodu.Text = win.Prefix + number;
            lblKumasAdi.Text = win.NameX;
            PrefixId = win.Id;
        }

        void Temizle()
        {
            this.Id = 0;
            txtKodu.Text = string.Empty;
            lblKumasAdi.Text = string.Empty;
            chckIpBoyali.IsChecked = false;
            txtAciklama.Text = string.Empty;
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            string combinedCode = PrefixId.ToString() + DokumaCinsiId.ToString() + DesenId.ToString() + (chckIpBoyali.IsChecked == true ? "1" : "0");
            string inventoryName = $"{txtDokumaCinsi.Text} {lblKumasAdi.Text} {txtKumasDesen.Text} {(chckIpBoyali.IsChecked.HasValue && chckIpBoyali.IsChecked.Value ? "İpliği Boyalı" : "")}";
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                { "InventoryCode",txtKodu.Text},
                { "InventoryName",inventoryName },
                { "CombinedCode",combinedCode },
                { "IsPrefix",false},
                { "Explanation",txtAciklama.Text},
                { "IsUse",chckKullanimda.IsChecked},
                { "Unit",string.Empty},
                { "IsStock",true},
                { "Type",Convert.ToInt32(Enums.Inventory.Kumas)},
            };
            var inventoryCode = _orm.GetInventoryCodeByCombinedCode(combinedCode);
            if (!string.IsNullOrEmpty(inventoryCode)/* && this.Id == 0*/) // burası daha sonra kontrol edilecek, ilk seferde kayıt ettirmezse 2. seferde güncelleme de izin veriyor - eğer bir alan güncellenecekse kayıt kontrolü yapılmalıdır.
            {
                Bildirim.Uyari2($"Belirtmiş olduğunuz özelliklere göre daha önceden bir kumaş kartı kayıt edilmiş.\nLütfen: {inventoryCode}' nolu kaydı kontrol ediniz!");
                return;
            }
            else
            {
                Id = _orm.Save("Inventory", dict);
                var savedInventoryName = _orm.GetById<dynamic>("Inventory", Id);
                inventoryName = savedInventoryName.InventoryName;
                lblKumasAdi.Text = savedInventoryName.InventoryName;
                Bildirim.Bilgilendirme2("Kumaş kayıt işlemi başarılı bir şekilde gerçekleştirildi.");
                _orm.Save("Numerator", new Dictionary<string, object> { { "Id", PrefixId }, { "Number", Convert.ToInt32(txtKodu.Text.Substring(3, 3)) } });
            }
        }

        public void Sil()
        {
            if (_orm.Delete("Inventory", Id, true) > 0)
            {
                Temizle();
            }
        }

        public void Yazdir()
        {
            if (this.Id == 0)
            {
                Bildirim.Uyari2("Form görüntüleyebilmek için bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Kumaş Kartı", Id);
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

        private void btnDesen_Click(object sender, RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("Desen", Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                DesenId = win.Id;
                txtKumasDesen.Text = win.Explanation;
            }
        }

        private void btnDokumaCinsi_Click(object sender, RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("Dokuma Cinsi", Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                DokumaCinsiId = win.Id;
                txtDokumaCinsi.Text = win.Explanation;
            }
        }

        public void Listele()
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.Code != null)
            {
                this.Id = win.Id;
                txtKodu.Text = win.Code;
                lblKumasAdi.Text = win.Name;
                chckIpBoyali.IsChecked = win.YarnDyed;
                txtAciklama.Text = win.Explanation;
            }
        }
    }
}
