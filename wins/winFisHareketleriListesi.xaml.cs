using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winFisHareketleriListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private int _receiptType;
        private ICollectionView _collectionView;
        public bool secimYapildi = false, _withWareHouse, _onayli;
        public int Id, CompanyId, _depoId, _inventoryId, _receiptItemId; // inventoryId alanı maliyet hesaplama için eklendi
        public string ReceiptNo, CompanyName, CompanyCode, Authorized, Maturity, CustomerOrderNo, Explanation, WareHouseCode, WareHouseName, OrderNo, _inventoryCode, _inventoryName;// _inventoryCode ve _inventoryName alanı maliyet hesaplama için eklendi
        FilterGridHelpers fgh;
        string _condition;
        public decimal _netMeter;

        public winFisHareketleriListesi(int depoId, Enums.Receipt receipt, bool withWarehouse = true, string condition = "")
        {
            InitializeComponent();
            this._receiptType = Convert.ToInt32(receipt);
            this._depoId = depoId;
            this._withWareHouse = withWarehouse;
            fgh = new FilterGridHelpers(grid, "Sipariş Listesi", "grid");
            _condition = condition;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }
        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "CompanyId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }

        public DateTime _Date, DuaDate;
        public List<Receipt> HareketlerListesi { get; set; } = new List<Receipt>();

        private IEnumerable<Receipt> _tumHareketler;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _tumHareketler = _orm.GetMovementList<Receipt>($"R.ReceiptType = {_receiptType} {_condition}");
            _collectionView = CollectionViewSource.GetDefaultView(_tumHareketler);
            grid.ItemsSource = _collectionView;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void dgListe_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                this.secimYapildi = true;
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                ReceiptNo = record.ReceiptNo;
                _Date = record.ReceiptDate;
                CompanyId = record.CompanyId;
                CompanyName = record.CompanyName;
                CompanyCode = record.CompanyCode;
                Authorized = record.Authorized;
                DuaDate = record.DuaDate;
                Maturity = record.Maturity.ToString();
                CustomerOrderNo = record.CustomerOrderNo.ToString();
                Explanation = record.Explanation.ToString();
                _depoId = record.WareHouseId;
                WareHouseCode = record.WareHouseCode;
                WareHouseName = record.WareHouseName;
                OrderNo = record.OrderNo;
                _onayli = record.Approved;
                _inventoryId = record.InventoryId;
                _inventoryCode = record.InventoryCode;
                _inventoryName = record.InventoryName;
                _receiptItemId = record.ReceiptItemId; // maliyet hesaplamada ilgili sipariş satırını MaliyetCalisildi = True yapmak için eklendi
                _netMeter = record.NetMeter;
                HareketlerListesi = _tumHareketler.Where(x => x.Id == Id).ToList();
                Close();
            }
        }
    }
}
