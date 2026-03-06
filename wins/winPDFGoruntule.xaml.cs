using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winPDFGoruntule : Window
    {
        string _path;
        public winPDFGoruntule(string path)
        {
            InitializeComponent();
            _path = path;
            pdfViewer.Navigate(path);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            pdfViewer.Navigate((Uri)null);
            //if (File.Exists(_path))
            //{
            //    File.Delete(_path);  // Geçici dosyayı sil
            //}
        }
    }
}
