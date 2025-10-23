using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winRaporSecimi : Window
    {
        string _formName;
        MiniOrm _orm = new MiniOrm();
        public winRaporSecimi(string formName)
        {
            InitializeComponent();
            this._formName = formName;
            SetReportNamesToCombobox();
        }
        void SetReportNamesToCombobox()
        {
            var reports = _orm.GetReportsToUserControl<dynamic>(_formName); //23.10.2025 - buradan devam edilecek
            foreach (var report in reports)
            {
                cmbRaporlar.Items.Add(report.ReportName);
            }
        }
    }
}
