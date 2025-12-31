using MaliyeHesaplama.models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MaliyeHesaplama.wins
{
    public partial class winKolonAyarlari : Window
    {
        public List<ColumnSetting> ColumnSettings { get; set; }
        public bool SettingsSaved { get; private set; } = false;

        private List<System.Windows.Controls.CheckBox> checkBoxList = new List<System.Windows.Controls.CheckBox>();
        private Window parentWindow;

        public winKolonAyarlari(List<ColumnSetting> currentSettings, Window parent)
        {
            InitializeComponent();
            ColumnSettings = currentSettings;
            parentWindow = parent;
            LoadColumnList();

            // Parent window kapanırsa bu da kapansın
            parentWindow.Closed += (s, e) => this.Close();
        }

        private void LoadColumnList()
        {
            ColumnListPanel.Children.Clear();
            checkBoxList.Clear();

            foreach (var setting in ColumnSettings)
            {
                var checkBox = new System.Windows.Controls.CheckBox
                {
                    Content = setting.ColumnName,
                    IsChecked = setting.IsVisible,
                    Tag = setting,
                    Margin = new Thickness(5),
                    FontSize = 13
                };

                checkBox.Checked += CheckBox_Changed;
                checkBox.Unchecked += CheckBox_Changed;

                checkBoxList.Add(checkBox);
                ColumnListPanel.Children.Add(checkBox);
            }
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as System.Windows.Controls.CheckBox;
            var setting = checkBox.Tag as ColumnSetting;
            setting.IsVisible = checkBox.IsChecked ?? false;
        }

        private void TumunuSec_Click(object sender, RoutedEventArgs e)
        {
            foreach (var checkBox in checkBoxList)
            {
                checkBox.IsChecked = true;
            }
        }

        private void TumunuKaldir_Click(object sender, RoutedEventArgs e)
        {
            foreach (var checkBox in checkBoxList)
            {
                checkBox.IsChecked = false;
            }
        }

        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            SettingsSaved = true;
            Close();
        }

        private void Iptal_Click(object sender, RoutedEventArgs e)
        {
            SettingsSaved = false;
            Close();
        }
    }
}