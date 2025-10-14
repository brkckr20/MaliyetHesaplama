using MaliyeHesaplama.helpers;
using Microsoft.Data.SqlClient;
using Stimulsoft.Client.Designer;
using Stimulsoft.Report;
using Stimulsoft.Report.Viewer;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static Stimulsoft.Report.Help.StiHelpProvider;


namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : UserControl
    {
        int Id = 1;
        MiniOrm _orm = new MiniOrm();
        private readonly IDbConnection _connection;
        string Rapor1;
        public UC_RaporOlusturma()
        {
            InitializeComponent();
            RaporDosyasiOlustur();            
        }
        void RaporDosyasiOlustur() // rapor oluşturma işlemlerinden devam edilecek --
            // code renklendirme sql için indir ve kullan. ardından listele ve ilgili sorguyu yeniden düzenleme adımlarına devam et. 14.10.2025
        {
            string sourcedir = Path.Combine(Directory.GetCurrentDirectory(), "reports");
            string filepath = Path.Combine(sourcedir, "blank.frx");
            if (File.Exists(filepath))
            {

            }
        }

        private void btnYeni_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnGeri_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnIleri_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnSil_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnKayit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnListe_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        //public DataSet goster()
        //{
        //    DataSet ds = new DataSet();
        //    var config = DbConfig.Load();
        //    if (config.DbType == "MSSQL")
        //    {
        //        _connection = new SqlConnection(config.ConnectionString);
        //    }
        //}
        public string TabloAdiniAl(string sorgu)
        {
            var match = System.Text.RegularExpressions.Regex.Match(sorgu, @"FROM\s+(\w+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }
        public void DizaynAc(string raporName, bool isDesing, int kayitNumarasi)
        {
            //try
            //{
            //    string dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "reports");
            //    string filepath = Path.Combine(dosyaYolu, raporName + ".frx");
            //    report.Load(filepath);
            //    //string Rapor1, Rapor2, Rapor3, Rapor4;
            //    //var reports = _orm.GetReport<dynamic>(raporName);
            //    //Rapor1 = reports.Query1;
            //    //Rapor2 = reports.Query2;
            //    //Rapor3 = reports.Query3;
            //    //Rapor4 = reports.Query4;
            //    if (isDesing)
            //    {
            //        //DesignerForm form = new DesignerForm();
            //        //form.Designer
            //    }
            //    else
            //    {
            //        report.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Bildirim.Uyari2("Hata : " + ex.Message);
            //}
        }
        private void btnDizayn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var reports = _orm.GetReport<dynamic>("Maliyet Formu");
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
            report.RegData("MyData",dataSet);
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
                Height = 700
            };
            designerWindow.Closing += DesignerWindow_Closing;
            designerWindow.ShowDialog();
            designer.Report.Save("reports/MaliyetFormu.mrt");
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

                    string reportFilePath = Path.Combine(reportsFolder, "MaliyetFormu.mrt");

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
            string reportPath = Path.Combine(exePath, "reports", "MaliyetFormu.mrt");

            if (!File.Exists(reportPath))
            {
                MessageBox.Show("Rapor dosyası bulunamadı.");
                return;
            }

            StiReport report = new StiReport();
            report.Load(reportPath);

            // İsteğe bağlı: SQL varsa ve dışarıdan veri bağlanacaksa
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
