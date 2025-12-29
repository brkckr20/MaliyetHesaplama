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
        string _screenNameForReport;

        public UC_MalzemeGirisCikis(Enums.Receipt receipt)
        {
            InitializeComponent();
            _receipt = receipt;
            ButtonBar.PageCommands = this;
            LoadData();
            SetVisibleControls();
            _screenNameForReport = receipt == Enums.Receipt.MalzemeGiris ? "Malzeme Giriş" : "Malzeme Çıkış";
        }

        private void btnFirmaListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetCompanyInformation(ref CompanyId, txtFirmaUnvan);
        }

        private void btnDepoListesi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainHelper.SetWareHouseInformation(ref WareHouseId, txtDepo);
        }

        public void Yeni()
        {
            Temizle();
            UpdateTotals();
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
            UpdateTotals();
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
            UpdateTotals();
        }

        public void Yazdir()
        {
            MainHelper.OpenReportWindow(_screenNameForReport, Id);
        }

        public void Ileri()
        {
            KayitlariGetir("Önceki");
        }

        public void Geri()
        {
            KayitlariGetir("Sonraki");
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
            UpdateTotals();
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
            if (_receipt == Enums.Receipt.MalzemeGiris)
            {
                _uh.GetOperationTypeList("MalzemeGirisOperasyonTipleri", cmbKalemIslem);
            }
            else
            {
                _uh.GetOperationTypeList("MalzemeCikisOperasyonTipleri", cmbKalemIslem);
            }
            UpdateTotals();
        }
        public void KayitlariGetir(string KayitTipi)
        {
            try
            {
                int id = this.Id;
                int? istenenId = _orm.GetIdForAfterOrBeforeRecord(KayitTipi, "Receipt", id, "ReceiptItem", "ReceiptId", Convert.ToInt32(_receipt));
                if (istenenId == null)
                {
                    Bildirim.Uyari2("Başka bir kayıt bulunamadı!");
                    return;
                }

                string query = MainHelper.GetRecordStringQuery(Convert.ToInt32(_receipt));

                var liste = _orm.GetAfterOrBeforeRecord(query, istenenId.Value);

                if (liste != null && liste.Count > 0)
                {
                    // Üst bilgileri doldur
                    var item = liste[0];
                    this.Id = Convert.ToInt32(item.Id);
                    this.CompanyId = Convert.ToInt32(item.CompanyId);
                    dpTarih.SelectedDate = Convert.ToDateTime(item.ReceiptDate);
                    //dpTermin.SelectedDate = Convert.ToDateTime(item.DuaDate);
                    txtFirmaUnvan.Text = item.CompanyName.ToString();
                    //txtYetkili.Text = item.Authorized.ToString();
                    //txtVade.Text = item.Maturity.ToString();
                    //txtMusteriOrderNo.Text = item.CustomerOrderNo.ToString();
                    txtAciklama.Text = item.Explanation;
                    txtFisNo.Text = item.ReceiptNo;
                    txtBelgeNo.Text = item.InvoiceNo;

                    table.Clear();
                    foreach (var i in liste)
                    {
                        DataRow row = table.NewRow();
                        row["Id"] = i.ReceiptItemId;
                        row["OperationType"] = i.OperationType;
                        row["InventoryId"] = i.InventoryId;
                        row["NetMeter"] = i.NetMeter;
                        //row["CashPayment"] = i.CashPayment;
                        //row["DeferredPayment"] = i.DeferredPayment;
                        row["RowExplanation"] = i.RowExplanation;
                        //row["Forex"] = i.Forex;
                        row["InventoryCode"] = i.InventoryCode;
                        row["InventoryName"] = i.InventoryName;
                        //row["VariantId"] = i.VariantId;
                        //row["VariantCode"] = i.VariantCode;
                        //row["Variant"] = i.Variant;
                        row["CustomerOrderNo"] = i.CustomerOrderNo_; // alt çizgi eklendi çünkü çakışma vardı - ReceiptItem ve Receipt tablosunda aynı isimde alan var
                        row["OrderNo"] = i.OrderNo_;
                        row["TrackingNumber"] = i.TrackingNumber;
                        row["NetWeight"] = i.NetWeight;
                        row["Piece"] = i.Piece;
                        table.Rows.Add(row);
                    }

                    // DataGrid artık DataTable üzerinden çalışıyor
                    dataGrid.ItemsSource = table.DefaultView;
                    UpdateTotals();
                }
                else
                {
                    Bildirim.Uyari2("Başka bir kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2("Hata: " + ex.Message);
            }
            //GetSumOrCount();
            UpdateTotals();
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

        private void MI_FasonGidenler_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string condition = $"R.ReceiptType = {Convert.ToInt32(Enums.Receipt.MalzemeCikis)}";
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            if (Convert.ToBoolean(data.UretimGirisiDepoZorunlu) && WareHouseId == 0)
            {
                condition += $" AND WareHouseId = {WareHouseId}";
                Bildirim.Uyari2("Lütfen bir depo seçimi yapınız.");
                return;
            }
            if (Convert.ToBoolean(data.UretimGirisiDepoZorunlu))
            {
                condition += $" AND WareHouseId = {WareHouseId}";
            }

            wins.winFasonaGidenler win = new wins.winFasonaGidenler(condition, WareHouseId.ToString(), "Piece");
            var result = win.ShowDialog();
            if (result == true)
            {
                var selected = win.SelectedReceipts;
                foreach (var r in selected)
                {
                    var row = table.NewRow();
                    row["Id"] = 0;
                    row["InventoryId"] = r.InventoryId;
                    row["OperationType"] = r.OperationType;
                    row["InventoryCode"] = r.InventoryCode ?? string.Empty;
                    row["InventoryName"] = r.InventoryName ?? string.Empty;
                    row["NetMeter"] = r.NetMeter;
                    row["NetWeight"] = 0m;
                    row["Piece"] = r.Piece;
                    row["RowExplanation"] = r.RowExplanation ?? string.Empty;
                    row["CustomerOrderNo"] = r.CustomerOrderNo ?? string.Empty;
                    row["ReceiptNo"] = r.ReceiptNo ?? string.Empty;
                    row["TrackingNumber"] = r.ReceiptItemId;
                    row["OrderNo"] = r.OrderNo;
                    table.Rows.Add(row);
                }
            }
            UpdateTotals();
        }

        private void MI_SatirSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _uh.RemoveRow(e, ref dataGrid);
        }

        private void stokSecimi_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void UpdateTotals()
        {
            if (table == null || table.Rows.Count == 0)
            {
                txtTotals.Text = "";
                return;
            }

            decimal totalPiece = 0;
            decimal totalMeter = 0;
            foreach (DataRow row in table.DefaultView.ToTable().Rows)
            {
                totalPiece += row["Piece"] != DBNull.Value ? Convert.ToDecimal(row["Piece"]) : 0;
                totalMeter += row["NetMeter"] != DBNull.Value ? Convert.ToDecimal(row["NetMeter"]) : 0;
            }
            txtTotals.Text = $"Toplam: {totalPiece:N2} Adet";//   Toplam : {totalMeter:N2} Metre
            txtTotal1.Text = $"Toplam: {table.Rows.Count.ToString()} Satır (Kalem)";
        }
        void SetVisibleControls()
        {
            if (_receipt == Enums.Receipt.MalzemeCikis)
            {
                dockSevkTarihi.Visibility = System.Windows.Visibility.Collapsed;
                MI_AcikSiparisler.Visibility = System.Windows.Visibility.Collapsed;
                fasGid.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                stokSecimi1.Visibility = System.Windows.Visibility.Collapsed;
                stokSecimi2.Visibility = System.Windows.Visibility.Collapsed;
                btnStok.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

    }
}
