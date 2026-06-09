using MaliyeHesaplama.helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MaliyeHesaplama
{
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();

        }
        private string _currentMainMenu = null;
        private bool _isDraggingFromMaximized = false;
        private System.Drawing.Point _restoreMousePosition;
        private void UretimYonetimi_Click(object sender, RoutedEventArgs e)
        {
            string title = "Üretim Yönetimi";
            string[] items = { "Maliyet Hesaplama", "Sipariş Girişi", "Üretim Girişi", "Onaylama İşlemleri" };
            ShowMegaMenu(title, items);
        }
        private void Ayarlar_Click(object sender, RoutedEventArgs e)
        {
            string title = "Ayarlar";
            string[] items = { "Rapor Oluşturma" };
            ShowMegaMenu(title, items);
        }

        private void V2_Click(object sender, RoutedEventArgs e)
        {
            string title = "v2 Kartlar";
            string[] items = { "Firma Kartı", "Malzeme Kartı V2", "Depo Kartı V2" };
            ShowMegaMenu(title, items);
        }

        private void MalzemeYonetimiV2_Click(object sender, RoutedEventArgs e)
        {
            string title = "Malzeme Yönetimi v2";
            string[] items = { "Malzeme Giriş", "Malzeme Çıkış" };
            ShowMegaMenu(title, items);
        }
        private void MegaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                string menuName = btn.Content.ToString();
                /*Üretim Yönetimi*/
                if (menuName == "Maliyet Hesaplama")
                    OpenTab(menuName, new userControls.Uc_MaliyetHesaplama());
                if (menuName == "Sipariş Girişi")
                    OpenTab(menuName, new userControls.UC_SiparisGirisi2());
                if (menuName == "Üretim Girişi")
                    OpenTab(menuName, new userControls.UC_UretimGirisi());
                if (menuName == "Onaylama İşlemleri")
                    OpenTab(menuName, new userControls.UC_OnaylamaIslemleri());

                /* Ayarlar */
                if (menuName == "Rapor Oluşturma")
                    OpenTab(menuName, new userControls.UC_RaporOlusturma());

                /* v2 */
                if (menuName == "Firma Kartı")
                    OpenTab(menuName, new userControls.UC_FirmaKarti(false));
                if (menuName == "Malzeme Kartı V2")
                    OpenTab(menuName, new v2.UserControls.UC_MalzemeKartiV2());
                if (menuName == "Depo Kartı V2")
                    OpenTab(menuName, new v2.UserControls.UC_DepoKartiV2());
                /* Malzeme Yönetimi v2 */
                if (menuName == "Malzeme Giriş")
                    OpenTab(menuName, new v2.UserControls.UC_MalzemeFisV2(v2.Models.ReceiptType.MalzemeGiris));

                if (menuName == "Malzeme Çıkış")
                    OpenTab(menuName, new v2.UserControls.UC_MalzemeFisV2(v2.Models.ReceiptType.MalzemeCikis));
            }

        }
        private void OpenTab(string title, System.Windows.Controls.UserControl view)
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
            if (sender is System.Windows.Controls.Button btn && btn.TemplatedParent is TabItem tab)
            {
                MainTabControl.Items.Remove(tab);
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }
            if (this.WindowState == WindowState.Maximized)
            {
                _isDraggingFromMaximized = true;
                var pos = e.GetPosition(this);
                _restoreMousePosition = new System.Drawing.Point((int)pos.X, (int)pos.Y);
                return;
            }

            this.DragMove();
        }

        private void btnAppClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void btnAppMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnAppMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        private void ToggleMaximize()
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        private void TitleBar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDraggingFromMaximized && e.LeftButton == MouseButtonState.Pressed)
            {
                _isDraggingFromMaximized = false;

                // Ekran koordinatını al
                var mouseX = e.GetPosition(this).X;
                var percentX = mouseX / this.ActualWidth;
                var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
                var screenWidth = screen.WorkingArea.Width;
                var screenHeight = screen.WorkingArea.Height;

                // Yeni pencereyi normal moda al
                this.WindowState = WindowState.Normal;

                // Pencerenin pozisyonunu mouse oranına göre yeniden ayarla
                this.Left = screen.WorkingArea.Left + (screenWidth * percentX) - (_restoreMousePosition.X);
                this.Top = screen.WorkingArea.Top + 5; // küçük offset

                this.DragMove(); // sürüklemeye devam et
            }
        }

        
    }
}
