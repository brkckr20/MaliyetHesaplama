using System.Windows;

namespace MaliyeHesaplama.helpers
{
    public static class Bildirim
    {
        public static void SilmeBasarili()
        {
            //MessageBox.Show("Silme işlemi başarılı", "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //public static bool SilmeOnayi()
        //{
        //    //return MessageBox.Show("Kayıt silinecek. Emin misiniz?\nBu işlem geri alınamaz", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        //}
        public static void Bilgilendirme(string msg)
        {
            //MessageBox.Show(msg, "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void Uyari(string msg)
        {
            //MessageBox.Show(msg, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //xceed messageboxlar
        public static void Uyari2(string msg)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(msg, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static void Bilgilendirme2(string msg)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(msg, "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static bool SilmeOnayi2()
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show("Kayıt silinecek. Emin misiniz?\nBu işlem geri alınamaz", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
        public static void SilmeBasarili2()
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("Silme işlemi başarılı", "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
