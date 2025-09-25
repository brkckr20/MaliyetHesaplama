using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_KumasKarti : UserControl
    {
        MiniOrm _orm = new MiniOrm();
        bool _receteOlacak = false;
        string _iplikTurleri;
        int Id = 0;

        public ObservableCollection<InventoryReceipt> _recete { get; set; } = new ObservableCollection<InventoryReceipt>();
        public List<string> KalemIslemler { get; set; }

        public UC_KumasKarti()
        {
            InitializeComponent();
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
            dataGrid.ItemsSource = _recete;
            var firstRow = new InventoryReceipt();
            _recete.Add(firstRow);
            dataGrid.SelectedItem = firstRow;
            dataGrid.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
        }

        private void btnYeni_Click(object sender, RoutedEventArgs e)
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

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            string combinedCode = txtKodu.Text.Substring(0, 3) + txtHamEn.Text + txtHamBoy.Text + txtMamulEn.Text + txtMamulBoy.Text + txtHamGrm2.Text + txtMamulGrm2.Text + Convert.ToInt32(chckIpBoyali.IsChecked);
            string _malzemeAdi = lblKumasAdi.Text + (txtHamEn.Text != string.Empty ? " H.En " + txtHamEn.Text : "") + (txtHamBoy.Text != string.Empty ? " H.Boy " + txtHamBoy.Text : "") + (txtMamulEn.Text != string.Empty ? " Mamul En " + txtMamulEn.Text : "") + (txtMamulBoy.Text != string.Empty ? " Mamul boy " + txtMamulBoy.Text : "") + (txtHamGrm2.Text != string.Empty ? " Ham Gr " + txtHamGrm2.Text : "") + (txtMamulGrm2.Text != string.Empty ? " Ham Gr " + txtMamulGrm2.Text : "") + (Convert.ToInt32(chckIpBoyali.IsChecked) == 0 ? "" : " İpliği Boyalı");
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                { "InventoryCode",txtKodu.Text},
                { "InventoryName",_malzemeAdi },
                { "Unit","" },
                { "Type",Enums.Inventory.Kumas },
                { "CombinedCode",combinedCode },
                { "IsPrefix",Convert.ToInt32(chckIpBoyali.IsChecked)},
                { "Explanation",txtAciklama.Text},
            };
            var inventoryCode = _orm.GetInventoryCodeByCombinedCode(combinedCode);

            if (!string.IsNullOrEmpty(inventoryCode))
            {
                Bildirim.Uyari2($"Yukarıdaki özelliklere göre daha önceden bir kumaş kartı kayıt edilmiş.\nLütfen: {inventoryCode}' nolu kaydı kontrol ediniz!");
                return;
            }

            else
            {
                Id = _orm.Save("Inventory", dict);
                lblKumasAdi.Text = _malzemeAdi;
                Bildirim.Bilgilendirme2("Kumaş kayıt işlemi başarılı bir şekilde gerçekleştirildi.");
            }

        }
        private void btnListe_Click(object sender, RoutedEventArgs e)
        {

        }
        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_recete == null) return;
            var grid = (DataGrid)sender;
            if (e.Key == Key.Enter || e.Key == Key.Down)
            {
                var newRow = new InventoryReceipt();
                _recete.Add(newRow);
                grid.SelectedItem = newRow;
                grid.ScrollIntoView(newRow);
                e.Handled = true;
            }
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
        }
    }
}
