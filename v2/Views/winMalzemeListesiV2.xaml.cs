using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MaliyeHesaplama.v2.Data;

namespace MaliyeHesaplama.v2.Views
{
    public partial class winMalzemeListesiV2 : Window
    {
        private readonly MaterialRepository _repo;
        private ICollectionView _collectionView;

        public int SecilenId { get; private set; }

        public winMalzemeListesiV2()
        {
            InitializeComponent();
            _repo = new MaterialRepository();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _repo.GetAll().ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;
        }

        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic secilen = grid.SelectedItem;
                SecilenId = secilen.Id;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void FilterDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Column.Header = "Id";
                e.Column.Width = 50;
            }
            else if (e.PropertyName == "Code")
                e.Column.Header = "Kodu";
            else if (e.PropertyName == "Name")
                e.Column.Header = "Adı";
            else if (e.PropertyName == "Type")
                e.Column.Header = "Tipi";
            else if (e.PropertyName == "UnitId")
                e.Column.Header = "Birim";
            else if (e.PropertyName == "Barcode")
                e.Column.Header = "Barkod";
            else if (e.PropertyName == "VatRate")
                e.Column.Header = "KDV (%)";
            else if (e.PropertyName == "MinStock")
                e.Column.Header = "Min Stok";
            else if (e.PropertyName == "MaxStock")
                e.Column.Header = "Max Stok";
            else if (e.PropertyName == "IsActive")
                e.Column.Header = "Aktif";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Kolon seçici - gerekirse ekle
        }
    }
}