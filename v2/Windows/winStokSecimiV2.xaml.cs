using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.helpers;

namespace MaliyeHesaplama.v2.Windows
{
public partial class winStokSecimiV2 : Window
    {
        private readonly StockRepository _stockRepo;
        private readonly int _depoId;
        FilterGridHelpers fgh;

        public ObservableCollection<StockSelectViewModel> Stoklar { get; set; }
        public List<StockSelectViewModel> SecilenSatirlar { get; private set; }

        public winStokSecimiV2(int depoId)
        {
            InitializeComponent();
            _stockRepo = new StockRepository();
            _depoId = depoId;
            Stoklar = new ObservableCollection<StockSelectViewModel>();
            SecilenSatirlar = new List<StockSelectViewModel>();
            fgh = new FilterGridHelpers(grid, "Stok Seçimi", "gridStokSecimi");
            LoadStoklar(_depoId);
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "WarehouseId", "InventoryId", "Ids", "Tarihler", "Vat", "UnitPrice", "FirstId", "GirisAdet" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void ctxKolonSecici_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }

        private void LoadStoklar(int depoId)
        {
            Stoklar.Clear();
            var stoklar = _stockRepo.GetByWarehouseId(depoId);
            foreach (var stok in stoklar)
            {
                var ids = stok.Ids?.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                int firstId = ids.Length > 0 ? int.TryParse(ids[0], out int parsed) ? parsed : 0 : 0;
                Stoklar.Add(new StockSelectViewModel
                {
                    Id = firstId,
                    InventoryId = stok.InventoryId,
                    InventoryCode = stok.InventoryCode,
                    InventoryName = stok.InventoryName,
                    GirisAdet = stok.GirisAdet,
                    Quantity = stok.QuantityPiece,
                    UnitPrice = stok.UnitPrice,
                    Vat = stok.Vat,
                    Ids = stok.Ids,
                    Tarihler = stok.Tarihler,
                    FirstId = ids.Length > 0 ? ids[0] : "0",
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

        private void ctxExcelAktar_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }
    }

    public class StockSelectViewModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        [Display(Name = "Kodu")]
        public string InventoryCode { get; set; }
        [Display(Name = "Adı")]
        public string InventoryName { get; set; }
        [Display(Name = "Giriş Adet")]
        public int GirisAdet { get; set; }
        [Display(Name = "Kalan")]
        public decimal Quantity { get; set; }
        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "KDV (%)")]
        public decimal Vat { get; set; }
        [Display(Name = "ID'ler")]
        public string Ids { get; set; }
        [Display(Name = "Tarihler")]
        public string Tarihler { get; set; }
        [Display(Name = "Giriş Id")]
        public string FirstId { get; set; }
        public int WarehouseId { get; set; }
    }
}