using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MaliyeHesaplama.wins
{
    /// <summary>
    /// Interaction logic for winBuyukResim.xaml
    /// </summary>
    public partial class winBuyukResim : Window
    {
        int _id;
        MiniOrm _orm = new MiniOrm();
        public winBuyukResim(int Id, string tableName, string fieldName)
        {
            InitializeComponent();
            _id = Id;
            using (var stream = new MemoryStream(_orm.GetImage(tableName, fieldName, _id)))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                largeImage.Source = bitmap;
            }
        }
    }
}
