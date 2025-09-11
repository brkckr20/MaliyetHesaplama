using Syncfusion.UI.Xaml.Grid;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class UC_FirmaKarti : UserControl
    {
        private MiniOrm _orm;
        private int Id = 0;
        public UC_FirmaKarti()
        {
            InitializeComponent();
            _orm = new MiniOrm();
            sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Company");
            sfDataGrid.Loaded += (s, e) =>
            {
                var idColumn = sfDataGrid.Columns.FirstOrDefault(c => c.MappingName == "Id");
                if (idColumn != null)
                    idColumn.IsHidden = true;
            };
            KolonlaraTurkceIsımVer();
            //gridin otomatik olarak ekranın tamamını kaplaması için gerekli ayarlamalar yapılacak --> 11.09.2025
        }
        private void btnKayit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object>
            {
                { "Id",Id },
                {"CompanyCode", txtFirmaKodu.Text },
                {"CompanyName", txtFirmaUnvan.Text },
                {"AddressLine1", txtAdres1.Text },
                {"AddressLine2", txtAdres2.Text },
                {"AddressLine3", txtAdres3.Text },
            };
            Id = _orm.Save("Company", dict);
        }

        void KolonlaraTurkceIsımVer()
        {
            sfDataGrid.AutoGeneratingColumn += (s, e) =>
            {
                switch (e.Column.MappingName)
                {
                    case "CompanyCode":
                        e.Column.HeaderText = "Firma Kodu";
                        break;
                    case "CompanyName":
                        e.Column.HeaderText = "Firma Adı";
                        break;
                    case "AdressLine1":
                        e.Column.HeaderText = "Adres 1";
                        break;
                    case "AdressLine2":
                        e.Column.HeaderText = "Adres 2";
                        break;
                    case "AdressLine3":
                        e.Column.HeaderText = "Adres 3";
                        break;
                }
            };
        }
        private void sfDataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (sfDataGrid.SelectedItem != null)
            {
                dynamic row = sfDataGrid.SelectedItem;
                if (row != null)
                {
                    Id = Convert.ToInt32(row.Id);
                    txtFirmaKodu.Text = row.CompanyCode?.ToString() ?? string.Empty;
                    txtFirmaUnvan.Text = row.CompanyName?.ToString() ?? string.Empty;
                    txtAdres1.Text = row.AddressLine1?.ToString() ?? string.Empty;
                    txtAdres2.Text = row.AddressLine2?.ToString() ?? string.Empty;
                    txtAdres3.Text = row.AddressLine3?.ToString() ?? string.Empty;
                }
            }
        }

        private void btnSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Kayıt silinecek. Emin misiniz?\nBu işlem geri alınamaz", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (_orm.Delete("Company", Id) > 0)
                {
                    MessageBox.Show("Silme işlemi başarılı", "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
                    sfDataGrid.ItemsSource = _orm.GetAll<dynamic>("Company");
                }
            }
        }
    }
}
