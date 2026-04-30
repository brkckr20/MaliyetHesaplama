using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;
using MaliyeHesaplama.wins;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace MaliyeHesaplama.v2.UserControls
{
    public partial class UC_MalzemeFisV2 : System.Windows.Controls.UserControl, IPageCommands
    {
private readonly ReceiptRepository _receiptRepo;
        private readonly StockMovementRepository _stockMoveRepo;
        private readonly InventoryRepository _materialRepo;
        //private readonly InventoryRepository _materialRepo;
        private readonly WarehouseRepository _warehouseRepo;
        private readonly CompanyRepository _companyRepo;
        private readonly MiniOrm _orm;
        private readonly UtilityHelpers _uh;
        private DataTable table;

        public ReceiptType FisTipi { get; private set; }
        private int _currentId = 0;
        private int _firmaId = 0;
        private int _depoId = 0;

        private ObservableCollection<ReceiptItemViewModel> _items;
        public ObservableCollection<string> OperationTypes { get; private set; }

        public UC_MalzemeFisV2(ReceiptType fisTipi)
        {
            InitializeComponent();
            FisTipi = fisTipi;
            _receiptRepo = new ReceiptRepository();
            _stockMoveRepo = new StockMovementRepository();
            //_stockRepo = new StockRepository();
            _materialRepo = new InventoryRepository();
            _warehouseRepo = new WarehouseRepository();
            _companyRepo = new CompanyRepository();
            _orm = new MiniOrm();
            _uh = new UtilityHelpers();
            table = new DataTable();

            _items = new ObservableCollection<ReceiptItemViewModel>();
            OperationTypes = new ObservableCollection<string>();

            LoadOperationTypes();

            this.DataContext = this;

            ButtonBar.PageCommands = this;
            Yeni();
        }

        private void LoadOperationTypes()
        {
            if (FisTipi == ReceiptType.MalzemeGiris)
            {
                _uh.GetOperationTypeList("MalzemeGirisOperasyonTipleri", cmbKalemIslem);
            }
            else
            {
                _uh.GetOperationTypeList("MalzemeCikisOperasyonTipleri", cmbKalemIslem);
            }
        }

        private void gridKalemler_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Loaded event - OperationTypes count: {OperationTypes.Count}");
        }


        private void AddEmptyRow()
        {
            var emptyRow = new ReceiptItemViewModel();
            _items.Add(emptyRow);
        }

        private void CheckAndAddEmptyRow()
        {
            var lastItem = _items.LastOrDefault();
            if (lastItem != null && !string.IsNullOrEmpty(lastItem.OperationType))
            {
                var hasEmpty = _items.Any(x => string.IsNullOrEmpty(x.OperationType));
                if (!hasEmpty)
                {
                    AddEmptyRow();
                }
            }
        }

        private void btnFirma_Click(object sender, RoutedEventArgs e)
        {
            var win = new winFirmaListesiV2();
            if (win.ShowDialog() == true)
            {
                _firmaId = win.SecilenId;
                txtFirma.Text = win.SecilenKodu;
                lblFirmaAdi.Text = win.SecilenAdi;
            }
        }

        private void btnDepo_Click(object sender, RoutedEventArgs e)
        {
            var win = new winDepoListesiV2();
            if (win.ShowDialog() == true)
            {
                _depoId = win.SecilenId;
                txtDepo.Text = win.SecilenKodu;
                lblDepoAdi.Text = win.SecilenAdi;
            }
        }

        private void btnMalzemeEkle_Click(object sender, RoutedEventArgs e)
        {
            var win = new winMalzemeListesiV2();
            if (win.ShowDialog() == true)
            {
                var newItem = new ReceiptItemViewModel
                {
                    Id = 0,
                    InventoryId = win.SecilenId,
                    InventoryCode = win.SecilenKodu,
                    InventoryName = win.SecilenAdi,
                    OperationType = "Giriş",
                    Piece = 0m,
                    NetMeter = 0m,
                    NetWeight = 0m,
                    UnitPrice = 0m,
                    Vat = win.SecilenVatRate,
                    RowAmount = 0m
                };
                _items.Add(newItem);
            }
        }

        private void btnSatirSil_Click(object sender, RoutedEventArgs e)
        {
            if (gridKalemler.SelectedItem != null)
            {
                var item = (ReceiptItemViewModel)gridKalemler.SelectedItem;
                _items.Remove(item);
            }
        }

        private void ctxFasonGidenler_Click(object sender, RoutedEventArgs e)
        {
            if (_firmaId <= 0)
            {
                MessageBox.Show("Lütfen önce firma seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var win = new winFasonGidenlerListesiV2(_firmaId, txtFirma.Text, lblFirmaAdi.Text);
            if (win.ShowDialog() == true && win.SecilenSatirlar.Count > 0)
            {
                var emptyItem = _items.FirstOrDefault(x => string.IsNullOrEmpty(x.OperationType));
                int startIndex = 0;

                if (emptyItem != null)
                {
                    startIndex = _items.IndexOf(emptyItem);
                }

                foreach (dynamic item in win.SecilenSatirlar)
                {
                    if (startIndex < _items.Count)
                    {
                        var target = _items[startIndex];
                        target.InventoryId = item.InventoryId;
                        target.InventoryCode = item.InventoryCode;
                        target.InventoryName = item.InventoryName;
                        target.OperationType = item.OperationType;
                        target.NetWeight = item.NetWeight;
                        target.NetMeter = item.NetMeter;
                        target.Piece = item.Piece;
                        target.UnitPrice = item.UnitPrice;
                        target.Vat = item.Vat;
                        target.TrackingNumber = item.Id.ToString();
                    }
                    else
                    {
                        _items.Add(new ReceiptItemViewModel
                        {
                            Id = 0,
                            InventoryId = item.InventoryId,
                            InventoryCode = item.InventoryCode,
                            InventoryName = item.InventoryName,
                            OperationType = item.OperationType,
                            NetWeight = item.NetWeight,
                            NetMeter = item.NetMeter,
                            Piece = item.Piece,
                            UnitPrice = item.UnitPrice,
                            Vat = item.Vat,
                            TrackingNumber = item.Id.ToString()
                        });
                    }
                    startIndex++;
                }
                CheckAndAddEmptyRow();
                CalculateTotals();
            }
        }


        private void MI_SatirSil_Click(object sender, RoutedEventArgs e)
        {
            if (gridKalemler.SelectedItem != null)
            {
                var item = (ReceiptItemViewModel)gridKalemler.SelectedItem;
                _items.Remove(item);
            }
        }

        private void MI_CikislardanGeriAl_Click(object sender, RoutedEventArgs e)
        {
            if (FisTipi != ReceiptType.MalzemeGiris)
            {
                MessageBox.Show("Bu özellik sadece Malzeme Giriş işlemlerinde kullanılabilir.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string condition = $"R.ReceiptType = {(int)ReceiptType.MalzemeCikis}";
            if (_depoId > 0)
            {
                condition += $" AND WareHouseId = {_depoId}";
            }

            var win = new winFasonaGidenler(condition, _depoId.ToString(), "Piece", "Çıkış İşlemlerinden Geri Al");
            if (win.ShowDialog() == true)
            {
                var selectedReceipts = win.SelectedReceipts;
                foreach (var r in selectedReceipts)
                {
                    var receiptItems = _receiptRepo.GetItemsByReceiptId(r.Id);
                    foreach (var item in receiptItems)
                    {
                        var newItem = new ReceiptItemViewModel
                        {
                            InventoryId = item.InventoryId,
                            InventoryCode = item.InventoryCode,
                            InventoryName = item.InventoryName,
                            NetWeight = (decimal)item.NetWeight,
                            NetMeter = item.NetMeter,
                            Piece = item.Piece,
                            UnitPrice = item.UnitPrice,
                            PriceUnit = "Adet",
                            Vat = item.Vat,
                            RowExplanation = $"Geri Alınan - {r.ReceiptNo}",
                            OperationType = OperationTypes.FirstOrDefault()
                        };
                        _items.Add(newItem);
                    }
                }
            }
        }

        private void btnSelectMaterial_Click(object sender, RoutedEventArgs e)
        {
            var win = new winMalzemeListesiV2();
            if (win.ShowDialog() == true)
            {
                var material = _materialRepo.GetById(win.SecilenId);
                if (material != null)
                {
                    var button = (System.Windows.Controls.Button)sender;
                    var item = button.DataContext as ReceiptItemViewModel;
                    if (item != null)
                    {
                        item.InventoryId = material.Id;
                        item.InventoryCode = material.InventoryCode;
                        item.InventoryName = material.InventoryName;
                        //item.Vat = material.va;
                    }
                }
            }
        }

        private void gridKalemler_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header?.ToString() == "İşlem Tipi" && e.EditAction == DataGridEditAction.Commit)
            {
                if (gridKalemler.SelectedItem is ReceiptItemViewModel item)
                {
                    item.Id = 0;
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    gridKalemler.CommitEdit(DataGridEditingUnit.Row, true);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            CheckAndAddEmptyRow();
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            var validItems = _items.Where(x => !string.IsNullOrEmpty(x.OperationType)).ToList();

            decimal totalKg = validItems.Sum(x => x.NetWeight);
            decimal totalMt = validItems.Sum(x => x.NetMeter);
            decimal totalAdet = validItems.Sum(x => x.Piece);
            decimal totalBirimFiyat = validItems.Sum(x => x.UnitPrice);
            decimal totalTutar = validItems.Sum(x => x.RowAmount);

            txtToplamKg.Text = totalKg.ToString("N2");
            //txtToplamMt.Text = totalMt.ToString("N2");
            //txtToplamAdet.Text = totalAdet.ToString("N2");
            //txtToplamBirimFiyat.Text = totalBirimFiyat.ToString("N2");
            //txtToplamTutar.Text = totalTutar.ToString("N2");
        }

        private void gridKalemler_CurrentCellChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGrid;
            grid.CommitEdit(DataGridEditingUnit.Cell, true);
            grid.CommitEdit(DataGridEditingUnit.Row, true);
            CheckAndAddEmptyRow();
        }

        private void gridKalemler_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.Column.Header?.ToString() is string header &&
                (header == "Kg" || header == "Metre" || header == "Adet" ||
                 header == "Birim Fiyat" || header == "KDV (%)"))
            {
                if (e.EditingElement is TextBox textBox)
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.LostFocus += TextBox_LostFocus;
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                e.Handled = true;
                var textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, ",");
                textBox.CaretIndex = caretIndex + 1;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Text = textBox.Text.Replace(".", ",");
                textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                textBox.LostFocus -= TextBox_LostFocus;
            }
        }

        private void gridKalemler_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var currentCell = gridKalemler.CurrentCell;
                int currentIndex = currentCell.Column.DisplayIndex;
                int rowIndex = gridKalemler.Items.IndexOf(currentCell.Item);

                if (currentIndex < gridKalemler.Columns.Count - 1)
                {
                    gridKalemler.CurrentCell = new DataGridCellInfo(
                        gridKalemler.Items[rowIndex],
                        gridKalemler.Columns[currentIndex + 1]);
                }
                else
                {
                    var row = currentCell.Item as ReceiptItemViewModel;
                    if (row != null && !string.IsNullOrEmpty(row.OperationType))
                    {
                        int beforeCount = _items.Count;
                        CheckAndAddEmptyRow();

                        if (_items.Count > beforeCount)
                        {
                            gridKalemler.CurrentCell = new DataGridCellInfo(
                                _items[_items.Count - 1],
                                gridKalemler.Columns[0]);
                        }
                        else
                        {
                            if (_items.Count > rowIndex + 1)
                            {
                                gridKalemler.CurrentCell = new DataGridCellInfo(
                                    _items[rowIndex + 1],
                                    gridKalemler.Columns[0]);
                            }
                        }
                    }
                }
                e.Handled = true;
            }
        }

        public void Yeni()
        {
            _currentId = 0;
            _firmaId = 0;
            _depoId = 0;
            txtFisNo.Text = string.Empty;
            txtFirma.Text = "";
            txtDepo.Text = "";
            txtBelgeNo.Text = "";
            txtIrsaliyeBelgeNo.Text = "";
            txtAciklama.Text = "";
            dpTarih.SelectedDate = DateTime.Now;
            dpIrsaliyeTarih.SelectedDate = DateTime.Now;
            lblDepoAdi.Text = string.Empty;
            lblFirmaAdi.Text = string.Empty;
            _items.Clear();
            _items.Add(new ReceiptItemViewModel());
            gridKalemler.ItemsSource = _items;
            CalculateTotals();
        }

        public void Kaydet()
        {
            if (_firmaId == 0)
            {
                MessageBox.Show("Lütfen firma seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_depoId == 0)
            {
                MessageBox.Show("Lütfen depo seçiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var validItems = _items.Where(x => !string.IsNullOrEmpty(x.OperationType) && x.InventoryId > 0).ToList();
            if (validItems.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir kalem ekleyiniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var receiptData = new Dictionary<string, object>
            {
                { "Id", _currentId },
                { "ReceiptNo", _currentId > 0 ? txtFisNo.Text : "" },
                { "ReceiptType", (int)FisTipi },
                { "ReceiptDate", dpTarih.SelectedDate ?? DateTime.Now },
                { "CompanyId", _firmaId },
                { "WareHouseId", _depoId },
                { "Explanation", txtAciklama.Text ?? "" },
                { "InvoiceNo", txtIrsaliyeBelgeNo.Text ?? "" },
                { "InvoiceDate", dpIrsaliyeTarih.SelectedDate },
            };

if (_currentId > 0)
            {
                var oldMovements = _stockMoveRepo.GetByDocument((int)FisTipi, _currentId).ToList();
                decimal totalOldKg = 0, totalOldMeter = 0;
                int totalOldPiece = 0;
                
                foreach (var mov in oldMovements)
                {
                    totalOldKg += mov.DeltaKg;
                    totalOldMeter += mov.DeltaMeter;
                    totalOldPiece += mov.DeltaPiece;
                }
                
                if (totalOldKg != 0 || totalOldMeter != 0 || totalOldPiece != 0)
                {
                    var isGiris = (int)FisTipi == 1;
                    var beforeStock = _stockMoveRepo.GetStock(oldMovements.First().InventoryId, oldMovements.First().WarehouseId);
                    
                    var reverseMovementData = new Dictionary<string, object>
                    {
                        { "Id", 0 },
                        { "InventoryId", oldMovements.First().InventoryId },
                        { "WarehouseId", oldMovements.First().WarehouseId },
                        { "MovementType", isGiris ? 2 : 1 },
                        { "DocumentType", (int)FisTipi },
                        { "DocumentId", _currentId },
                        { "DeltaKg", -totalOldKg },
                        { "DeltaMeter", -totalOldMeter },
                        { "DeltaPiece", -totalOldPiece },
                        { "BeforeKg", beforeStock.kg },
                        { "AfterKg", beforeStock.kg - totalOldKg },
                        { "BeforeMeter", beforeStock.meter },
                        { "AfterMeter", beforeStock.meter - totalOldMeter },
                        { "BeforePiece", beforeStock.piece },
                        { "AfterPiece", beforeStock.piece - totalOldPiece },
                        { "CreatedAt", DateTime.Now }
                    };
                    _stockMoveRepo.Save(reverseMovementData);
                }
                
                _stockMoveRepo.DeleteByDocument((int)FisTipi, _currentId);
                _receiptRepo.DeleteItems(_currentId);
            }

            _currentId = _receiptRepo.Save(receiptData);
            txtFisNo.Text = _currentId.ToString();

            var hizmetTipi = "Hizmet";

            foreach (var item in validItems)
            {
                var itemData = new Dictionary<string, object>
                {
                    { "Id", 0 },
                    { "ReceiptId", _currentId },
                    { "OperationType", item.OperationType },
                    { "InventoryId", item.InventoryId },
                    { "Piece", item.Piece },
                    { "NetMeter", item.NetMeter },
                    { "NetWeight", item.NetWeight },
                    { "UnitPrice", item.UnitPrice },
                    { "MeasurementUnit", item.PriceUnit ?? "Adet" },
                    { "Vat", item.Vat },
                    { "RowAmount", item.RowAmount },
                    { "RowExplanation", item.RowExplanation ?? "" },
                    { "TrackingNumber", item.TrackingNumber ?? "" }
                };
                var itemId = _receiptRepo.SaveItem(itemData);

                if (item.OperationType != hizmetTipi)
                {
                    var isGiris = (int)FisTipi == 1;
                    
                    var currentStock = _stockMoveRepo.GetStock(item.InventoryId, _depoId);
                    var beforeKg = currentStock.kg;
                    var beforeMeter = currentStock.meter;
                    var beforePiece = currentStock.piece;
                    
                    var afterKg = beforeKg + (isGiris ? item.NetWeight : -item.NetWeight);
                    var afterMeter = beforeMeter + (isGiris ? item.NetMeter : -item.NetMeter);
                    var afterPiece = beforePiece + (isGiris ? (int)item.Piece : -(int)item.Piece);

                    var movementData = new Dictionary<string, object>
                    {
                        { "Id", 0 },
                        { "InventoryId", item.InventoryId },
                        { "WarehouseId", _depoId },
                        { "Quantity", 0 },
                        { "MovementType", isGiris ? 1 : 2 },
                        { "DocumentType", (int)FisTipi },
                        { "DocumentId", _currentId },
                        { "DocumentLineId", itemId },
                        { "ReceiptId", _currentId },
                        { "ReceiptItemId", itemId },
                        { "DeltaKg", item.NetWeight },
                        { "DeltaMeter", item.NetMeter },
                        { "DeltaPiece", (int)item.Piece },
                        { "BeforeKg", beforeKg },
                        { "AfterKg", afterKg },
                        { "BeforeMeter", beforeMeter },
                        { "AfterMeter", afterMeter },
                        { "BeforePiece", beforePiece },
                        { "AfterPiece", afterPiece },
                        { "CreatedAt", DateTime.Now }
                    };
                    _stockMoveRepo.Save(movementData);

                    var deltaKg = isGiris ? item.NetWeight : -item.NetWeight;
                    var deltaMeter = isGiris ? item.NetMeter : -item.NetMeter;
                    var deltaPiece = isGiris ? (int)item.Piece : -(int)item.Piece;
                }
            }

            MessageBox.Show("Kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Sil()
        {
            if (_currentId == 0) return;

            var result = MessageBox.Show("Fişi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var movements = _stockMoveRepo.GetByDocument((int)FisTipi, _currentId).ToList();
                
                foreach (var mov in movements)
                {
                    var currentStock = _stockMoveRepo.GetStock(mov.InventoryId, mov.WarehouseId);
                    var reverseKg = -mov.DeltaKg;
                    var reverseMeter = -mov.DeltaMeter;
                    var reversePiece = -mov.DeltaPiece;
                    
                    var reverseMovementData = new Dictionary<string, object>
                    {
                        { "Id", 0 },
                        { "InventoryId", mov.InventoryId },
                        { "WarehouseId", mov.WarehouseId },
                        { "MovementType", mov.MovementType == 1 ? 2 : 1 },
                        { "DocumentType", mov.DocumentType },
                        { "DocumentId", mov.DocumentId },
                        { "DeltaKg", reverseKg },
                        { "DeltaMeter", reverseMeter },
                        { "DeltaPiece", reversePiece },
                        { "BeforeKg", currentStock.kg },
                        { "AfterKg", currentStock.kg + reverseKg },
                        { "BeforeMeter", currentStock.meter },
                        { "AfterMeter", currentStock.meter + reverseMeter },
                        { "BeforePiece", currentStock.piece },
                        { "AfterPiece", currentStock.piece + reversePiece },
                        { "CreatedAt", DateTime.Now }
                    };
                    _stockMoveRepo.Save(reverseMovementData);
                }

                _stockMoveRepo.DeleteByDocument((int)FisTipi, _currentId);
                _receiptRepo.DeleteItems(_currentId);
                _receiptRepo.Delete(_currentId);
                Yeni();
            }
        }

        public void Yazdir()
        {
            throw new NotImplementedException();
        }

        public void Ileri()
        {
            throw new NotImplementedException();
        }

        public void Geri()
        {
            throw new NotImplementedException();
        }

        public void Listele()
        {
            winFisListesiV2 win = new winFisListesiV2((int)FisTipi);
            if (win.ShowDialog() == true && win.SecilenId > 0)
            {
                _currentId = win.SecilenId;
                txtFisNo.Text = _currentId.ToString();
                dpTarih.SelectedDate = win.ReceiptDate;
                txtIrsaliyeBelgeNo.Text = win.InvoiceNo ?? "";
                txtAciklama.Text = win.Explanation ?? "";
                _firmaId = win.CompanyId;
                txtFirma.Text = win.CompanyCode;
                lblFirmaAdi.Text = win.CompanyName;
                _depoId = win.WareHouseId;
                txtDepo.Text = win.WareHouseCode;
                lblDepoAdi.Text = win.WareHouseName;

                _items.Clear();
                foreach (var item in win.SecilenKalemler)
                {
                    _items.Add(new ReceiptItemViewModel
                    {
                        Id = item.Id,
                        InventoryId = item.InventoryId,
                        InventoryCode = item.InventoryCode,
                        InventoryName = item.InventoryName,
                        OperationType = item.OperationType,
                        NetMeter = item.NetMeter,
                        NetWeight = item.NetWeight,
                        Piece = item.Piece,
                        UnitPrice = item.UnitPrice,
                        Vat = item.Vat,
                        RowAmount = item.RowAmount,
                        RowExplanation = item.RowExplanation,
                        PriceUnit = string.IsNullOrEmpty(item.PriceUnit) ? "Kg" : item.PriceUnit
                    });
                }
                gridKalemler.ItemsSource = _items;
            }
        }        
    }
}