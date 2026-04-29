using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MaliyeHesaplama;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
namespace MaliyeHesaplama.v2.Windows
{
    public partial class winFisListesiV2 : Window
    {
        private readonly ReceiptRepository _repo;
        private readonly MiniOrm _orm;
        private ICollectionView _collectionView;
        FilterGridHelpers fgh;
        public int SecilenId { get; private set; }
        public bool SecimYapildi { get; private set; }
        private readonly int _receiptType;

        public int Id, CompanyId, WareHouseId;
        public string ReceiptNo, CompanyName, CompanyCode, Authorized, Maturity, CustomerOrderNo, Explanation;
        public string WareHouseCode, WareHouseName, DocumentName, InvoiceNo;
        public byte[] Document;
        public DateTime ReceiptDate, DuaDate;
        public string TrackingNumber, OrderNo;
        public List<ReceiptItemViewModel> SecilenKalemler { get; private set; } = new List<ReceiptItemViewModel>();

        public winFisListesiV2(int receiptType)
        {
            InitializeComponent();
            _repo = new ReceiptRepository();
            _orm = new MiniOrm();
            _receiptType = receiptType;
            fgh = new FilterGridHelpers(grid, "Fiş Listesi", "gridFisListe");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = _orm.GetMovementList<ReceiptListDto>($"R.ReceiptType = {_receiptType}");
                _collectionView = CollectionViewSource.GetDefaultView(data);
                grid.ItemsSource = _collectionView;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    fgh.InitializeColumnSettings();
                    fgh.LoadColumnSettingsFromDatabase();
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Hata: {ex.Message}", "Hata");
            }
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "CompanyId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic record = grid.SelectedItem;
                this.SecimYapildi = true;
                this.Id = record.Id;
                this.ReceiptNo = record.ReceiptNo ?? "";
                this.ReceiptDate = record.ReceiptDate;
                this.CompanyId = record.CompanyId;
                this.CompanyName = record.CompanyName ?? "";
                this.CompanyCode = record.CompanyCode ?? "";
                this.Authorized = record.Authorized ?? "";
                this.DuaDate = record.DuaDate != null ? Convert.ToDateTime(record.DuaDate) : DateTime.Now;
                this.Maturity = record.Maturity ?? "";
                this.CustomerOrderNo = record.CustomerOrderNo ?? "";
                this.Explanation = record.Explanation ?? "";
                this.WareHouseId = record.WareHouseId;
                this.WareHouseCode = record.WareHouseCode ?? "";
                this.WareHouseName = record.WareHouseName ?? "";
                this.TrackingNumber = record.TrackingNumber ?? "";
                this.OrderNo = record.OrderNo ?? "";
                this.DocumentName = record.DocumentName ?? "";
                this.InvoiceNo = record.InvoiceNo ?? "";
                this.SecilenId = Id;

                var items = _repo.GetItemsByReceiptId(Id);
                SecilenKalemler = items.Select(x => new ReceiptItemViewModel
                {
                    Id = x.Id,
                    InventoryId = x.InventoryId,
                    InventoryCode = x.InventoryCode ?? "",
                    InventoryName = x.InventoryName ?? "",
                    OperationType = x.OperationType ?? "",
                    NetMeter = x.NetMeter,
                    NetWeight = x.NetWeight ?? 0,
                    Piece = x.Piece,
                    UnitPrice = x.UnitPrice,
                    Vat = x.Vat,
                    RowAmount = x.RowAmount,
                    RowExplanation = x.RowExplanation ?? "",
                    PriceUnit = x.MeasurementUnit ?? "Kg"
                }).ToList();

                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnSec_Click(object sender, RoutedEventArgs e)
        {
            //if (grid.SelectedItem != null)
            //{
            //    dynamic record = grid.SelectedItem;
            //    this.SecimYapildi = true;
            //    this.Id = record.Id;
            //    this.ReceiptNo = record.ReceiptNo;
            //    this.ReceiptDate = record.ReceiptDate;
            //    this.CompanyId = record.CompanyId;
            //    this.CompanyName = record.CompanyName;
            //    this.CompanyCode = record.CompanyCode;
            //    this.Authorized = record.Authorized;
            //    this.DuaDate = record.DuaDate;
            //    this.Maturity = record.Maturity;
            //    this.CustomerOrderNo = record.CustomerOrderNo;
            //    this.Explanation = record.Explanation;
            //    this.WareHouseId = record.WareHouseId;
            //    this.WareHouseCode = record.WareHouseCode;
            //    this.WareHouseName = record.WareHouseName;
            //    this.OrderNo = record.OrderNo;
            //    this.Approved = record.Approved;
            //    this.InventoryId = record.InventoryId;
            //    this.InventoryCode = record.InventoryCode;
            //    this.InventoryName = record.InventoryName;
            //    this.ReceiptItemId = record.ReceiptItemId;
            //    this.NetMeter = record.NetMeter;
            //    this.NetWeight = record.NetWeight;
            //    this.Piece = record.Piece;
            //    this.UnitPrice = record.UnitPrice;
            //    this.Vat = record.Vat;
            //    this.RowAmount = record.RowAmount;
            //    this.RowExplanation = record.RowExplanation;
            //    this.TrackingNumber = record.TrackingNumber;
            //    this.Receiver = record.Receiver;
            //    this.DocumentName = record.DocumentName;
            //    this.InvoiceNo = record.InvoiceNo;
            //    this.SecilenId = Id;
            //    this.DialogResult = true;
            //    this.Close();
            //}
        }

        private void btnIptal_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }

        private void grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }
    }
}