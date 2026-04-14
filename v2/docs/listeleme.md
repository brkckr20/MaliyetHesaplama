# v2 Liste Ekranları Kural ve Yapıları

## Model Kuralları

Model class'larında `[Display(Name = "...")]` attribute'u kullanılmalı.

```csharp
[Display(Name = "Kodu")]
public string Code { get; set; }

[Display(Name = "Adı")]
public string Name { get; set; }
```

## Repository Yapısı

`dynamic` yerine **Model tipi** kullanılmalı:

```csharp
public IEnumerable<MaterialMaster> GetAll()
{
    return _orm.GetAll<MaterialMaster>("MaterialMaster");
}
```

## Liste Ekranı Yapısı

### Code-Behind

```csharp
FilterGridHelpers fgh;

public winMalzemeListesiV2()
{
    InitializeComponent();
    _repo = new MaterialRepository();
    fgh = new FilterGridHelpers(grid, "Malzeme Listesi", "gridMalzemeListe");
}

private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
{
    var hiddenColumns = new[] { "CategoryId", "CreatedAt", "UpdatedAt" };
    fgh.GridGeneratingColumn(e, grid, hiddenColumns);
}

private void Window_Loaded(object sender, RoutedEventArgs e)
{
    var data = _repo.GetAll().ToList();
    _collectionView = CollectionViewSource.GetDefaultView(data);
    grid.ItemsSource = _collectionView;
    Dispatcher.BeginInvoke(new Action(() =>
    {
        fgh.InitializeColumnSettings();
        fgh.LoadColumnSettingsFromDatabase();
    }), System.Windows.Threading.DispatcherPriority.Loaded);
}
```

### XAML

```xml
<control:FilterDataGrid Grid.Column="0" FontSize="12"
    VerticalContentAlignment="Center" IsReadOnly="True" GridLinesVisibility="All"
    FilterLanguage="Turkish" ShowStatusBar="True" ShowElapsedTime="True"
    AutoGenerateColumns="True" AutoGeneratingColumn="grid_AutoGeneratingColumn"
    x:Name="grid" MouseDoubleClick="sfDataGrid_MouseDoubleClick">
    <control:FilterDataGrid.ContextMenu>
        <ContextMenu>
            <MenuItem Header="Kolon Seçicisi" Click="MenuItem_Click"/>
        </ContextMenu>
    </control:FilterDataGrid.ContextMenu>
</control:FilterDataGrid>
```

## Önemli Noktalar

1. Her model için `[Display(Name = "...")]` eklenmeli
2. Repository'de `dynamic` yerine model tipi kullanılmalı
3. Liste ekranlarında `FilterGridHelpers` kullanılmalı
4. Gizlenecek kolonlar model'de olmayanlar olmamalı
5. Window_Loaded'da Dispatcher ile kolon ayarları yüklenmeli