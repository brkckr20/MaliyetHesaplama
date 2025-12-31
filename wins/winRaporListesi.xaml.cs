using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using MaliyeHesaplama.helpers;

namespace MaliyeHesaplama.wins
{
    public partial class winRaporListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public string ReportName, FormName, Query1, Query2, Query3, Query4, Query5, DataSource1, DataSource2, DataSource3, DataSource4, DataSource5;

        private void rName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "ReportName", _collectionView, lblRecordCount);
        }

        private void rsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "FormName", _collectionView, lblRecordCount);
        }

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
                DataSource1 = record.DataSource1;
                DataSource2 = record.DataSource2;
                DataSource3 = record.DataSource3;
                DataSource4 = record.DataSource4;
                DataSource5 = record.DataSource5;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("Report").Where(x => x.AppId == 2).ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            sfDataGrid.ItemsSource = _collectionView;
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
        private void txtFirmaKodu_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //SearchWithTextboxValue(txtFirmaKodu, "ReportName");
        }
    }
}
