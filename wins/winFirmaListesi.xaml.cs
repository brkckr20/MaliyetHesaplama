using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MaliyeHesaplama.wins
{
    public partial class winFirmaListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public int Id;
        public string FirmaKodu, FirmaUnvan, Adres1, Adres2, Adres3;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("Company");
            _collectionView = CollectionViewSource.GetDefaultView(data);
            sfDataGrid.ItemsSource = _collectionView;
        }
        void SearchWithTextboxValue(TextBox aranacakTextbox,string fieldAdi)
        {
            string filterText = aranacakTextbox.Text.ToLower();

            if (_collectionView != null)
            {
                _collectionView.Filter = item =>
                {
                    var dict = (IDictionary<string, object>)item;

                    // Eğer "CompanyName" property’si varsa
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
        private void txtFirmaUnvan_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaUnvan,"CompanyName");
        }

        private void txtFirmaKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaKodu, "CompanyCode");
        }

        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                FirmaKodu = record.CompanyCode;
                FirmaUnvan = record.CompanyName;
                Adres1 = record.AddressLine1;
                Adres2 = record.AddressLine2;
                Adres3 = record.AddressLine3;
                this.Close();
            }
        }
        public winFirmaListesi()
        {
            InitializeComponent();
            txtFirmaUnvan.Focus();
        }               
    }
}
