using MaliyeHesaplama.models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
            var source = _orm.GetAll<Company>("Company");
            grid.ItemsSource = source;
        }

        private void FilterDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CompanyCode":
                    e.Column.Header = "Firma Kodu";
                    break;
                case "CompanyName":
                    e.Column.Header = "Firma Ünvan";
                    break;
                case "AddressLine1":
                    e.Column.Header = "Adres 1";
                    break;
                case "AddressLine2":
                    e.Column.Header = "Adres 2";
                    break;
                case "AddressLine3":
                    e.Column.Header = "Adres 3";
                    break;
            }
        }

        private void sfDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic record = grid.SelectedItem;
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
