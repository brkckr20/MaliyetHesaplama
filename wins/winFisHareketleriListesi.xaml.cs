using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winFisHareketleriListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private int _receiptType;
        private ICollectionView _collectionView;
        public bool secimYapildi = false;
        public int Id;
        public string ReceiptNo;
        public winFisHareketleriListesi(int depoId, Enums.Receipt receipt)
        {
            InitializeComponent();
            this._receiptType = Convert.ToInt32(receipt);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetMovementList<dynamic>(_receiptType);
            _collectionView = CollectionViewSource.GetDefaultView(data);
            dgListe.ItemsSource = _collectionView;
        }

        private void dgListe_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgListe.SelectedItem != null)
            {
                this.secimYapildi = true;
                dynamic record = dgListe.SelectedItem;
                Id = record.Id;
                ReceiptNo = record.ReceiptNo;
                Close();
            }
            /*
             if (sfDataGrid.SelectedItem != null)
            {
                this.secimYapildi = true;
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                CompanyId = record.CompanyId;
                CompanyName = record.CompanyName;
                InventoryName = record.InventoryName;
                InventoryId = record.InventoryId;
                OrderNo = record.OrderNo;
                Date = record.Date;
                CompanyCode = record.CompanyCode;
                InventoryCode = record.InventoryCode;
                ImageData = _orm.GetImage("Cost", "ProductImage", Id);
                this.Close();
            }
             */
        }
    }
}
