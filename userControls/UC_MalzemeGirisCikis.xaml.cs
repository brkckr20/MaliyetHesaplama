using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            dataGrid.ItemsSource = table.DefaultView;
            //LoadCurrenciesFromDb();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void MI_SatirSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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
