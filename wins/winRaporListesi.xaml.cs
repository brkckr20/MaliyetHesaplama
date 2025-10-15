using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;

namespace MaliyeHesaplama.wins
{
    /// <summary>
    /// Interaction logic for winRaporListesi.xaml
    /// </summary>
    public partial class winRaporListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public string ReportName, FormName, Query1, Query2, Query3, Query4, Query5;
        public int Id;
        public bool IsSelectRow = false;
        public winRaporListesi()
        {
            InitializeComponent();
        }

        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                IsSelectRow = true;
                dynamic record = sfDataGrid.SelectedItem;
                Id = record.Id;
                FormName = record.FormName;
                ReportName = record.ReportName;
                Query1 = record.Query1;
                Query2 = record.Query2;
                Query3 = record.Query3;
                Query4 = record.Query4;
                Query5 = record.Query5;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("Report");
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
        private void txtFirmaKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SearchWithTextboxValue(txtFirmaKodu, "ReportName");
        }
    }
}
