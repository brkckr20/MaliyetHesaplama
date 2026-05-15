using System.Diagnostics;
using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winRaporSecimi : Window
    {
        string _formName;
        int _kayitNo;
        MiniOrm _orm = new MiniOrm();
        public winRaporSecimi(string formName, int kayitNo)
        {
            InitializeComponent();
            this._formName = formName;
            this._kayitNo = kayitNo;
            SetReportNamesToCombobox();
        }
        void SetReportNamesToCombobox()
        {
            var reports = _orm.GetReportsToUserControl<dynamic>(_formName);
            foreach (var report in reports)
            {
                cmbRaporlar.Items.Add(report.ReportName);
            }
        }

        private void btnIptal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnTamam_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRaporlar.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Lütfen bir rapor seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string reportAppPath = @"C:\\Users\\casper\Desktop\Klasörler\z\ReportApp\bin\Debug\ReportApp.exe";
            try
            {
                Process.Start(reportAppPath, $"{cmbRaporlar.SelectedItem} {_kayitNo}");
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Rapor açılırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
