using FastReport;
using FastReport.Design.StandardDesigner;
using MaliyeHesaplama.helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Controls;
using FastReport.Design;
using FastReport.Preview;


namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : UserControl
    {
        int Id = 1;
        Report report = new Report();
        MiniOrm _orm = new MiniOrm();
        private readonly IDbConnection _connection;
        public UC_RaporOlusturma()
        {
            InitializeComponent();
            RaporDosyasiOlustur();            
        }
        void RaporDosyasiOlustur() // rapor oluşturma işlemlerinden devam edilecek --
            // raporlamada patladık :( 13.10.2025
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
            try
            {
                string dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "reports");
                string filepath = Path.Combine(dosyaYolu, raporName + ".frx");
                report.Load(filepath);
                //string Rapor1, Rapor2, Rapor3, Rapor4;
                //var reports = _orm.GetReport<dynamic>(raporName);
                //Rapor1 = reports.Query1;
                //Rapor2 = reports.Query2;
                //Rapor3 = reports.Query3;
                //Rapor4 = reports.Query4;
                if (isDesing)
                {
                    //DesignerForm form = new DesignerForm();
                    //form.Designer
                }
                else
                {
                    report.Show();
                }
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2("Hata : " + ex.Message);
            }
        }
        private void btnDizayn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.Id != 0)
            {
                DizaynAc("blank", true, 0);
            }
            else
            {
                string newReportName = txtRaporAdi.Text.Trim();
                if (string.IsNullOrEmpty(newReportName))
                {
                    Bildirim.Uyari2("Rapor adı boş bırakılamaz!!");
                }
            }
        }
    }
}
