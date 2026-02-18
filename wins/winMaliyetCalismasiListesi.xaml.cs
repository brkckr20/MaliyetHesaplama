using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    public partial class winMaliyetCalismasiListesi : Window
    {
        public int Id, CompanyId, InventoryId, InsertedBy, UpdatedBy,ReceiptId;
        public string CompanyName, InventoryName, OrderNo, CompanyCode, InventoryCode,ReceiptNo;
        public DateTime Date, InsertedDate, UpdatedDate;
        public bool secimYapildi = false;
        public byte[] ImageData;
        private ICollectionView _collectionView;
        MiniOrm _orm = new MiniOrm();
        //private List<ColumnSetting> columnSettings;
        //private winKolonAyarlari ayarlarWindow;

        FilterGridHelpers fgh;
        public winMaliyetCalismasiListesi()
        {
            InitializeComponent();
            fgh = new FilterGridHelpers(grid, "Maliyet Çalışması Listesi", "grid");
        }
        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "InsertedBy", "InsertedDate", "UpdatedBy", "UpdatedDate", "RecipeId", "Type", "ProductImage", "CompanyId", "InventoryId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }
        private void grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }
        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                this.secimYapildi = true;
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                CompanyId = record.CompanyId;
                CompanyName = record.CompanyName;
                InventoryName = record.InventoryName;
                InventoryId = record.InventoryId;
                OrderNo = record.OrderNo;
                Date = record.Date;
                CompanyCode = record.CompanyCode;
                InventoryCode = record.InventoryCode;
                InsertedBy = record.InsertedBy;
                InsertedDate = record.InsertedDate;
                UpdatedBy = record.UpdatedBy;
                UpdatedDate = record.UpdatedDate;
                ReceiptId = record.ReceiptId;
                ReceiptNo = record.ReceiptNo;
                ImageData = _orm.GetImage("Cost", "ProductImage", Id);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetCostList<Cost>();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}
