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

        public ObservableCollection<InventoryReceipt> _recete { get; set; } = new ObservableCollection<InventoryReceipt>();


        public UC_KumasKarti()
        {
            InitializeComponent();
            BaslangicVerileri();
            this.DataContext = this;
        }
        void BaslangicVerileri() //birim ve birim fiyat konularını araştır ve birimleri ve dövizleri databaseden çekerek ilgili yerlere yansıt -23.09.2025
        {
            var _parametreler = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            _receteOlacak = _parametreler.KumasRecetesiOlacak;
            gbRecete.Visibility = _receteOlacak ? Visibility.Visible : Visibility.Collapsed;
            dataGrid.ItemsSource = _recete;
            var firstRow = new InventoryReceipt();
            _recete.Add(firstRow);
            dataGrid.SelectedItem = firstRow;
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
    }
}
