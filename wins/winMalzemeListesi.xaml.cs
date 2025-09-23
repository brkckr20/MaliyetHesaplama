using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winMalzemeListesi : Window
    {
        public int _inventoryType, Id;
        public string Code, Name;
        private ICollectionView collectionView;
        MiniOrm _orm = new MiniOrm();
        public winMalzemeListesi(int InventoryType)
        {
            InitializeComponent();
            _inventoryType = InventoryType;
            var data = _orm.GetAll<dynamic>("Inventory");
            txtAdi.Focus();
            collectionView = CollectionViewSource.GetDefaultView(data);
            collectionView.Filter = obj =>
            {
                dynamic item = obj;
                return item.Type == _inventoryType && item.IsPrefix == false;
            };
            sfDataGrid.ItemsSource = collectionView;
            switch (_inventoryType)
            {
                case 2:
                    Title = "İplik Kartı Listesi";
                    break;
                default:
                    break;
            }
        }

        private void txtKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DataGridSearchHelper.SearchWithTextboxValue(txtKodu, "InventoryCode", collectionView);
        }

        private void txtAdi_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DataGridSearchHelper.SearchWithTextboxValue(txtAdi, "InventoryName", collectionView);
        }

        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                Code = record.InventoryCode;
                Name = record.InventoryName;
                this.Close();
            }
        }
    }
}
