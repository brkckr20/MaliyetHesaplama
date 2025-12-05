using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    public partial class winMaliyetCalismasiListesi : Window
    {
        public int Id, CompanyId, InventoryId;
        public string CompanyName, InventoryName, OrderNo, CompanyCode, InventoryCode;
        public DateTime Date;
        public bool secimYapildi = false;
        public byte[] ImageData;
        private ICollectionView _collectionView;
        MiniOrm _orm = new MiniOrm();
        public winMaliyetCalismasiListesi()
        {
            InitializeComponent();
        }
        void Search(object sender, string fieldName)
        {
            var tb = sender as TextBox;
            MainHelper.SearchWithColumnHeader(tb, fieldName, _collectionView, lblRecordCount);
        }

        private void _tarih_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "Date");
        }

        private void _calismaNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "OrderNo");
        }

        private void _firmaUnvan_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "CompanyName");
        }

        private void _urunKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "InventoryCode");
        }

        private void _urunAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(sender, "InventoryName");
        }

        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetCostList<dynamic>();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            sfDataGrid.ItemsSource = _collectionView;
        }
        void SearchWithTextboxValue(TextBox aranacakTextbox, string fieldAdi)
        {
            string filterText = aranacakTextbox.Text.ToLower();

            if (_collectionView != null)
            {
                _collectionView.Filter = item =>
                {
                    var dict = (IDictionary<string, object>)item;

                    if (dict.ContainsKey(fieldAdi) && dict[fieldAdi] != null)
                    {
                        string companyName = dict[fieldAdi].ToString().ToLower();
                        return companyName.Contains(filterText);
                    }
                    return false;
                };
                _collectionView.Refresh();
            }
        }
    }
}
