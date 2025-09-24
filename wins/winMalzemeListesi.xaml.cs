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
            collectionView.Filter = FilterItems;            
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
        private bool FilterItems(object obj)
        {
            dynamic item = obj;

            if (item.Type != _inventoryType || item.IsPrefix)
                return false;
            bool matchesCode = string.IsNullOrEmpty(txtKodu.Text) || (item.InventoryCode?.ToLower().Contains(txtKodu.Text.ToLower()) ?? false);
            bool matchesName = string.IsNullOrEmpty(txtAdi.Text) || (item.InventoryName?.ToLower().Contains(txtAdi.Text.ToLower()) ?? false);

            return matchesCode && matchesName;
        }

        private void txtKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            collectionView.Refresh();
        }

        private void txtAdi_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            collectionView.Refresh();
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
