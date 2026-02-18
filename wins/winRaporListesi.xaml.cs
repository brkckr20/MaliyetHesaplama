using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;

namespace MaliyeHesaplama.wins
{
    public partial class winRaporListesi : Window
    {
        MiniOrm _orm = new MiniOrm();
        //private ICollectionView _collectionView;
        public string ReportName, FormName, Query1, Query2, Query3, Query4, Query5, DataSource1, DataSource2, DataSource3, DataSource4, DataSource5;

        private List<ColumnSetting> columnSettings;
        private const string SCREEN_NAME = "Rapor Listesi";
        private const string GRID_NAME = "gridRaporListesi";
        private int currentUserId = Properties.Settings.Default.RememberUserId;
        //private winKolonAyarlari ayarlarWindow;
        FilterGridHelpers fgh;

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var hiddenColumns = new[] { "Query1", "Query2", "Query3", "Query4", "Query5", "DataSource1", "DataSource2", "DataSource3", "DataSource4", "DataSource5", "FormGroup", "AppId" };
            fgh.GridGeneratingColumn(e, grid, hiddenColumns);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        public int Id;
        public bool IsSelectRow = false;
        public winRaporListesi()
        {
            InitializeComponent();
            fgh = new FilterGridHelpers(grid, "Form Listesi", "grid");
        }

        private void sfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                IsSelectRow = true;
                dynamic record = grid.SelectedItem;
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
            var source = _orm.GetAll<Report>("Report").Where(x => x.AppId == 2).ToList();
            grid.ItemsSource = source;
            //InitializeColumnSettings();
        }
        private void InitializeColumnSettings()
        {
            columnSettings = new List<ColumnSetting>();
            int location = 0;
            foreach (var column in grid.Columns)
            {
                columnSettings.Add(new ColumnSetting
                {
                    ColumnName = column.Header.ToString(),
                    Hidden = column.Visibility != Visibility.Visible,
                    Width = (int)column.ActualWidth,
                    Location = location++,
                    UserId = currentUserId,
                    ScreenName = SCREEN_NAME,
                    GridName = GRID_NAME
                });
            }
        }
    }
}
