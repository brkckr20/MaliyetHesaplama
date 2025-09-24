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

        private void btnKayit_Click(object sender, RoutedEventArgs e) // kayıt işlemi yapılırken combinedkod kısmını form uygulamasından kontrol et
        {
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                { "InventoryCode",txtKodu.Text + " x" },
                { "InventoryName",lblKumasAdi.Text + " y" },
                { "Unit","" },
                { "Type",1 },
            };
            Id = _orm.Save("Inventory", dict);
            Bildirim.Bilgilendirme2("Kayıt edildi");
        }

        private void btnListe_Click(object sender, RoutedEventArgs e)
        {

        }
        private void dataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
            var button = sender as Button;
            if (button == null) return;
            var row = button.DataContext as InventoryReceipt;
            if (row == null) return;
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(2);
            win.ShowDialog();
            row.InventoryCode = win.Code;
            row.InventoryName = win.Name;

        }

        private void btnKumasKodu_Click(object sender, RoutedEventArgs e)
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(1);
            win.ShowDialog();
            txtKodu.Text = win.Code;
            lblKumasAdi.Text = win.Name;
        }
    }
}
