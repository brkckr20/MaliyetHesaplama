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
        public bool YarnDyed;

        string ScreenName = string.Empty;
        string GridName = "gridMalzemeListesi";
        private readonly int CurrentUserId = Properties.Settings.Default.RememberUserId;
        private List<ColumnSelector> _savedColumnSettings;
        private ICollectionView collectionView;
        MiniOrm _orm = new MiniOrm();
        private Dictionary<string, ColumnSelector> _columnSettings;
        public winMalzemeListesi(int InventoryType)
        {
            InitializeComponent();
            _inventoryType = InventoryType;
        }
        private void mColChooser_Click(object sender, RoutedEventArgs e)
        {
            var kolonSecici = new winKolonSecici(ScreenName, CurrentUserId, this.GridName)
            {
                Owner = this,
                Topmost = true,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };
            kolonSecici.Show();
        }
        private Dictionary<string, ColumnSelector> LoadGridSettings()
        {
            return _orm.GetAll<ColumnSelector>("ColumnSelector")
                .Where(x =>
                    x.UserId == CurrentUserId &&
                    x.ScreenName == this.ScreenName &&
                    x.GridName == this.GridName)
                .ToDictionary(x => x.ColumnName, x => x);
        }

        private void LoadData()
        {
            var data = _orm.GetAll<Inventory>("Inventory").Where(x => x.Type == _inventoryType && x.IsPrefix == false).ToList();
            collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = collectionView;
        }
        private void FilterDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var itemType = grid.ItemsSource
                            ?.Cast<object>()
                            ?.FirstOrDefault()
                            ?.GetType();

            if (itemType == null)
                return;

            PropertyInfo propertyInfo = itemType.GetProperty(e.PropertyName);

            if (propertyInfo != null && e.Column is DataGridBoundColumn boundColumn)
            {
                var displayAttr = propertyInfo?.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null && !string.IsNullOrEmpty(displayAttr.Name))
                {
                    e.Column.Header = displayAttr.Name;
                }
                var savedSetting = _savedColumnSettings
                            ?.FirstOrDefault(s => s.ColumnName == e.PropertyName);
                if (savedSetting != null)
                {
                    e.Column.Visibility = savedSetting.Hidden ? Visibility.Collapsed : Visibility.Visible;
                    e.Column.Width = new DataGridLength(savedSetting.Width, DataGridLengthUnitType.Pixel);
                    e.Column.DisplayIndex = savedSetting.Location;
                }
                //else
                //{
                //    Bildirim.Uyari2("savedSetting null");
                //}
            }
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                dynamic record = grid.SelectedItem;
                Id = record.Id;
                Code = record.InventoryCode;
                Name = record.InventoryName;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void SaveGridSettings()
        {
            if (grid.Columns == null || grid.Columns.Count == 0) return;

            var currentSettings = new List<ColumnSelector>();
            var sortedColumns = grid.Columns.OrderBy(c => c.DisplayIndex).ToList();

            foreach (var column in sortedColumns)
            {
                string columnName = string.Empty;
                if (column is DataGridBoundColumn boundColumn && boundColumn.Binding is Binding binding)
                {
                    columnName = binding.Path.Path;
                }

                if (string.IsNullOrEmpty(columnName)) continue;

                var existingSetting = _savedColumnSettings.FirstOrDefault(s => s.ColumnName == columnName);

                currentSettings.Add(new ColumnSelector
                {
                    Id = existingSetting?.Id ?? 0,
                    ColumnName = columnName,
                    Width = (int)column.ActualWidth,
                    Hidden = column.Visibility == Visibility.Collapsed,
                    Location = column.DisplayIndex,
                    UserId = CurrentUserId,
                    ScreenName = this.ScreenName,
                    GridName = this.GridName
                });
            }

            foreach (var setting in currentSettings)
            {
                var data = new Dictionary<string, object>
                {
                    { "Id", setting.Id }, { "ColumnName", setting.ColumnName }, { "UserId", setting.UserId },
                    { "ScreenName", setting.ScreenName }, { "GridName", setting.GridName },
                    { "Hidden", setting.Hidden }, { "Width", setting.Width }, { "Location", setting.Location }
                };
                _orm.Save("ColumnSelector", data);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //SaveGridSettings(); //kolon seçici çalışmadı, baştan kontrol edilecek - 16.12.2025
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            switch (_inventoryType)
            {
                case 1:
                    Title = "Kumaş Kartı Listesi";
                    this.ScreenName = Title;
                    this.GridName = "gridKumasListesi";
                    break;
                case 2:
                    Title = "İplik Kartı Listesi";
                    this.ScreenName = Title;
                    this.GridName = "gridIplikListesi";
                    break;
                default:
                    this.ScreenName = "Malzeme Listesi";
                    this.GridName = "gridVarsayilan";
                    break;
            }
            //_savedColumnSettings = LoadGridSettings();
            var data = _orm.GetAll<Inventory>("Inventory").Where(x => x.Type == _inventoryType && x.IsPrefix == false).ToList();
            collectionView = CollectionViewSource.GetDefaultView(data);
            grid.ItemsSource = collectionView;
        }
    }
}