using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;
using MaliyeHesaplama.wins;
using System.Collections.ObjectModel;
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
        private readonly MiniOrm _orm;
        private readonly UtilityHelpers _uh;

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
            _materialRepo = new MaterialRepository();
            _warehouseRepo = new WarehouseRepository();
            _companyRepo = new CompanyRepository();
            _orm = new MiniOrm();
            _uh = new UtilityHelpers();

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

        private void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpIrsaliyeTarih.SelectedDate = DateTime.Now;
            txtFisNo.Text = _receiptRepo.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", (int)FisTipi);
            gridKalemler.ItemsSource = _items;
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
                            MaterialId = item.Id,
                            MaterialCode = item.MaterialCode,
                            MaterialName = item.MaterialName,
                            NetWeight = item.NetWeight,
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
            throw new NotImplementedException();
        }

        public void Sil()
        {
            throw new NotImplementedException();
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
            var win = new winFisListesiV2((int)FisTipi);
            if (win.ShowDialog() == true)
            {
                _currentId = win.SecilenId;
                if (_currentId > 0)
                {
                    LoadFromId(_currentId);
                }
            }
        }

        private void LoadFromId(int id)
        {
            var receipt = _receiptRepo.GetById(id);
            if (receipt != null)
            {
                _currentId = receipt.Id;
                txtFisNo.Text = receipt.ReceiptNo;
                dpTarih.SelectedDate = receipt.ReceiptDate;
                //dpIrsaliyeTarih.SelectedDate = receipt.DuaDate;
                //txtBelgeNo.Text = receipt.DocumentNo;
                txtIrsaliyeBelgeNo.Text = receipt.InvoiceNo;
                txtAciklama.Text = receipt.Explanation;

                _firmaId = receipt.CompanyId;
                var company = _companyRepo.GetById(receipt.CompanyId);
                if (company != null)
                {
                    txtFirma.Text = company.CompanyCode;
                    lblFirmaAdi.Text = company.CompanyName;
                }

                _depoId = receipt.WareHouseId;
                var warehouse = _warehouseRepo.GetById(receipt.WareHouseId);
                if (warehouse != null)
                {
                    //txtDepo.Text = warehouse.WareHouseCode;
                    //lblDepoAdi.Text = warehouse.WareHouseName;
                }

                LoadItems(id);
            }
        }

        private void LoadItems(int receiptId)
        {
            _items.Clear();
            var items = _receiptRepo.GetItemsByReceiptId(receiptId);
            foreach (var item in items)
            {
                _items.Add(new ReceiptItemViewModel
                {
                    Id = item.Id,
                    MaterialId = item.Id,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    OperationType = item.OperationType,
                    NetWeight = item.NetWeight,
                    NetMeter = item.NetMeter,
                    Piece = item.Piece,
                    UnitPrice = item.UnitPrice,
                    PriceUnit = "Adet",
                    Vat = item.Vat,
                    RowAmount = item.RowAmount,
                    RowExplanation = item.RowExplanation
                });
            }
            gridKalemler.ItemsSource = _items;
            CalculateTotals();
        }
    }
}