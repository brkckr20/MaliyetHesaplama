using MaliyeHesaplama.helpers;
using Microsoft.Data.SqlClient;
using Stimulsoft.Client.Designer;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : UserControl
    {
        int Id = 0;
        MiniOrm _orm = new MiniOrm();
        private readonly IDbConnection _connection;
        string Rapor1;
        public UC_RaporOlusturma()
        {
            InitializeComponent();
        }

        private void btnYeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGeri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIleri_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnKayit_Click(object sender, RoutedEventArgs e)
        {
            string reportName = txtRaporAdi.Text;
            string sourcePath = "reports/blank.mrt";
            string destPath = $"reports/{reportName}.mrt";
            if (this.Id == 0)
            {
                if (File.Exists(sourcePath))
                {
                    if (!File.Exists(destPath))
                        File.Copy(sourcePath, destPath);
                    else
                    {
                        Bildirim.Bilgilendirme2("Bu isimde bir rapor mevcut!");
                        return;
                    }
                }
            }
            var dict = new Dictionary<string, object>
            {
                {"Id",this.Id },{ "FormName", txtEkranAdi.Text },{"ReportName",txtRaporAdi.Text},{"DataSource1",vkSorgu1.Text},{"DataSource2",vkSorgu2.Text},{"DataSource3",vkSorgu3.Text},{"DataSource4",vkSorgu4.Text},{"DataSource5",vkSorgu5.Text},{"Query1",sorgu1edit.Text},{"Query2",sorgu2edit.Text},{"Query3",sorgu3edit.Text},{"Query4",sorgu4edit.Text},{"Query5",sorgu5edit.Text}
            };
            this.Id = _orm.Save("Report", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi tamamlandı.");
        }
        private void btnListe_Click(object sender, RoutedEventArgs e)
        {
            wins.winRaporListesi win = new wins.winRaporListesi();
            win.ShowDialog();
            if (win.IsSelectRow)
            {
                this.Id = win.Id;
                txtRaporAdi.Text = win.ReportName;
                txtEkranAdi.Text = win.FormName;
                sorgu1edit.Text = win.Query1;
                sorgu2edit.Text = win.Query2;
                sorgu3edit.Text = win.Query3;
                sorgu4edit.Text = win.Query4;
                sorgu5edit.Text = win.Query5;
                vkSorgu1.Text = win.DataSource1;
                vkSorgu2.Text = win.DataSource2;
                vkSorgu3.Text = win.DataSource3;
                vkSorgu4.Text = win.DataSource4;
                vkSorgu5.Text = win.DataSource5;
            }
        }
        private void btnDizayn_Click(object sender, RoutedEventArgs e)
        {
            string reportName = txtRaporAdi.Text;
            string reportPath = $"reports/{reportName}.mrt";
            string destPath = $"reports/{reportName}.mrt";

            var reports = _orm.GetReport<dynamic>(reportName);
            var config = DbConfig.Load();
            string connectionString = config.ConnectionString;
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                FillDataSetWithQuery(reports.Query1, reports.DataSource1, connection, dataSet);
                FillDataSetWithQuery(reports.Query2, reports.DataSource2, connection, dataSet);
                FillDataSetWithQuery(reports.Query3, reports.DataSource3, connection, dataSet);
                FillDataSetWithQuery(reports.Query4, reports.DataSource4, connection, dataSet);
                FillDataSetWithQuery(reports.Query5, reports.DataSource5, connection, dataSet);
            }

            StiReport report = new StiReport();
            if (File.Exists(reportPath))
                report.Load(reportPath);
            else
            {
                MessageBox.Show("Rapor dosyası bulunamadı.");
                return;
            }
            RegDataToReport(reports, dataSet, report);
            OpenReportDesigner(report);
            report.Save(destPath);
        }
        private void OpenReportDesigner(StiReport report)
        {
            var designer = new StiDesignerControl { Report = report };
            var designerWindow = new Window
            {
                Title = "Rapor Tasarımı",
                Content = designer,
                Width = 1000,
                Height = 700,
                WindowState = WindowState.Maximized,
            };
            designerWindow.Closing += DesignerWindow_Closing;
            designerWindow.ShowDialog();
        }
        public void RegDataToReport(dynamic reports, DataSet dataSet, StiReport report)
        {
            if (!string.IsNullOrWhiteSpace(reports.DataSource1) && dataSet.Tables.Contains(reports.DataSource1))
                report.RegData(reports.DataSource1, dataSet.Tables[reports.DataSource1]);

            if (!string.IsNullOrWhiteSpace(reports.DataSource2) && dataSet.Tables.Contains(reports.DataSource2))
                report.RegData(reports.DataSource2, dataSet.Tables[reports.DataSource2]);

            if (!string.IsNullOrWhiteSpace(reports.DataSource3) && dataSet.Tables.Contains(reports.DataSource3))
                report.RegData(reports.DataSource3, dataSet.Tables[reports.DataSource3]);

            if (!string.IsNullOrWhiteSpace(reports.DataSource4) && dataSet.Tables.Contains(reports.DataSource4))
                report.RegData(reports.DataSource4, dataSet.Tables[reports.DataSource4]);

            if (!string.IsNullOrWhiteSpace(reports.DataSource5) && dataSet.Tables.Contains(reports.DataSource5))
                report.RegData(reports.DataSource5, dataSet.Tables[reports.DataSource5]);

            report.Dictionary.Synchronize();
        }
        bool IsDesign = true; // kayıt işlemi tamamlandı. command.Parameters.AddWithValue("@Id", 1); id alanı olmazsa hata verdi ve dizaynda default 1 verildi
        public int GoruntulenecekId = 7;
        public void FillDataSetWithQuery(string query, string dataSource, SqlConnection connection, DataSet dataSet)
        {
            if (IsDesign)
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", GoruntulenecekId);
                    new SqlDataAdapter(command).Fill(dataSet, dataSource);
                }
            }
        }
        private void DesignerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = sender as Window;
            var designer = window.Content as StiDesignerControl;

            if (designer.Report.IsModified)
            {
                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?",
                                             "Kaydet", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string mainWindowFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string reportsFolder = Path.Combine(mainWindowFolder, "reports");
                    if (!Directory.Exists(reportsFolder))
                        Directory.CreateDirectory(reportsFolder);

                    string reportFilePath = Path.Combine(reportsFolder, txtRaporAdi.Text + ".mrt");

                    designer.Report.Save(reportFilePath);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
        
    }
}
