using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaliyeHesaplama.wins
{
    /// <summary>
    /// Interaction logic for winRenkListesi.xaml
    /// </summary>
    public partial class winRenkListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public int Id, CompanyId, ParentId, EmployeeId,Type;
        public string Kodu, Adi, Pantone, Doviz, Explanation;
        public bool SecimYapildi = false, IsParent, IsUse;
        public DateTime TalepTarihi, OkeyTarihi;
        public decimal Fiyat;
        public winRenkListesi(bool IsVariant)
        {
            InitializeComponent();
            IsParent = IsVariant;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetColorList<dynamic>(IsParent);
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

        private void txtKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtKodu, "Code");
        }

        private void txtAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtAdi, "Name");
        }

        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                CompanyId = record.CompanyId;
                ParentId = record.ParentId;
                TalepTarihi = record.RequestDate;
                OkeyTarihi = record.ConfirmDate;
                Adi = record.Name;
                Kodu = record.Code;
                Pantone = record.PantoneNo;
                Fiyat = record.Price;
                Doviz = record.Forex;
                Type = record.Type;
                IsUse = record.IsUse;
                Explanation = record.Explanation;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void txtPantoneNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtPantoneNo, "PantoneNo");
        }

        private void txtFirma_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
