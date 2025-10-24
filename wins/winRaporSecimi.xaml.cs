using MaliyeHesaplama.userControls;
using Microsoft.Data.SqlClient;
using Stimulsoft.Report;
using Stimulsoft.Report.Viewer;
using System.Data;
using System.IO;
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
        UC_RaporOlusturma ro = new UC_RaporOlusturma();
        private void btnTamam_Click(object sender, RoutedEventArgs e)
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string reportPath = Path.Combine(exePath, "reports", $"{cmbRaporlar.SelectedValue}.mrt");
            ro.GoruntulenecekId = _kayitNo;
            StiReport report = new StiReport();
            report.Load(reportPath);
            var config = DbConfig.Load();
            string connectionString = config.ConnectionString;
            var reports = _orm.GetReport<dynamic>(cmbRaporlar.SelectedValue.ToString());
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ro.FillDataSetWithQuery(reports.Query1, reports.DataSource1, connection, dataSet);
                ro.FillDataSetWithQuery(reports.Query2, reports.DataSource2, connection, dataSet);
                ro.FillDataSetWithQuery(reports.Query3, reports.DataSource3, connection, dataSet);
                ro.FillDataSetWithQuery(reports.Query4, reports.DataSource4, connection, dataSet);
                ro.FillDataSetWithQuery(reports.Query5, reports.DataSource5, connection, dataSet);
            }

            ro.RegDataToReport(reports, dataSet, report);
            report.Dictionary.Synchronize();
            report.Render();

            var viewer = new StiWpfViewerControl
            {
                Report = report
            };

            var viewerWindow = new Window
            {
                Title = "Rapor Önizleme [ " + cmbRaporlar.SelectedValue.ToString() + " ]",
                Content = viewer,
                Width = 1000,
                Height = 700
            };

            viewerWindow.ShowDialog();
        }
    }
}
