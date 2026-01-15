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
            string reportName = $"\"{cmbRaporlar.SelectedValue.ToString()}\"";
            string reportAppPath = @"C:\\Users\\casper\\Desktop\\Klasörler\\z\\ReportApp\\bin\\Debug\\ReportApp.exe";
            Process.Start(reportAppPath, $"{reportName} {_kayitNo}");
        }
    }
}
