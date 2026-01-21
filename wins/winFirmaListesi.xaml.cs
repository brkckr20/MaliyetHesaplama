using MaliyeHesaplama.models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        private List<ColumnSetting> columnSettings;
        private const string SCREEN_NAME = "Firma Listesi";
        private const string GRID_NAME = "gridFirma";
        private int currentUserId = Properties.Settings.Default.RememberUserId;
        private winKolonAyarlari ayarlarWindow;

        public winFirmaListesi()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var source = _orm.GetAll<Company>("Company").Where(z => z.IsOwner == false);// şirket kartlarına göre düzenlenecek - şuan için sadece IsOwner false olanlar listelendi
            grid.ItemsSource = source;
            InitializeColumnSettings();
            LoadColumnSettingsFromDatabase();
            AttachColumnEvents();
        }
        private void AttachColumnEvents()
        {
            foreach (var column in grid.Columns)
            {
                column.Width = new DataGridLength(column.ActualWidth);
                var dpd = DependencyPropertyDescriptor.FromProperty(
                    DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));

                dpd?.AddValueChanged(column, Column_WidthChanged);
            }

            // Kolon sırası değişikliğini dinle
            grid.ColumnReordered += Grid_ColumnReordered;
        }
        private void Grid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            var orderedColumns = grid.Columns.OrderBy(c => c.DisplayIndex).ToList();

            for (int i = 0; i < orderedColumns.Count; i++)
            {
                var column = orderedColumns[i];
                var setting = columnSettings
                    .FirstOrDefault(s => s.ColumnName == column.Header.ToString());

                if (setting != null)
                {
                    setting.Location = i;
                }
            }
            SaveColumnSettingsToDatabase(showMessage: false);
        }
        private void Column_WidthChanged(object sender, EventArgs e)
        {
            var column = sender as DataGridColumn;
            if (column != null)
            {
                var setting = columnSettings
                    .FirstOrDefault(s => s.ColumnName == column.Header.ToString());

                if (setting != null && setting.Width != (int)column.ActualWidth)
                {
                    setting.Width = (int)column.ActualWidth;
                    SaveColumnSettingsToDatabase(showMessage: false);
                }
            }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var currentSettings = columnSettings.Select(s =>
                new ColumnSetting(s.ColumnName, s.Hidden)).ToList();

            ayarlarWindow = new winKolonAyarlari(currentSettings, this);
            ayarlarWindow.Closed += AyarlarWindow_Closed;
            ayarlarWindow.Show();
        }
        private void AyarlarWindow_Closed(object sender, EventArgs e)
        {
            if (ayarlarWindow.SettingsSaved)
            {
                // Kullanıcı kaydet'e bastı - Hidden bilgisini güncelle
                for (int i = 0; i < columnSettings.Count; i++)
                {
                    var updatedSetting = ayarlarWindow.ColumnSettings
                        .FirstOrDefault(s => s.ColumnName == columnSettings[i].ColumnName);

                    if (updatedSetting != null)
                    {
                        columnSettings[i].Hidden = updatedSetting.Hidden;
                    }
                }

                ApplyColumnSettings();
                SaveColumnSettingsToDatabase();
            }

            ayarlarWindow = null;
        }

        private void ApplyColumnSettings()
        {
            var sortedSettings = columnSettings.OrderBy(s => s.Location).ToList();

            for (int i = 0; i < sortedSettings.Count; i++)
            {
                var setting = sortedSettings[i];
                var column = grid.Columns
                    .FirstOrDefault(c => c.Header.ToString() == setting.ColumnName);

                if (column != null)
                {
                    // Görünürlük
                    column.Visibility = setting.Hidden
                        ? Visibility.Collapsed
                        : Visibility.Visible;

                    // Sıralama - DisplayIndex ile
                    int currentIndex = grid.Columns.IndexOf(column);
                    if (currentIndex != i)
                    {
                        column.DisplayIndex = i;
                    }

                    // Genişlik
                    if (setting.Width.HasValue && setting.Width.Value > 50) // Minimum 50 pixel
                    {
                        column.Width = new DataGridLength(setting.Width.Value);
                    }
                    else
                    {
                        // Varsayılan genişlik - içeriğe göre otomatik
                        column.Width = DataGridLength.Auto;

                        // Veya sabit bir genişlik vermek isterseniz:
                        // column.Width = new DataGridLength(150);
                    }
                }
            }
        }

        private void SaveColumnSettingsToDatabase(bool showMessage = true)
        {
            try
            {
                // Önce bu kullanıcı ve ekran için mevcut kayıtları al
                var existingSettings = _orm.GetAll<ColumnSetting>("ColumnSelector")
                    .Where(s => s.UserId == currentUserId
                             && s.ScreenName == SCREEN_NAME
                             && s.GridName == GRID_NAME)
                    .ToList();

                foreach (var setting in columnSettings)
                {
                    // Bu kolon için kayıt var mı kontrol et
                    var existing = existingSettings
                        .FirstOrDefault(e => e.ColumnName == setting.ColumnName);

                    var data = new Dictionary<string, object>
                    {
                        { "ColumnName", setting.ColumnName },
                        { "Width", setting.Width },
                        { "Hidden", setting.Hidden },
                        { "Location", setting.Location },
                        { "UserId", setting.UserId },
                        { "ScreenName", setting.ScreenName },
                        { "GridName", setting.GridName }
                    };

                    if (existing != null)
                    {
                        // Güncelleme - Id'yi ekle
                        data["Id"] = existing.Id;
                    }
                    else
                    {
                        // Yeni kayıt - Id = 0 (IDENTITY için)
                        data["Id"] = 0;
                    }

                    _orm.Save("ColumnSelector", data);
                }

                //if (showMessage)
                //{
                //    MessageBox.Show("Kolon ayarları başarıyla kaydedildi!", "Bilgi",
                //        MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }
            catch (System.Exception ex)
            {
                if (showMessage)
                {
                    System.Windows.MessageBox.Show($"Ayarlar kaydedilirken hata oluştu: {ex.Message}",
                        "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void LoadColumnSettingsFromDatabase()
        {
            try
            {
                var savedSettings = _orm.GetAll<ColumnSetting>("ColumnSelector")
                    .Where(s => s.UserId == currentUserId
                             && s.ScreenName == SCREEN_NAME
                             && s.GridName == GRID_NAME)
                    .OrderBy(s => s.Location)
                    .ToList();

                if (savedSettings.Any())
                {
                    // Kaydedilmiş ayarları uygula
                    foreach (var saved in savedSettings)
                    {
                        var setting = columnSettings
                            .FirstOrDefault(s => s.ColumnName == saved.ColumnName);

                        if (setting != null)
                        {
                            setting.Hidden = saved.Hidden;
                            setting.Width = saved.Width;
                            setting.Location = saved.Location ?? 0;
                        }
                    }

                    // Ayarları uygula
                    Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        ApplyColumnSettings();
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
                else
                {
                    // Yeni kullanıcı - varsayılan genişlikleri ayarla
                    Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        SetDefaultColumnWidths();
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
            }
            catch
            {
                // Hata durumunda varsayılan ayarlarla devam et
            }
        }
        private void SetDefaultColumnWidths()
        {
            foreach (var column in grid.Columns)
            {
                column.Width = DataGridLength.Auto;
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
    }
}