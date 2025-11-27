using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_KumasKarti : UserControl, IPageCommands
    {
        MiniOrm _orm = new MiniOrm();
        bool _receteOlacak = false;
        string _iplikTurleri;
        int Id = 0, PrefixId;
        private DataTable table;
        //public ObservableCollection<InventoryReceipt> _recete { get; set; } = new ObservableCollection<InventoryReceipt>();
        public List<string> KalemIslemler { get; set; }

        public UC_KumasKarti()
        {
            InitializeComponent();
            ButtonBar.CommandTarget = this;
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
            gbRecete.Visibility = _receteOlacak ? Visibility.Visible : Visibility.Collapsed;
            //dataGrid.ItemsSource = _recete;
            var firstRow = new InventoryReceipt();
            //_recete.Add(firstRow);
            //dataGrid.SelectedItem = firstRow;
            dataGrid.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
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
        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (_recete == null) return;
            //var grid = (DataGrid)sender;
            //if (e.Key == Key.Enter || e.Key == Key.Down)
            //{
            //    var newRow = new InventoryReceipt();
            //    _recete.Add(newRow);
            //    grid.SelectedItem = newRow;
            //    grid.ScrollIntoView(newRow);
            //    e.Handled = true;
            //}
        }

        private void btnMalzemeKodu_Click(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            //if (button == null) return;
            //var row = button.DataContext as InventoryReceipt;
            //if (row == null) return;
            //wins.winMalzemeListesi win = new wins.winMalzemeListesi(2);
            //win.ShowDialog();
            //row.InventoryCode = win.Code;
            //row.InventoryName = win.Name;
            //MessageBox.Show("");
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
            txtHamEn.Text = string.Empty;
            txtHamBoy.Text = string.Empty;
            txtMamulEn.Text = string.Empty;
            txtMamulBoy.Text = string.Empty;
            txtHamGrm2.Text = string.Empty;
            txtMamulGrm2.Text = string.Empty;
            chckIpBoyali.IsChecked = false;
            txtAciklama.Text = string.Empty;
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            string combinedCode = txtKodu.Text.Substring(0, 3) + txtHamEn.Text + txtHamBoy.Text + txtMamulEn.Text + txtMamulBoy.Text + txtHamGrm2.Text + txtMamulGrm2.Text + Convert.ToInt32(chckIpBoyali.IsChecked);
            string _malzemeAdi = lblKumasAdi.Text + (txtHamEn.Text != string.Empty ? " H.En " + txtHamEn.Text : "") + (txtHamBoy.Text != string.Empty ? " H.Boy " + txtHamBoy.Text : "") + (txtMamulEn.Text != string.Empty ? " Mamul En " + txtMamulEn.Text : "") + (txtMamulBoy.Text != string.Empty ? " Mamul boy " + txtMamulBoy.Text : "") + (txtHamGrm2.Text != string.Empty ? " Ham Gr " + txtHamGrm2.Text : "") + (txtMamulGrm2.Text != string.Empty ? " Mamul Gr " + txtMamulGrm2.Text : "") + (Convert.ToInt32(chckIpBoyali.IsChecked) == 0 ? "" : " İpliği Boyalı");
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                { "InventoryCode",txtKodu.Text}, { "InventoryName",_malzemeAdi },
                { "Unit","" }, { "Type",Enums.Inventory.Kumas },
                { "CombinedCode",combinedCode }, { "IsPrefix",false},
                { "Explanation",txtAciklama.Text},{ "RawWidth",txtHamEn.Text }, {"RawHeight", txtHamBoy.Text},
                {"ProdWidth", txtMamulEn.Text }, {"ProdHeight", txtMamulBoy.Text},
                {"RawGrammage", txtHamGrm2.Text }, {"ProdGrammage",txtMamulGrm2.Text}, {"YarnDyed",chckIpBoyali.IsChecked}
            };
            var inventoryCode = _orm.GetInventoryCodeByCombinedCode(combinedCode);

            if (!string.IsNullOrEmpty(inventoryCode))
            {
                Bildirim.Uyari2($"Belirtmiş olduğunuz özelliklere göre daha önceden bir kumaş kartı kayıt edilmiş.\nLütfen: {inventoryCode}' nolu kaydı kontrol ediniz!");
                return;
            }

            else
            {
                Id = _orm.Save("Inventory", dict);
                lblKumasAdi.Text = _malzemeAdi;
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
            wins.winRaporSecimi win = new wins.winRaporSecimi("Kumaş Kartı", Id);
            win.ShowDialog();
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
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.Code != null)
            {
                this.Id = win.Id;
                txtKodu.Text = win.Code;
                lblKumasAdi.Text = win.Name;
                txtHamEn.Text = win.RawWidth;
                txtHamBoy.Text = win.RawHeight;
                txtMamulEn.Text = win.ProdWidth;
                txtMamulBoy.Text = win.ProdHeight;
                txtHamGrm2.Text = win.RawGrammage;
                txtMamulGrm2.Text = win.ProdGrammage;
                chckIpBoyali.IsChecked = win.YarnDyed;
                txtAciklama.Text = win.Explanation;
            }
        }
    }
}
