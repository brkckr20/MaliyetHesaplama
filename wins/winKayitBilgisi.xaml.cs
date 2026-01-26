using System.Globalization;
using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winKayitBilgisi : Window
    {
        int _insertedBy, _updatedBy;
        DateTime _insertedDate, _updatedDate;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userInserted = orm.GetById<dynamic>("Users", _insertedBy);
            var userUpdated = orm.GetById<dynamic>("Users", _updatedBy);
            lbInsertUser.Content = userInserted != null ? userInserted.Code + " - " + userInserted.Name + " " + userInserted.Surname : "Bilinmiyor";
            lbInsertDate.Content = _insertedDate.ToString("dd.MM.yyyy dddd", new CultureInfo("tr-TR"));
            lbInsertTime.Content = _insertedDate.ToString("HH:mm:ss");
            lbUpdateUser.Content = userUpdated != null ? userUpdated.Code + " - " + userUpdated.Name + " " + userUpdated.Surname : "Bilinmiyor";
            lbUpdateDate.Content = _updatedDate.ToString("dd.MM.yyyy dddd", new CultureInfo("tr-TR"));
            lbUpdateTime.Content = _updatedDate.ToString("HH:mm:ss");
        }

        MiniOrm orm = new MiniOrm();
        public winKayitBilgisi(int InsertedBy, DateTime InsertedDate, int UpdatedBy, DateTime UpdatedDate)
        {
            InitializeComponent();
            _insertedBy = InsertedBy;
            _insertedDate = InsertedDate;
            _updatedBy = UpdatedBy;
            _updatedDate = UpdatedDate;
        }
    }
}
