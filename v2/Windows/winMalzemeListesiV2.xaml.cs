using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.v2.Data;

namespace MaliyeHesaplama.v2.Windows
{
    public partial class winMalzemeListesiV2 : Window
    {
        private readonly MaterialRepository _repo;
        private ICollectionView _collectionView;
        FilterGridHelpers fgh;

        public int SecilenId { get; private set; }

        public winMalzemeListesiV2()
        {
            InitializeComponent();
            _repo = new MaterialRepository();
            fgh = new FilterGridHelpers(grid, "Malzeme Listesi", "gridMalzemeListe");
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "CreatedAt", "UpdatedAt", "CategoryId", "UnitId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _repo.GetAllWithDetails().ToList();
            _collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = _collectionView;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }
    }
}