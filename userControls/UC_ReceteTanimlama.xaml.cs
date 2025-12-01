using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Data;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_ReceteTanimlama : UserControl, IPageCommands
    {
        int _InventoryType, inventoryId, Id = 0;
        MiniOrm _orm = new MiniOrm();
        private DataTable table;
        public UC_ReceteTanimlama(int inventoryType)
        {
            InitializeComponent();
            _InventoryType = inventoryType;
            ButtonBar.PageCommands = this;
            BaslangicVerileri();
        }

        public void Geri()
        {
            throw new NotImplementedException();
        }

        public void Ileri()
        {
            throw new NotImplementedException();
        }

        public void Kaydet()
        {
            _Kaydet();
        }

        public void Listele()
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

        public void Yeni()
        {
            throw new NotImplementedException();
        }
        void BaslangicVerileri()
        {
            txtFisNo.Text = _orm.GetRecordNo("InventoryReceipt", "ReceiptNo", "InventoryType", _InventoryType);
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("InventoryId", typeof(int));
            table.Columns.Add("OperationType", typeof(string));
            table.Columns.Add("InventoryCode", typeof(string));
            table.Columns.Add("InventoryName", typeof(string));
            table.Columns.Add("Variant", typeof(string));
            table.Columns.Add("NetMeter", typeof(decimal));
            table.Columns.Add("CashPayment", typeof(decimal));
            table.Columns.Add("DeferredPayment", typeof(decimal));
            table.Columns.Add("Forex", typeof(string));
            table.Columns.Add("RowExplanation", typeof(string));
            table.Columns.Add("VariantId", typeof(int));
            table.Columns.Add("VariantCode", typeof(string));
            dataGrid.ItemsSource = table.DefaultView;
            LoadOperationTypesFromDb();
        }
        private void LoadOperationTypesFromDb()
        {
            var data = _orm.GetById<dynamic>("ProductionManagementParams", 1);
            string list = data.ReceteOperasyonTipleri;
            cmbOperasyonTipleri.ItemsSource = list.Split(',').ToList();
        }
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void btnKumasListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            DataRowView rowView = btn.DataContext as DataRowView;
            if (rowView == null) return;

            wins.winMalzemeListesi win = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Iplik));
            if (win.ShowDialog() == true)
            {
                rowView["InventoryId"] = win.Id;
                rowView["InventoryCode"] = win.Code;
                rowView["InventoryName"] = win.Name;
            }
        }

        private void btnVariantListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnKumasKodu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            wins.winMalzemeListesi win = new wins.winMalzemeListesi(_InventoryType);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                inventoryId = win.Id;
                txtKumasKodu.Text = win.Code;
                lblKumasAdi.Content = win.Name;
            }
        }
        void _Kaydet()
        {
            var dict = new Dictionary<string, object>
            {
                {"Id",Id },{"ReceiptNo",txtFisNo.Text},{"RawWidth",txtHamEn.Text},{"RawHeight",txtHamBoy.Text},{"ProductWidth",txtMamulEn.Text},{"ProductHeight",txtMamulBoy.Text}, {"RawGrammage",txtHamGramaj.Text},{"ProductGrammage",txtMamulGramaj.Text},{"ReceiptType", Convert.ToInt32(Enums.Receipt.KumasRecetesi)},{"InventoryId",inventoryId},{"InventoryType",Convert.ToInt32(Enums.Inventory.Kumas)}
            };
            Id = _orm.Save("InventoryReceipt", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi başarılı");
            //InventoryReceiptItem tablosu oluşturulacak ve kumaş reçetesi ilgili tabloya kayıt edilecek - 01.12.2025
        }
    }
}
