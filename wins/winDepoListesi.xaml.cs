using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaliyeHesaplama.wins
{
    public partial class winDepoListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        private ICollectionView _collectionView;
        public int Id;
        public string Kodu, Adi;
        public bool SecimYapildi = false, Kullanimda;

        //private List<ColumnSetting> columnSettings;
        private const string SCREEN_NAME = "Depo Listesi";
        private const string GRID_NAME = "gridDepo";
        private int currentUserId = Properties.Settings.Default.RememberUserId;
        //private winKolonAyarlari ayarlarWindow;
        FilterGridHelpers fgh;
        public winDepoListesi()
        {
            InitializeComponent();
            fgh = new FilterGridHelpers(grid, "Depo Listesi", "gridDepoListe");
        }

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "Id" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = _orm.GetAll<WareHouse>("WareHouse");
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
                this.SecimYapildi = true;
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                Kodu = record.Code;
                Adi = record.Name;
                Kullanimda = record.IsUse;
                this.Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }
    }
}
