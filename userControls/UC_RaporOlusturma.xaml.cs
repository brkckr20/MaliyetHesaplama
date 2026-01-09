using FastReport;
using FastReport;
using FastReport.Utils;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using Microsoft.Data.SqlClient;
//using Stimulsoft.Client.Designer;
//using Stimulsoft.Report;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : System.Windows.Controls.UserControl, IPageCommands
    {
        int Id = 0;
        MiniOrm _orm = new MiniOrm();
        private readonly IDbConnection _connection;
        string Rapor1;
        Report report1 = new Report();
        bool IsFastReport = true;
        public UC_RaporOlusturma()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
        }
        private void btnDizayn_Click(object sender, RoutedEventArgs e)
        {
            string reportName = txtRaporAdi.Text;
            string reportPath = $"reports/{reportName}.mrt";
            //string reportPathFast = $"reports/{reportName}.frx";
            string destPath = $"reports/{reportName}.mrt";
            //string destPathFast = $"reports/{reportName}.frx";
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
            //StiReport report = new StiReport();
            //if (File.Exists(reportPath))
            //    //report.Load(reportPath);
            //else
            //{
            //    System.Windows.MessageBox.Show("Rapor dosyası bulunamadı.");
            //    return;
            //}
            //RegDataToReport(reports, dataSet, report);
            //OpenReportDesigner(report);
            //report.Save(destPath);
        }
        //private void OpenReportDesigner(StiReport report)
        //{
        //    var designer = new StiDesignerControl { Report = report };
        //    var designerWindow = new Window
        //    {
        //        Title = "Rapor Tasarımı",
        //        Content = designer,
        //        Width = 1000,
        //        Height = 700,
        //        WindowState = WindowState.Maximized,
        //    };
        //    designerWindow.Closing += DesignerWindow_Closing;
        //    designerWindow.ShowDialog();
        //}
        //public void RegDataToReport(dynamic reports, DataSet dataSet, StiReport report)
        //{
        //    if (!string.IsNullOrWhiteSpace(reports.DataSource1) && dataSet.Tables.Contains(reports.DataSource1))
        //        report.RegData(reports.DataSource1, dataSet.Tables[reports.DataSource1]);

        //    if (!string.IsNullOrWhiteSpace(reports.DataSource2) && dataSet.Tables.Contains(reports.DataSource2))
        //        report.RegData(reports.DataSource2, dataSet.Tables[reports.DataSource2]);

        //    if (!string.IsNullOrWhiteSpace(reports.DataSource3) && dataSet.Tables.Contains(reports.DataSource3))
        //        report.RegData(reports.DataSource3, dataSet.Tables[reports.DataSource3]);

        //    if (!string.IsNullOrWhiteSpace(reports.DataSource4) && dataSet.Tables.Contains(reports.DataSource4))
        //        report.RegData(reports.DataSource4, dataSet.Tables[reports.DataSource4]);

        //    if (!string.IsNullOrWhiteSpace(reports.DataSource5) && dataSet.Tables.Contains(reports.DataSource5))
        //        report.RegData(reports.DataSource5, dataSet.Tables[reports.DataSource5]);

        //    report.Dictionary.Synchronize();
        //}
        public void RegDataToReport1(dynamic reports, DataSet dataSet, Report report)
        {
            RegisterTable(report, dataSet, reports.DataSource1);
            RegisterTable(report, dataSet, reports.DataSource2);
            RegisterTable(report, dataSet, reports.DataSource3);
            RegisterTable(report, dataSet, reports.DataSource4);
            RegisterTable(report, dataSet, reports.DataSource5);
        }
        private void RegisterTable(Report report, DataSet dataSet, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return;

            if (!dataSet.Tables.Contains(tableName))
                return;

            report.RegisterData(dataSet.Tables[tableName], tableName);

            var ds = report.GetDataSource(tableName);
            if (ds != null)
                ds.Enabled = true;
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
            //var designer = window.Content as StiDesignerControl;

            //if (designer.Report.IsModified)
            //{
            //    var result = System.Windows.MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?",
            //                                 "Kaydet", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        string mainWindowFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //        string reportsFolder = Path.Combine(mainWindowFolder, "reports");
            //        if (!Directory.Exists(reportsFolder))
            //            Directory.CreateDirectory(reportsFolder);

            //        string reportFilePath = Path.Combine(reportsFolder, txtRaporAdi.Text + ".mrt");

            //        //designer.Report.Save(reportFilePath);
            //    }
            //    else if (result == MessageBoxResult.Cancel)
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }
        void Temizle()
        {
            txtRaporAdi.Text = string.Empty;
            txtEkranAdi.Text = string.Empty;
            this.Id = 0;
            vkSorgu1.Text = string.Empty;
            vkSorgu2.Text = string.Empty;
            vkSorgu3.Text = string.Empty;
            vkSorgu4.Text = string.Empty;
            vkSorgu5.Text = string.Empty;
            sorgu1edit.Text = string.Empty;
            sorgu2edit.Text = string.Empty;
            sorgu3edit.Text = string.Empty;
            sorgu4edit.Text = string.Empty;
            sorgu5edit.Text = string.Empty;
        }
        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            string reportName = txtRaporAdi.Text;
            string sourcePath = "reports/blank.mrt";
            string sourcePathFast = "reports/blank.frx";
            string destPath = $"reports/{reportName}.mrt";
            string destPathFast = $"reports/{reportName}.frx";

            if (this.Id == 0)
            {
                if (IsFastReport)
                {
                    if (File.Exists(sourcePathFast))
                    {
                        if (!File.Exists(destPathFast))
                            File.Copy(sourcePathFast, destPathFast);
                        else
                        {
                            Bildirim.Bilgilendirme2("Bu isimde bir rapor 'FastReport' dosyası mevcut!");
                            return;
                        }
                    }
                }
                else
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

            }
            var dict = new Dictionary<string, object>
            {
                {"Id",this.Id },{ "FormName", txtEkranAdi.Text },{"ReportName",txtRaporAdi.Text},{"DataSource1",vkSorgu1.Text},{"DataSource2",vkSorgu2.Text},{"DataSource3",vkSorgu3.Text},{"DataSource4",vkSorgu4.Text},{"DataSource5",vkSorgu5.Text},{"Query1",sorgu1edit.Text},{"Query2",sorgu2edit.Text},{"Query3",sorgu3edit.Text},{"Query4",sorgu4edit.Text},{"Query5",sorgu5edit.Text},{"AppId",2}
            };
            this.Id = _orm.Save("Report", dict);
            Bildirim.Bilgilendirme2("Kayıt işlemi tamamlandı.");
        }

        public void Sil()
        {
            if (_orm.Delete("Report", Id, true) > 0)
            {
                string reportName = txtRaporAdi.Text;
                string destPath = $"reports/{reportName}.mrt";
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                    Temizle();
                }
            }
        }

        public void Yazdir()
        {

        }

        public void Ileri()
        {

        }

        public void Geri()
        {

        }

        public void Listele()
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

        private void frTest_Click(object sender, RoutedEventArgs e)
        {
            string reportName = $"\"{txtRaporAdi.Text}\"";
            Process.Start("C:\\Users\\casper\\Desktop\\Klasörler\\z\\ReportApp\\bin\\Debug\\ReportApp.exe", reportName);
        }

        private void frView_Click(object sender, RoutedEventArgs e)
        {
            int id = 1015;
            DataSet ds = new DataSet();

            var config = DbConfig.Load();
            if (config == null)
            {
                System.Windows.MessageBox.Show("Config NULL");
                return;
            }
            if (string.IsNullOrEmpty(config.ConnectionString))
            {
                System.Windows.MessageBox.Show("Connection string boş veya null");
                return;
            }

            string reportPath = @"C:\Users\casper\Desktop\Klasörler\z\MaliyeHesaplama\bin\Debug\net6.0-windows\reports\Renk Kartı Formu.frx";

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(config.ConnectionString);
                System.Windows.MessageBox.Show("Connection nesnesi oluşturuldu");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Connection oluşturulurken hata: " + ex.Message);
                return;
            }

            try
            {
                connection.Open();
                System.Windows.MessageBox.Show("Connection açık ✅");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Connection açılırken hata: " + ex.Message);
                return;
            }

            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand("SELECT * FROM Color WHERE Id = @Id", connection);
                System.Windows.MessageBox.Show("Command nesnesi oluşturuldu");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Command oluşturulurken hata: " + ex.Message);
                return;
            }

            cmd.Parameters.Add(new SqlParameter("@Id", id));

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds, "Renk Kartı");
                    System.Windows.MessageBox.Show($"DS dolduruldu, satır sayısı: {ds.Tables["Renk Kartı"].Rows.Count}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Adapter.Fill hata: " + ex.Message);
                return;
            }

            try
            {
                Report report = new Report();
                report.Load(reportPath);
                report.RegisterData(ds);
                report.GetDataSource("Renk Kartı").Enabled = true;
                report.Show();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Report açılırken hata: " + ex.Message);
            }
        }

    }
}
