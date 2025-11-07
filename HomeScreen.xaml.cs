using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaliyeHesaplama
{
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
        }
        private bool isMegaMenuVisible = false;
        private bool _isMegaMenuOpen = false;
        private string _currentMainMenu = null;
        private void Kartlar_Click(object sender, RoutedEventArgs e)
        {
            string title = "Kart İşlemleri";
            string[] items = { "Kumaş Kartı", "Firma Kartı" };
            ShowMegaMenu(title, items);
        }
        private void UretimYonetimi_Click(object sender, RoutedEventArgs e)
        {
            string title = "Üretim Yönetimi";
            string[] items = { "Maliyet Hesaplama", "Sipariş Girişi" };
            ShowMegaMenu(title, items);
        }
        private void MegaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string menuName = btn.Content.ToString();

                if (menuName == "Maliyet Hesaplama")
                    OpenTab(menuName, new userControls.Uc_MaliyetHesaplama());
                if (menuName == "Kumaş Kartı")
                    OpenTab(menuName, new userControls.UC_KumasKarti());
                if (menuName == "Firma Kartı")
                    OpenTab(menuName, new userControls.UC_FirmaKarti());
            }
        }
        private void OpenTab(string title, UserControl view)
        {
            var existingTab = MainTabControl.Items
                .OfType<TabItem>()
                .FirstOrDefault(t => (string)t.Header == title);

            if (existingTab != null)
            {
                MainTabControl.SelectedItem = existingTab;
                HideMegaMenu();
                return;
            }

            var tab = new TabItem
            {
                Header = title,
                Content = view
            };

            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedItem = tab;
            HideMegaMenu();
        }
        private void ShowMegaMenu(string title, string[] items)
        {
            // Eğer aynı menüye tekrar tıklandıysa menüyü kapat
            if (MegaMenuPanel.Visibility == Visibility.Visible && _currentMainMenu == title)
            {
                HideMegaMenu();
                return;
            }

            _currentMainMenu = title;
            MegaMenuTitle.Text = title;
            MegaMenuItems.ItemsSource = items;

            MegaMenuPanel.Visibility = Visibility.Visible;

            var slideIn = new DoubleAnimation
            {
                From = -250,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            MegaMenuTransform.BeginAnimation(TranslateTransform.XProperty, slideIn);
        }
        private void HideMegaMenu()
        {
            _currentMainMenu = null;

            var slideOut = new DoubleAnimation
            {
                From = 0,
                To = -250,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            slideOut.Completed += (s, e) =>
            {
                MegaMenuPanel.Visibility = Visibility.Collapsed;
            };

            MegaMenuTransform.BeginAnimation(TranslateTransform.XProperty, slideOut);
        }
        private void ContentArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideMegaMenu();
        }
        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.TemplatedParent is TabItem tab)
            {
                MainTabControl.Items.Remove(tab);
            }
        }
    }
}
