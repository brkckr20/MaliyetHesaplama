using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;

namespace ReportDesigner
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Bildirim.Uyari2("Rapor adı gönderilmedi!")
                return;
            }
            Report report = new Report();
            if (args.Length > 0 && File.Exists(args[0]))
            {
                report.Load(args[0]);
            }
            MessageBox.Show("Rapor tasarımcısı başlatılıyor...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            report.Design();
        }
    }
}
