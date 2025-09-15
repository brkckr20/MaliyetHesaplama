using System.Windows;

namespace MaliyeHesaplama.helpers
{
    public static class Bildirim
    {
        public static void SilmeBasarili()
        {
            MessageBox.Show("Silme işlemi başarılı", "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static bool SilmeOnayi()
        {
            return MessageBox.Show("Kayıt silinecek. Emin misiniz?\nBu işlem geri alınamaz", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
        public static void Bilgilendirme(string msg)
        {
            MessageBox.Show(msg, "Bilgilendirme", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static void Uyari(string msg)
        {
            MessageBox.Show(msg, "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
