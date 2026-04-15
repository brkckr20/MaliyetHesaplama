using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;

namespace MaliyeHesaplama.v2.Views
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

        private DataTable _table;

        public UC_MalzemeFisV2(ReceiptType fisTipi)
        {
            InitializeComponent();
            FisTipi = fisTipi;
            _receiptRepo = new ReceiptRepository();
            _stockMoveRepo = new StockMovementRepository();
            _materialRepo = new MaterialRepository();
            _warehouseRepo = new WarehouseRepository();

            ButtonBar.PageCommands = this;
            LoadData();
            Yeni();
        }

        private void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpIrsaliyeTarih.SelectedDate = DateTime.Now;
            txtFisNo.Text = _receiptRepo.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", (int)FisTipi);

            _table = new DataTable();
            _table.Columns.Add("Id", typeof(int));
            _table.Columns.Add("MaterialId", typeof(int));
            _table.Columns.Add("MaterialCode", typeof(string));
            _table.Columns.Add("MaterialName", typeof(string));
            _table.Columns.Add("OperationType", typeof(string));
            _table.Columns.Add("Piece", typeof(decimal));
            _table.Columns.Add("NetMeter", typeof(decimal));
            _table.Columns.Add("NetWeight", typeof(decimal));
            _table.Columns.Add("UnitPrice", typeof(decimal));
            _table.Columns.Add("Vat", typeof(decimal));
            _table.Columns.Add("RowAmount", typeof(decimal));
            _table.Columns.Add("RowExplanation", typeof(string));

            gridKalemler.ItemsSource = _table.DefaultView;
        }

        private void btnFirma_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Firma seçim penceresi
        }

        private void btnDepo_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Depo seçim penceresi
        }

        private void btnMalzemeEkle_Click(object sender, RoutedEventArgs e)
        {
            var win = new winMalzemeListesiV2();
            if (win.ShowDialog() == true)
            {
                var material = _materialRepo.GetById(win.SecilenId);
                if (material != null)
                {
                    var row = _table.NewRow();
                    row["Id"] = 0;
                    row["MaterialId"] = material.Id;
                    row["MaterialCode"] = material.Code;
                    row["MaterialName"] = material.Name;
                    row["Piece"] = 0m;
                    row["NetMeter"] = 0m;
                    row["NetWeight"] = 0m;
                    row["UnitPrice"] = 0m;
                    row["Vat"] = material.VatRate;
                    row["RowAmount"] = 0m;
                    _table.Rows.Add(row);
                }
            }
        }

        private void btnSatirSil_Click(object sender, RoutedEventArgs e)
        {
            if (gridKalemler.SelectedItem != null)
            {
                var row = (DataRowView)gridKalemler.SelectedItem;
                row.Delete();
            }
        }

        private void gridKalemler_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Auto-calculate RowAmount
        }

        private void gridKalemler_CurrentCellChanged(object sender, EventArgs e)
        {
            // Auto-calculate RowAmount
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
            _table.Clear();
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

            foreach (DataRow row in _table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;

                var itemData = new Dictionary<string, object>
                {
                    { "Id", row["Id"] },
                    { "ReceiptId", _currentId },
                    { "MaterialId", row["MaterialId"] },
                    { "OperationType", row["OperationType"]?.ToString() ?? "" },
                    { "Piece", row["Piece"] },
                    { "NetMeter", row["NetMeter"] },
                    { "NetWeight", row["NetWeight"] },
                    { "UnitPrice", row["UnitPrice"] },
                    { "Vat", row["Vat"] },
                    { "RowAmount", row["RowAmount"] },
                    { "RowExplanation", row["RowExplanation"]?.ToString() ?? "" }
                };

                int itemId = orm.Save("ReceiptItem", itemData);

                if (Convert.ToInt32(row["Id"]) == 0)
                    row["Id"] = itemId;

                // StockMovement oluştur
                int materialId = Convert.ToInt32(row["MaterialId"]);
                decimal quantity = Convert.ToDecimal(row["Piece"]);

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