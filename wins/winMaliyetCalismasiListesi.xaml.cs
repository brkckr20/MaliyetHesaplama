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

        private void txtUrunKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            //SearchWithTextboxValue(txtUrunKodu, "InventoryCode");
        }// burdan devam edilecek 05-12-2025
        private void _tarih_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void _tarih_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void _calismaNo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void _firmaUnvan_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void _urunKodu_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void _urunAdi_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            //txtUrunAdi.Focus();
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
        private void txtUrunAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            //SearchWithTextboxValue(txtUrunAdi, "InventoryName");
        }

        private void txtFirmaUnvan_TextChanged(object sender, TextChangedEventArgs e)
        {
            //SearchWithTextboxValue(txtFirmaUnvan, "CompanyName");
        }

        private void txtOrderNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            //SearchWithTextboxValue(txtOrderNo, "OrderNo");
        }
    }
}
