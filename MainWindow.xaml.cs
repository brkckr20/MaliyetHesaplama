using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MaliyeHesaplama
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddOrSelectTab(string header, UserControl content)
        {
            foreach (TabItem item in mainTabControl.Items)
            {
                if (item.Header is StackPanel sp &&
                    sp.Children.OfType<TextBlock>().FirstOrDefault()?.Text == header)
                {
                    mainTabControl.SelectedItem = item;
                    return;
                }
            }

            // Header için StackPanel: Başlık + X butonu
            StackPanel headerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock headerText = new TextBlock
            {
                Text = header,
                Margin = new Thickness(0, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            Button closeButton = new Button
            {
                Content = "❌",
                Width = 16,
                Height = 16,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                ToolTip = "Sekmeyi Kapat"
            };

            closeButton.Click += (s, e) =>
            {
                mainTabControl.Items.Remove(
                    mainTabControl.Items
                        .Cast<TabItem>()
                        .FirstOrDefault(t => t.Header == headerPanel)
                );
            };

            headerPanel.Children.Add(headerText);
            headerPanel.Children.Add(closeButton);

            TabItem newTabItem = new TabItem
            {
                Header = headerPanel,
                Content = content
            };

            mainTabControl.Items.Add(newTabItem);
            mainTabControl.SelectedItem = newTabItem;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
               

        private void btnMaliyetHesaplama_Click(object sender, RoutedEventArgs e)
        {
            userControls.Uc_MaliyetHesaplama uc = new userControls.Uc_MaliyetHesaplama();
            AddOrSelectTab("Maliyet Hesaplama", uc);
        }
    }
}