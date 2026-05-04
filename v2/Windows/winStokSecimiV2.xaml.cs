using System.Windows;
using System.Collections.ObjectModel;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Data;

namespace MaliyeHesaplama.v2.Windows
{
public partial class winStokSecimiV2 : Window
    {
        private readonly StockRepository _stockRepo;
        private readonly int _depoId;

        public ObservableCollection<StockSelectViewModel> Stoklar { get; set; }
        public List<StockSelectViewModel> SecilenSatirlar { get; private set; }

        public winStokSecimiV2(int depoId)
        {
            InitializeComponent();
            _stockRepo = new StockRepository();
            _depoId = depoId;
            Stoklar = new ObservableCollection<StockSelectViewModel>();
            SecilenSatirlar = new List<StockSelectViewModel>();
            LoadStoklar(_depoId);
        }

        private void LoadStoklar(int depoId)
        {
            Stoklar.Clear();
            var stoklar = _stockRepo.GetByWarehouseId(depoId);
            foreach (var stok in stoklar)
            {
                Stoklar.Add(new StockSelectViewModel
                {
                    InventoryId = stok.InventoryId,
                    InventoryCode = stok.InventoryCode,
                    InventoryName = stok.InventoryName,
                    Quantity = stok.Quantity,
                    Unit = stok.Unit,
                    UnitPrice = stok.UnitPrice,
                    Vat = stok.Vat,
                    WarehouseId = depoId
                });
            }
            grid.ItemsSource = Stoklar;
        }

        private void btnAktar_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = grid.SelectedItems.Cast<StockSelectViewModel>().ToList();
            if (selectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Lütfen en az bir satır seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SecilenSatirlar = selectedItems;
            this.DialogResult = true;
            this.Close();
        }

        private void btnVazgec_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    public class StockSelectViewModel
    {
        public int InventoryId { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Vat { get; set; }
        public int WarehouseId { get; set; }
    }
}