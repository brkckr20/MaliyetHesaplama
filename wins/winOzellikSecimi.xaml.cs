using MaliyeHesaplama.helpers;
//using Syncfusion.UI.Xaml.Grid;
using System.Windows;

namespace MaliyeHesaplama.wins
{
    public partial class winOzellikSecimi : Window
    {
        public string _type, Explanation;
        MiniOrm _orm = new MiniOrm();
        public int Id = 0, _inventoryType;
        public bool SecimYapildi = false;
        public winOzellikSecimi(string type, int inventoryType)
        {
            InitializeComponent();
            _type = type;
            _inventoryType = inventoryType;
            Title += " [ " + _type + " ]";
            dgOzellik.ItemsSource = _orm.GetAll<dynamic>("FeatureCoding").Where(x => x.Type == _type).ToList();
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (dgOzellik.SelectedItem != null)
            {
                dynamic r = dgOzellik.SelectedItem;
                _orm.Delete("FeatureCoding", r.Id, false);
                dgOzellik.ItemsSource = _orm.GetAll<dynamic>("FeatureCoding").Where(x => x.Type == _type).ToList();
            }
            else
            {
                Bildirim.Uyari2("Silme işlemini gerçekleştirebilmek için lütfen bir kayıt seçiniz!");
            }
        }

        private void dgOzellik_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgOzellik.SelectedItem != null)
            {
                this.SecimYapildi = true;
                dynamic r = dgOzellik.SelectedItem;
                Id = r.Id;
                Explanation = r.Explanation;
                this.Close();
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            var dict = new Dictionary<string, object>
            {
                {"Id",Id },{"Type", _type },{"Explanation", txtAciklama.Text},{"InventoryType", _inventoryType}
            };
            bool exist = _orm.IfExistRecord("FeatureCoding", "Explanation", txtAciklama.Text, _type) > 0;
            if (exist)
            {
                Bildirim.Bilgilendirme2($"{txtAciklama.Text} daha önce kayıt edilmiş, mükerrer kayıt olmaması için kayıt yapılamaz!");
            }
            else
            {
                _orm.Save("FeatureCoding", dict);
                dgOzellik.ItemsSource = _orm.GetAll<dynamic>("FeatureCoding").Where(x => x.Type == _type).ToList();
            }
        }
    }
}
