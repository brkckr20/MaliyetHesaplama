using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MaterialRepository _materialRepo;
        private readonly WarehouseRepository _warehouseRepo;
        private readonly CompanyRepository _companyRepo;

        public ReceiptType FisTipi { get; private set; }
        private int _currentId = 0;
        private int _firmaId = 0;
        private int _depoId = 0;

        private ObservableCollection<ReceiptItemViewModel> _items;

        public UC_MalzemeFisV2(ReceiptType fisTipi)
        {
            InitializeComponent();
            FisTipi = fisTipi;
            _receiptRepo = new ReceiptRepository();
            _stockMoveRepo = new StockMovementRepository();
            _materialRepo = new MaterialRepository();
            _warehouseRepo = new WarehouseRepository();
            _companyRepo = new CompanyRepository();

            _items = new ObservableCollection<ReceiptItemViewModel>();

            ButtonBar.PageCommands = this;
            Yeni();
        }

        private void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpIrsaliyeTarih.SelectedDate = DateTime.Now;
            txtFisNo.Text = _receiptRepo.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", (int)FisTipi);
            gridKalemler.ItemsSource = _items;
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
                var material = _materialRepo.GetById(win.SecilenId);
                if (material != null)
                {
                    var newItem = new ReceiptItemViewModel
                    {
                        Id = 0,
                        MaterialId = material.Id,
                        MaterialCode = material.Code,
                        MaterialName = material.Name,
                        OperationType = "Giriş",
                        Piece = 0m,
                        NetMeter = 0m,
                        NetWeight = 0m,
                        UnitPrice = 0m,
                        Vat = material.VatRate,
                        RowAmount = 0m
                    };
                    _items.Add(newItem);
                }
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
                        item.MaterialId = material.Id;
                        item.MaterialCode = material.Code;
                        item.MaterialName = material.Name;
                        item.Vat = material.VatRate;
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
            txtFisNo.Text = _receiptRepo.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", (int)FisTipi);
            txtFirma.Text = "";
            txtDepo.Text = "";
            txtBelgeNo.Text = "";
            txtAciklama.Text = "";
            dpTarih.SelectedDate = DateTime.Now;
            dpIrsaliyeTarih.SelectedDate = DateTime.Now;
            _items.Clear();
            _items.Add(new ReceiptItemViewModel());
            gridKalemler.ItemsSource = _items;
            CalculateTotals();
        }

        public void Kaydet()
        {
            if (_depoId == 0)
            {
                MessageBox.Show("Depo seçimi zorunludur!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var validItems = _items.Where(x => !string.IsNullOrEmpty(x.OperationType)).ToList();
            if (!validItems.Any())
            {
                MessageBox.Show("En az bir kalem eklemelisiniz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var orm = new MiniOrm();

            var receiptData = new Dictionary<string, object>
            {
                { "Id", _currentId },
                { "ReceiptNo", txtFisNo.Text },
                { "ReceiptType", (int)FisTipi },
                { "ReceiptDate", dpTarih.SelectedDate.Value },
                { "CompanyId", _firmaId },
                { "WareHouseId", _depoId },
                { "InvoiceNo", txtBelgeNo.Text },
                { "InvoiceDate", dpIrsaliyeTarih.SelectedDate },
                { "Explanation", txtAciklama.Text }
            };

            var itemsList = new List<Dictionary<string, object>>();
            foreach (var item in _items)
            {
                if (string.IsNullOrEmpty(item.OperationType)) continue;

                var itemData = new Dictionary<string, object>
                {
                    { "Id", item.Id },
                    { "InventoryId", item.MaterialId },
                    { "OperationType", item.OperationType },
                    { "Piece", item.Piece },
                    { "NetMeter", item.NetMeter },
                    { "NetWeight", item.NetWeight },
                    { "UnitPrice", item.UnitPrice },
                    { "MeasurementUnit", item.PriceUnit },
                    { "Vat", item.Vat },
                    { "RowAmount", item.RowAmount },
                    { "RowExplanation", item.RowExplanation ?? "" }
                };

                itemsList.Add(itemData);
            }

            _currentId = orm.SaveReceiptWithStock(receiptData, itemsList, _depoId, null, "Receipt", "ReceiptItem");

            foreach (var item in validItems)
            {
                var savedItem = itemsList.FirstOrDefault(x => x.ContainsKey("Id") && Convert.ToInt32(x["Id"]) == 0);
                if (savedItem != null && item.Id == 0)
                {
                    var matched = itemsList.FirstOrDefault(x => 
                        x["InventoryId"]?.Equals(item.MaterialId) == true &&
                        x["OperationType"]?.Equals(item.OperationType) == true);
                    if (matched != null)
                    {
                        item.Id = Convert.ToInt32(matched["Id"]);
                    }
                }
            }

            MessageBox.Show("Kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Sil()
        {
            if (_currentId == 0) return;

            var result = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // StockMovement sil
                _stockMoveRepo.DeleteByDocument(1, _currentId);

                // ReceiptItem sil
                using var orm = new MiniOrm();
                orm.ExecuteRaw($"DELETE FROM ReceiptItem WHERE ReceiptId = {_currentId}");
                orm.ExecuteRaw($"DELETE FROM Receipt WHERE Id = {_currentId}");

                Yeni();
            }
        }

        public void Yazdir()
        {
            MessageBox.Show("Yazdırma özelliği henüz eklenmedi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Ileri()
        {
            // Sonraki kayıt
        }

        public void Geri()
        {
            // Önceki kayıt
        }

        public void Listele()
        {
            var win = new winFisListesiV2((int)FisTipi);
            if (win.ShowDialog() == true)
            {
                LoadFromId(win.SecilenId);
            }
        }

        private void LoadFromId(int id)
        {
            var receipt = _receiptRepo.GetById(id);
            if (receipt == null) return;

            _currentId = receipt.Id;
            txtFisNo.Text = receipt.ReceiptNo;
            dpTarih.SelectedDate = receipt.ReceiptDate;
            dpIrsaliyeTarih.SelectedDate = receipt.InvoiceDate;
            _firmaId = receipt.CompanyId;
            _depoId = receipt.WareHouseId;
            txtBelgeNo.Text = receipt.InvoiceNo ?? "";
            txtAciklama.Text = receipt.Explanation ?? "";

            var company = _companyRepo.GetById(_firmaId);
            if (company != null)
            {
                txtFirma.Text = company.CompanyCode;
                lblFirmaAdi.Text = company.CompanyName;
            }

            var warehouse = _warehouseRepo.GetById(_depoId);
            if (warehouse != null)
            {
                txtDepo.Text = warehouse.Code;
                lblDepoAdi.Text = warehouse.Name;
            }

            _items.Clear();
            var items = _receiptRepo.GetItemsByReceiptId(id);
            foreach (var item in items)
            {
                var material = _materialRepo.GetById(item.MaterialId);
                var vm = new ReceiptItemViewModel
                {
                    Id = item.Id,
                    MaterialId = item.MaterialId,
                    MaterialCode = material?.Code ?? "",
                    MaterialName = material?.Name ?? "",
                    OperationType = item.OperationType,
                    Piece = item.Piece,
                    NetMeter = item.NetMeter,
                    NetWeight = item.NetWeight,
                    UnitPrice = item.UnitPrice,
                    PriceUnit = item.MeasurementUnit ?? "Adet",
                    Vat = item.Vat,
                    RowAmount = item.RowAmount,
                    RowExplanation = item.RowExplanation ?? ""
                };
                _items.Add(vm);
            }

            if (!_items.Any())
            {
                _items.Add(new ReceiptItemViewModel());
            }

            gridKalemler.ItemsSource = _items;
            CalculateTotals();
        }
    }
}