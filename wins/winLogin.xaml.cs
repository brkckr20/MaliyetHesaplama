using MaliyeHesaplama.helpers;
using System.Windows;

namespace MaliyeHesaplama.wins
{
    public class User
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }

    public partial class winLogin : Window
    {
        public winLogin()
        {
            InitializeComponent();
            KullanicilariGetir();
            LoadRememberMe();
        }

        MiniOrm _orm = new MiniOrm();
        int Id = 0;

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (cmbUsername.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Lütfen kullanıcı seçiniz!");
                return;
            }
            Id = Convert.ToInt32(cmbUsername.SelectedValue);
            var user = _orm.GetById<dynamic>("Users", Id);
            if (txtPassword.Password == user.Password.ToString())
            {
                if (chkRememberMe.IsChecked == true)
                {
                    Properties.Settings.Default.RememberMe = true;
                    Properties.Settings.Default.RememberUserId = Id;
                    Properties.Settings.Default.Password = txtPassword.Password;
                }
                else
                {
                    Properties.Settings.Default.RememberMe = false;
                    Properties.Settings.Default.RememberUserId = 0;
                    Properties.Settings.Default.Password = string.Empty;
                }
                Properties.Settings.Default.Save();
                this.DialogResult = true;
                this.Close();               
            }
            else
            {
                Bildirim.Uyari2("Kullanıcı adı veya şifre yanlış!");
            }
        }
        void KullanicilariGetir()
        {
            //_orm.TestQuery();
            var list = _orm.GetAll<User>("Users")
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Surname,
                    FullName = $"{x.Code} - {x.Name} {x.Surname}"
                }).ToList();
            cmbUsername.ItemsSource = list;
        }
        void LoadRememberMe()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                chkRememberMe.IsChecked = true;
                cmbUsername.SelectedValue = Properties.Settings.Default.RememberUserId;
                txtPassword.Password = Properties.Settings.Default.Password;
            }
        }
    }
}
