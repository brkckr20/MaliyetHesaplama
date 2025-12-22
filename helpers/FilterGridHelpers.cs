using ClosedXML.Excel;
using MaliyeHesaplama.models;
using MaliyeHesaplama.wins;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MaliyeHesaplama.helpers
{
    public class FilterGridHelpers
    {
        private winKolonAyarlari ayarlarWindow;
        private List<ColumnSetting> columnSettings;
        private DataGrid dataGrid;
        private string screenName;
        private string gridName;
        MiniOrm _orm;
        private System.Windows.Threading.DispatcherTimer _saveTimer;
        public FilterGridHelpers(DataGrid grid, string screen, string gridNameParam)
        {
            dataGrid = grid;
            screenName = screen;
            gridName = gridNameParam;
            columnSettings = new List<ColumnSetting>();
            _orm = new MiniOrm();

            // Timer oluştur - genişlik değişikliklerini toplu kaydetmek için
            _saveTimer = new System.Windows.Threading.DispatcherTimer();
            _saveTimer.Interval = TimeSpan.FromMilliseconds(500); // 500ms bekle
            _saveTimer.Tick += (s, e) =>
            {
                _saveTimer.Stop();
                SaveColumnSettingsToDatabase(showMessage: false);
            };
        }
        public void SetColumnSettings(List<ColumnSetting> settings)
        {
            columnSettings = settings;
        }
        public void GridGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e, FilterDataGrid.FilterDataGrid grid, string[] hiddenCols)
        {
            var hiddenColumns = new[] { "InsertedBy", "InsertedDate", "UpdatedBy", "UpdatedDate", "RecipeId", "Type", "ProductImage", "CompanyId", "InventoryId" };
            if (hiddenCols.Contains(e.PropertyName))
            {
                e.Cancel = true;
            }
            var itemType = grid.ItemsSource?.Cast<object>()?.FirstOrDefault()?.GetType();
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
            }
        }
        public void OpenColumnsForm(Window window)
        {
            if (columnSettings == null || !columnSettings.Any())
            {
                Bildirim.Uyari2("Kolon ayarları henüz yüklenmedi!");
                return;
            }

            var currentSettings = columnSettings.Select(s =>
                new ColumnSetting(s.ColumnName, s.Hidden)).ToList();

            ayarlarWindow = new winKolonAyarlari(currentSettings, window);
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
        public void InitializeColumnSettings()
        {
            columnSettings.Clear();

            int location = 0;
            foreach (var column in dataGrid.Columns)
            {
                columnSettings.Add(new ColumnSetting
                {
                    ColumnName = column.Header?.ToString() ?? $"Column{location}",
                    Hidden = column.Visibility != Visibility.Visible,
                    Width = (int)column.ActualWidth,
                    Location = location++,
                    UserId = Properties.Settings.Default.RememberUserId,
                    ScreenName = screenName,
                    GridName = gridName
                });
            }

            // Event handler'ları bağla
            AttachColumnEvents();
        }
        private void ApplyColumnSettings()
        {
            var sortedSettings = columnSettings.OrderBy(s => s.Location).ToList();

            for (int i = 0; i < sortedSettings.Count; i++)
            {
                var setting = sortedSettings[i];
                var column = dataGrid.Columns
                    .FirstOrDefault(c => c.Header.ToString() == setting.ColumnName);

                if (column != null)
                {
                    column.Visibility = setting.Hidden
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                    int currentIndex = dataGrid.Columns.IndexOf(column);
                    if (currentIndex != i)
                    {
                        column.DisplayIndex = i;
                    }
                    if (setting.Width.HasValue && setting.Width.Value > 50)
                    {
                        column.Width = new DataGridLength(setting.Width.Value);
                    }
                    else
                    {
                        column.Width = DataGridLength.Auto;
                    }
                }
            }
        }

        public void LoadColumnSettingsFromDatabase()
        {
            if (columnSettings == null || !columnSettings.Any())
                return;

            try
            {
                var orm = new MiniOrm();
                var userId = Properties.Settings.Default.RememberUserId;

                var savedSettings = orm.GetAll<ColumnSetting>("ColumnSelector")
                    .Where(s => s.ScreenName == screenName && 
                                s.GridName == gridName && 
                                s.UserId == userId)
                    .ToList();

                if (savedSettings.Any())
                {
                    foreach (var setting in columnSettings)
                    {
                        var savedSetting = savedSettings.FirstOrDefault(s => 
                            s.ColumnName == setting.ColumnName);

                        if (savedSetting != null)
                        {
                            setting.Id = savedSetting.Id;
                            setting.Hidden = savedSetting.Hidden;
                            setting.Width = savedSetting.Width;
                            setting.Location = savedSetting.Location;
                        }
                    }
                    ApplyColumnSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Kolon ayarları yüklenemedi: {ex.Message}");
            }
        }
        void SaveColumnSettingsToDatabase(bool showMessage = true)
        {
            try
            {
                // Önce bu kullanıcı ve ekran için mevcut kayıtları al
                var existingSettings = _orm.GetAll<ColumnSetting>("ColumnSelector")
                    .Where(s => s.UserId == Properties.Settings.Default.RememberUserId
                             && s.ScreenName == screenName
                             && s.GridName == gridName)
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
                    Bildirim.Uyari2($"Ayarlar kaydedilirken hata oluştu: {ex.Message}");
                }
            }
        }
        public void GridReOrdered(object sender, DataGridColumnEventArgs e)
        {
            var orderedColumns = dataGrid.Columns.OrderBy(c => c.DisplayIndex).ToList();

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
                    .FirstOrDefault(s => s.ColumnName == column.Header?.ToString());

                if (setting != null && setting.Width != (int)column.ActualWidth)
                {
                    setting.Width = (int)column.ActualWidth;
                    
                    // Timer'ı sıfırla ve yeniden başlat
                    _saveTimer.Stop();
                    _saveTimer.Start();
                }
            }
        }
        private void AttachColumnEvents()
        {
            foreach (var column in dataGrid.Columns)
            {
                column.Width = new DataGridLength(column.ActualWidth);
                var dpd = DependencyPropertyDescriptor.FromProperty(
                    DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));

                dpd?.AddValueChanged(column, Column_WidthChanged);
            }
        }
        public void ExportToExcel()
        {
            try
            {
                if (dataGrid == null || dataGrid.Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("Dışarı aktarılacak veri bulunamadı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                    FileName = $"{screenName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sayfa1");
                        
                        // Başlıkları ekle
                        int colIndex = 1;
                        var visibleColumns = dataGrid.Columns
                            .Where(c => c.Visibility == Visibility.Visible)
                            .OrderBy(c => c.DisplayIndex)
                            .ToList();

                        foreach (var column in visibleColumns)
                        {
                            worksheet.Cell(1, colIndex).Value = column.Header?.ToString() ?? "";
                            worksheet.Cell(1, colIndex).Style.Font.Bold = true;
                            worksheet.Cell(1, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                            colIndex++;
                        }
                        int rowIndex = 2;
                        foreach (var item in dataGrid.Items)
                        {
                            colIndex = 1;
                            foreach (var column in visibleColumns)
                            {
                                if (column is DataGridBoundColumn boundColumn)
                                {
                                    var binding = boundColumn.Binding as System.Windows.Data.Binding;
                                    if (binding != null)
                                    {
                                        var value = GetPropertyValue(item, binding.Path.Path);
                                        
                                        var cell = worksheet.Cell(rowIndex, colIndex);

                                        if (value is DateTime dateValue)
                                        {
                                            cell.Value = dateValue;
                                            cell.Style.DateFormat.Format = "dd.MM.yyyy";
                                        }
                                        else if (value is decimal || value is double || value is float)
                                        {
                                             cell.Value = Convert.ToDouble(value);
                                        }
                                        else if (value is int || value is long || value is short)
                                        {
                                             cell.Value = Convert.ToInt64(value);
                                        }
                                        else
                                        {
                                            cell.Value = value?.ToString() ?? "";
                                        }
                                    }
                                }
                                colIndex++;
                            }
                            rowIndex++;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(saveFileDialog.FileName);
                        
                        Bildirim.Bilgilendirme2("Excel'e aktarım başarıyla tamamlandı.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Excel'e aktarılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private object GetPropertyValue(object item, string propertyPath)
        {
            if (item == null) return null;
            
            try 
            {
                var property = item.GetType().GetProperty(propertyPath);
                return property?.GetValue(item, null);
            }
            catch
            {
                return null;
            }
        }
    }
}
