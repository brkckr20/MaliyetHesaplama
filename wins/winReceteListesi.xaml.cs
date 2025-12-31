using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    public partial class winReceteListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public bool SecimYapildi = false;
        public int Id,inventoryId;
        public string ReceiptNo;
        public winReceteListesi(int _inventoryId)
        {
            InitializeComponent();
            inventoryId = _inventoryId;
        }
        void SearchWithTextboxValue(System.Windows.Controls.TextBox aranacakTextbox, string fieldAdi)
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
        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic r = sfDataGrid.SelectedItem;
                Id = r.Id;
                ReceiptNo = r.ReceiptNo;
                Close();
            }
        }

        private void txtFirmaKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaKodu, "ReceiptNo");
        }

        private void txtFirmaUnvan_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaUnvan, "RawWidth");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("InventoryReceipt").Where(x => x.ReceiptType == 11 && x.InventoryId == inventoryId).ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            sfDataGrid.ItemsSource = _collectionView;
        }

        private void txtHamBoy_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaUnvan, "RawHeight");
        }

        private void txtMamulEn_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtMamulEn, "ProductWidth");
        }

        private void txtMamülBoy_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtMamülBoy, "ProductHeight");
        }

        private void txtHamGramaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtHamGramaj, "RawGrammage");
        }

        private void txtMamulGramaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtMamulGramaj, "ProductGrammage");
        }
    }
}
