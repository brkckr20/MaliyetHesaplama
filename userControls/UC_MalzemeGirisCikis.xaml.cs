using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_MalzemeGirisCikis : UserControl, IPageCommands
    {
        Enums.Receipt _receipt;
        MiniOrm _orm = new MiniOrm();
        public int CompanyId = 0, Id, WareHouseId;
        private DataTable table;
        UtilityHelpers _uh = new UtilityHelpers();

        public UC_MalzemeGirisCikis(Enums.Receipt receipt)
        {
            InitializeComponent();
            _receipt = receipt;
            ButtonBar.PageCommands = this;
            LoadData();
        }

        private void btnFirmaListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetCompanyInformation(ref CompanyId,txtFirmaUnvan);
        }

        private void btnDepoListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetWareHouseInformation(ref WareHouseId, txtDepo);
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            var dict0 = new Dictionary<string, object>()
            {
                {"Id", Id},{"ReceiptNo",txtFisNo.Text},{"ReceiptType", Convert.ToInt32(_receipt)},{"ReceiptDate", dpTarih.SelectedDate.Value},{"CompanyId",CompanyId},{"WareHouseId",WareHouseId},{"Explanation",txtAciklama.Text},{"InvoiceNo",txtBelgeNo.Text},{"InvoiceDate", dpSevkTarih.SelectedDate.Value}
            };
            Id = _orm.Save("Receipt", dict0);
            var dbColumns = new List<string> { "Id", "OperationType", "InventoryId", "Piece", "UnitPrice", "RowExplanation", "TrackingNumber", "Vat", "RowAmount" }; // db'ye kayıt edilecek tablo alanları - gridi doğrudan aldığı için
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                var dict = new Dictionary<string, object>();
                foreach (var colName in dbColumns)
                {
                    var value = row[colName];
                    dict[colName] = value == DBNull.Value ? null : value;
                }
                dict["ReceiptId"] = Id;
                int newId = _orm.Save("ReceiptItem", dict, "Id");

                if (Convert.ToInt32(dict["Id"]) == 0)
                    row["Id"] = newId;
            }
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı bir şekilde gerçekleştirildi");
        }

        public void Sil()
        {
            if (_orm.Delete("Receipt", Id, true) > 0)
            {
                _orm.Delete("ReceiptItem", Id, false, "ReceiptId");
                Temizle();
            }
        }
        void Temizle()
        {
            this.Id = 0; this.CompanyId = 0;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(_receipt));
            dpTarih.SelectedDate = DateTime.Now;
            dpSevkTarih.SelectedDate = DateTime.Now;
            txtFirmaUnvan.Text = string.Empty;
            txtBelgeNo.Text = string.Empty;
            WareHouseId = 0;
            txtDepo.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            table.Clear();

        }

        public void Yazdir()
        {
            MainHelper.OpenReportWindow("Malzeme Giriş", Id);
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
            wins.winFisHareketleriListesi win = new wins.winFisHareketleriListesi(WareHouseId, _receipt, false);
            win.ShowDialog();
            if (win.secimYapildi)
            {
                this.Id = win.Id;
                txtFisNo.Text = win.ReceiptNo;
                dpTarih.SelectedDate = win._Date;
                this.CompanyId = win.CompanyId;
                txtFirmaUnvan.Text = win.CompanyName;
                WareHouseId = win._depoId;
                txtDepo.Text = win.WareHouseCode + " - " + win.WareHouseName;
                //dpTermin.SelectedDate = win.DuaDate;
                //txtVade.Text = win.Maturity;
                //txtMusteriOrderNo.Text = win.CustomerOrderNo;
                txtAciklama.Text = win.Explanation;
                table.Clear();
                foreach (var h in win.HareketlerListesi)
                {
                    DataRow row = table.NewRow();
                    row["Id"] = h.ReceiptItemId; // kalem kayıt no
                    row["InventoryId"] = h.InventoryId;
                    row["OperationType"] = h.OperationType;
                    row["InventoryCode"] = h.InventoryCode;
                    row["InventoryName"] = h.InventoryName;
                    //row["Variant"] = h.Variant;
                    row["NetMeter"] = h.NetMeter;
                    row["NetWeight"] = h.NetWeight;
                    row["Piece"] = h.Piece;
                    //row["CashPayment"] = h.CashPayment;
                    //row["DeferredPayment"] = h.DeferredPayment;
                    //row["Forex"] = h.Forex;
                    row["RowExplanation"] = h.RowExplanation;
                    //row["VariantId"] = h.VariantId;
                    //row["VariantCode"] = h.VariantCode;
                    row["CustomerOrderNo"] = h.CustomerOrderNo;
                    row["OrderNo"] = h.OrderNo;
                    row["TrackingNumber"] = h.TrackingNumber != null ? h.TrackingNumber : 0;
                    row["UnitPrice"] = h.UnitPrice;
                    row["Vat"] = h.Vat;
                    row["RowAmount"] = h.RowAmount;
                    table.Rows.Add(row);
                }
                dataGrid.ItemsSource = table.DefaultView;
            }
            //GetSumOrCount();
        }
        void LoadData()
        {
            dpTarih.SelectedDate = DateTime.Now;
            dpSevkTarih.SelectedDate = DateTime.Now;
            txtFisNo.Text = _orm.GetRecordNo("Receipt", "ReceiptNo", "ReceiptType", Convert.ToInt32(Enums.Receipt.MalzemeGiris));
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("OperationType", typeof(string));
            table.Columns.Add("InventoryCode", typeof(string));
            table.Columns.Add("InventoryName", typeof(string));
            //table.Columns.Add("Variant", typeof(string));
            table.Columns.Add("NetMeter", typeof(decimal));
            table.Columns.Add("NetWeight", typeof(decimal));
            table.Columns.Add("Piece", typeof(decimal));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("RowAmount", typeof(decimal));
            //table.Columns.Add("CashPayment", typeof(decimal));
            //table.Columns.Add("DeferredPayment", typeof(decimal));
            //table.Columns.Add("Forex", typeof(string));
            table.Columns.Add("RowExplanation", typeof(string));
            table.Columns.Add("CustomerOrderNo", typeof(string));
            table.Columns.Add("OrderNo", typeof(string));
            table.Columns.Add("ReceiptNo", typeof(string));
            //table.Columns.Add("VariantId", typeof(int));
            //table.Columns.Add("VariantCode", typeof(string));
            table.Columns.Add("TrackingNumber", typeof(int));
            table.Columns.Add("Vat", typeof(decimal));
            dataGrid.ItemsSource = table.DefaultView;
            //LoadCurrenciesFromDb();
            _uh.GetOperationTypeList("MalzemeGirisOperasyonTipleri", cmbKalemIslem);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Kalem İşlem")
            {
                if (dataGrid.SelectedItem is DataRowView drv)
                {
                    drv["Id"] = 0;
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetInventoryInformation(sender, Enums.Inventory.Malzeme);
        }
        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGrid;

            // Hücre ve satırı ZORLA commit et
            grid.CommitEdit(DataGridEditingUnit.Cell, true);
            grid.CommitEdit(DataGridEditingUnit.Row, true);

            if (grid.CurrentItem is not DataRowView rowView)
                return;

            decimal quantity = rowView["Piece"] != DBNull.Value
                ? Convert.ToDecimal(rowView["Piece"])
                : 0;

            decimal unitPrice = rowView["UnitPrice"] != DBNull.Value
                ? Convert.ToDecimal(rowView["UnitPrice"])
                : 0;

            decimal vat = rowView["Vat"] != DBNull.Value
                ? Convert.ToDecimal(rowView["Vat"])
                : 0;

            decimal rowTotal = quantity * unitPrice * (1 + vat / 100);

            rowView["RowAmount"] = rowTotal;
        }

        private void MI_SatirSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _uh.RemoveRow(e, ref dataGrid);
        }

        private void UpdateTotals()
        {
            if (table == null || table.Rows.Count == 0)
            {
                txtTotals.Text = $"Toplam Adet: 0   Toplam Miktar: 0,00";
                return;
            }

            decimal totalPiece = 0;
            decimal totalMeter = 0;
            foreach (DataRow row in table.DefaultView.ToTable().Rows)
            {
                totalPiece += row["Piece"] != DBNull.Value ? Convert.ToDecimal(row["Piece"]) : 0;
                totalMeter += row["NetMeter"] != DBNull.Value ? Convert.ToDecimal(row["NetMeter"]) : 0;
            }
            txtTotals.Text = $"Toplam Adet: {totalPiece:N2}   Toplam Miktar: {totalMeter:N2}";
        }

    }
}
