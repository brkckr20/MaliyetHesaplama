using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input; // MouseButtonEventArgs için

namespace MaliyeHesaplama.wins
{
    public partial class winMalzemeListesi : Window
    {
        public int _inventoryType, Id;
        public string Code, Name, RawWidth, RawHeight, ProdWidth, ProdHeight, RawGrammage, ProdGrammage, Explanation;
        public bool YarnDyed,IsUse;

        private readonly int CurrentUserId = Properties.Settings.Default.RememberUserId;
        //private List<ColumnSelector> _savedColumnSettings;
        private ICollectionView collectionView;
        MiniOrm _orm = new MiniOrm();
        //private List<ColumnSetting> columnSettings;
        //private winKolonAyarlari ayarlarWindow;
        FilterGridHelpers fgh;
        public winMalzemeListesi(int InventoryType)
        {
            InitializeComponent();
            _inventoryType = InventoryType;
            fgh = new FilterGridHelpers(grid, "Malzeme Listesi", "grid" + Title);
        }
        private void FilterDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "InsertedBy", "InsertedDate", "UpdatedBy", "UpdatedDate", "RecipeId", "Type", "ProductImage", "CompanyId", "InventoryId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fgh.OpenColumnsForm(this);
        }
        private void grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            fgh.GridReOrdered(sender, e);
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            fgh.ExportToExcel();
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                Code = record.InventoryCode;
                Name = record.InventoryName;
                IsUse = record.IsUse;
                this.DialogResult = true;
                this.Close();
            }
        }        

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            switch (_inventoryType)
            {
                case 0:
                    Title = "Malzeme Kartı Listesi";
                    break;
                case 1:
                    Title = "Kumaş Kartı Listesi";
                    break;
                case 2:
                    Title = "İplik Kartı Listesi";
                    break;
                default:
                    break;
            }
            //_savedColumnSettings = LoadGridSettings();
            var data = _orm.GetAll<Inventory>("Inventory").Where(x => x.Type == _inventoryType && x.IsPrefix == false).ToList();
            collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = collectionView;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                fgh.InitializeColumnSettings();
                fgh.LoadColumnSettingsFromDatabase();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}