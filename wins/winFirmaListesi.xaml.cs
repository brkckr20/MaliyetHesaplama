using MaliyeHesaplama.helpers;
using System.ComponentModel;
using System.Data;
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
        public bool SecimYapildi = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<dynamic>("Company");
            _collectionView = CollectionViewSource.GetDefaultView(data);
            sfDataGrid.ItemsSource = _collectionView;
            MainHelper.SetRecordCount(_collectionView,lblRecordCount);
        }
        private void srcCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            MainHelper.SearchWithColumnHeader(textBox,"CompanyCode",_collectionView,lblRecordCount);
        }

        private void srcName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "CompanyName", _collectionView, lblRecordCount);
        }

        private void srcAdres1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "AddressLine1", _collectionView, lblRecordCount);
        }

        private void srcAdres2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "AddressLine2", _collectionView, lblRecordCount);
        }

        private void srcAdres3_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            MainHelper.SearchWithColumnHeader(textBox, "AddressLine3", _collectionView, lblRecordCount);
        }
        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                this.SecimYapildi = true;
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
        }
    }
}
