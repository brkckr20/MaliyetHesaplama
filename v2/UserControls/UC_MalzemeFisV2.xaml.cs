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
using System.Windows.Media;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;

namespace MaliyeHesaplama.v2.UserControls
{
    public partial class UC_MalzemeFisV2 : System.Windows.Controls.UserControl, IPageCommands
    {
        private readonly ReceiptRepository _receiptRepo;
        private readonly StockMovementRepository _stockMoveRepo;
        private readonly MaterialRepository _materialRepo;
        private readonly WarehouseRepository _warehouseRepo;

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
            if (e.Column.Header.ToString() == "İşlem Tipi" && e.EditAction == DataGridEditAction.Commit)
            {
                if (gridKalemler.SelectedItem is ReceiptItemViewModel item)
                {
                    item.Id = 0;
                    var hasEmpty = _items.Any(x => string.IsNullOrEmpty(x.OperationType));
                    if (!hasEmpty)
                    {
                        _items.Add(new ReceiptItemViewModel());
                    }
                }
            }
        }

        private void gridKalemler_CurrentCellChanged(object sender, EventArgs e)
        {
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
                        AddEmptyRow();
                        gridKalemler.CurrentCell = new DataGridCellInfo(
                            _items[_items.Count - 1],
                            gridKalemler.Columns[0]);
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
        }

        public void Kaydet()
        {
            if (_depoId == 0)
            {
                MessageBox.Show("Depo seçimi zorunludur!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            _currentId = orm.Save("Receipt", receiptData);

            foreach (var item in _items)
            {
                if (string.IsNullOrEmpty(item.OperationType)) continue;

                var itemData = new Dictionary<string, object>
                {
                    { "Id", item.Id },
                    { "ReceiptId", _currentId },
                    { "MaterialId", item.MaterialId },
                    { "OperationType", item.OperationType },
                    { "Piece", item.Piece },
                    { "NetMeter", item.NetMeter },
                    { "NetWeight", item.NetWeight },
                    { "UnitPrice", item.UnitPrice },
                    { "PriceUnit", item.PriceUnit },
                    { "Vat", item.Vat },
                    { "RowAmount", item.RowAmount },
                    { "RowExplanation", item.RowExplanation ?? "" }
                };

                int itemId = orm.Save("ReceiptItem", itemData);

                if (item.Id == 0)
                    item.Id = itemId;

                // StockMovement oluştur
                int materialId = item.MaterialId;
                decimal quantity = item.Piece;

                if (quantity > 0)
                {
                    int movementType = (FisTipi == ReceiptType.MalzemeCikis) ? 2 : 1;

                    var stockData = new Dictionary<string, object>
                    {
                        { "MaterialId", materialId },
                        { "WarehouseId", _depoId },
                        { "Quantity", movementType == 2 ? -quantity : quantity },
                        { "MovementType", movementType },
                        { "DocumentType", 1 },
                        { "DocumentId", _currentId },
                        { "DocumentLineId", itemId },
                        { "CreatedAt", DateTime.Now }
                    };

                    _stockMoveRepo.Save(stockData);
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
            _firmaId = receipt.CompanyId;
            _depoId = receipt.WareHouseId;
            txtBelgeNo.Text = receipt.InvoiceNo ?? "";
            txtAciklama.Text = receipt.Explanation ?? "";

            // Kalemleri yükle
            // TODO: ReceiptItem yükleme
        }
    }
}