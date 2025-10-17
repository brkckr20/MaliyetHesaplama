using MaliyeHesaplama.helpers;
using Microsoft.Data.SqlClient;
using Stimulsoft.Client.Designer;
using Stimulsoft.Database;
using Stimulsoft.Report;
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
            //RaporDosyasiOlustur();
        }
        void RaporDosyasiOlustur()
        {
            string sourcedir = Path.Combine(Directory.GetCurrentDirectory(), "reports");
            string filepath = Path.Combine(sourcedir, "blank.frx");
            if (File.Exists(filepath))
            {

            }
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
            if (this.Id == 0) // bu kısım sadece eklendi fakat düzenlenmedi. düzenlenecek kontrol edilecek 15.10.2025
            {
                string reportName = txtRaporAdi.Text;
                string sourcePath = "reports/blank.mrt";
                string destPath = $"reports/{reportName}.mrt";

                // Boş şablon dosyasını kopyala
                if (File.Exists(sourcePath))
                {
                    if (!File.Exists(destPath))
                    {
                        File.Copy(sourcePath, destPath);
                    }
                    else
                    {
                        MessageBox.Show("Bu isimde bir rapor zaten var.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("blank.mrt dosyası bulunamadı.");
                    return;
                }

                var reports = _orm.GetReport<dynamic>(reportName);
                string sqlQuery = reports.Query5;

                var config = DbConfig.Load();
                string connectionString = config.ConnectionString;
                DataSet dataSet = new DataSet();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);
                    adapter.Fill(dataSet, "MyData");
                }

                StiReport report = new StiReport();
                report.Load(destPath);
                report.RegData("MyData", dataSet);
                report.Dictionary.Synchronize();

                var designer = new StiDesignerControl
                {
                    Report = report
                };
                var designerWindow = new Window
                {
                    Title = "Yeni Rapor Tasarımı",
                    Content = designer,
                    Width = 1000,
                    Height = 700,
                    WindowState = WindowState.Maximized,
                };
                designerWindow.Closing += DesignerWindow_Closing;
                designerWindow.ShowDialog();
                report.Save(destPath);
            }
            else
            {
                var reports = _orm.GetReport<dynamic>(txtRaporAdi.Text);
                string sqlQuery = reports.Query5;
                var config = DbConfig.Load();
                string connectionString = config.ConnectionString;
                DataSet dataSet = new DataSet();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);
                    adapter.Fill(dataSet, "MyData");
                }
                StiReport report = new StiReport();
                string reportPath = $"reports/{txtRaporAdi.Text}.mrt";
                if (File.Exists(reportPath))
                {
                    report.Load(reportPath);
                }
                else
                {
                    MessageBox.Show("Rapor dosyası bulunamadı.");
                    return;
                }
                report.RegData("MyData", dataSet);
                report.Dictionary.Synchronize();
                var designer = new StiDesignerControl
                {
                    Report = report
                };
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
                designer.Report.Save($"reports/{txtRaporAdi.Text}.mrt");
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

        private void btnOnizle_Click(object sender, RoutedEventArgs e)
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string reportPath = Path.Combine(exePath, "reports", $"{txtRaporAdi.Text}.mrt");

            if (!File.Exists(reportPath))
            {
                MessageBox.Show("Rapor dosyası bulunamadı.");
                return;
            }

            StiReport report = new StiReport();
            report.Load(reportPath);
            var config = DbConfig.Load();
            string connectionString = config.ConnectionString;

            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sql = "select * from Inventory";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(dataSet, "MyData");
            }

            report.RegData("MyData", dataSet);
            report.Dictionary.Synchronize();
            report.Render();

            var viewer = new StiWpfViewerControl
            {
                Report = report
            };

            var viewerWindow = new Window
            {
                Title = "Rapor Önizleme",
                Content = viewer,
                Width = 1000,
                Height = 700
            };

            viewerWindow.ShowDialog();
        }
    }
}
