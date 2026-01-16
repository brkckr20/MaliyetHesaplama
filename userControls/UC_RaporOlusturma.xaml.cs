using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Windows;


namespace MaliyeHesaplama.userControls
{
    public partial class UC_RaporOlusturma : System.Windows.Controls.UserControl, IPageCommands
    {
        int Id = 0;
        MiniOrm _orm = new MiniOrm();
        string reportAppPath = @"C:\\Users\\casper\\Desktop\\Klasörler\\z\\ReportApp\\bin\\Debug\\ReportApp.exe";
        string sourceFilePath = @"C:\\Users\\casper\\Desktop\\Klasörler\\z\\ReportApp\\bin\\Debug\\report";
        public UC_RaporOlusturma()
        {
            InitializeComponent();
            ButtonBar.PageCommands = this;
            ButtonBar.btnIleri.IsEnabled = false;
            ButtonBar.btnGeri.IsEnabled = false;
            ButtonBar.btnYazdir.IsEnabled = false;
        }
        private void btnDizayn_Click(object sender, RoutedEventArgs e)
        {
            string reportName = $"\"{txtRaporAdi.Text}\"";
            Process.Start(reportAppPath, reportName);
        }
        public int GoruntulenecekId = 7;

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
            string sourcePath = sourceFilePath + "\\blank.frx";
            string destPath = sourceFilePath + $"\\{reportName}.frx";
            if (this.Id == 0)
            {
                if (File.Exists(sourcePath))
                {
                    if (!File.Exists(destPath))
                        File.Copy(sourcePath, destPath);
                    else
                    {
                        Bildirim.Bilgilendirme2("Bu isimde bir rapor 'FastReport' dosyası mevcut!");
                        return;
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
            //if (_orm.Delete("Report", Id, true) > 0)
            //{
            //    string reportName = txtRaporAdi.Text;
            //    string destPath = $"reports/{reportName}.mrt";
            //    if (File.Exists(destPath))
            //    {
            //        File.Delete(destPath);
            //        Temizle();
            //    }
            //}
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
        //şuan için gizli
        private void frTest_Click(object sender, RoutedEventArgs e)
        {
            string reportName = $"\"{txtRaporAdi.Text}\"";
            Process.Start(reportAppPath, reportName);
        }
        //şuan için gizli
        private void frView_Click(object sender, RoutedEventArgs e)
        {
            string reportName = $"\"{txtRaporAdi.Text}\"";
            Process.Start(reportAppPath, $"{reportName} 1015");
        }

    }
}
