using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Windows
{
    public partial class winDepoListesiV2 : Window
    {
        private readonly WarehouseRepository _repo;
        private ICollectionView _collectionView;
        FilterGridHelpers fgh;

        public int SecilenId { get; private set; }
        public string SecilenKodu { get; private set; }
        public string SecilenAdi { get; private set; }

        public winDepoListesiV2()
        {
            InitializeComponent();
            _repo = new WarehouseRepository();
            fgh = new FilterGridHelpers(grid, "Depo Listesi", "gridDepoListe");
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "CreatedAt", "UpdatedAt" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _repo.GetActive().ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                var secilen = (Warehouse)grid.SelectedItem;
                SecilenId = secilen.Id;
                SecilenKodu = secilen.Code;
                SecilenAdi = secilen.Name;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }
    }
}