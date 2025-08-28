using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    /// <summary>
    /// Interaction logic for Uc_MaliyetHesaplama.xaml
    /// </summary>
    public partial class Uc_MaliyetHesaplama : UserControl
    {
        public Uc_MaliyetHesaplama()
        {
            InitializeComponent();
        }
        private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                DockPanel panel = tb.Parent as DockPanel;
                if (panel != null && panel.Tag is string operation)
                {
                    var textBoxes = panel.Children.OfType<TextBox>().ToArray();
                    if (textBoxes.Length >= 3)
                    {
                        double.TryParse(textBoxes[0].Text, out double v1);
                        double.TryParse(textBoxes[1].Text, out double v2);
                        double result = 0;

                        switch (operation)
                        {
                            case "Multiply":
                                result = v1 * v2;
                                break;
                            case "Divide":
                                result = v2 != 0 ? v1 / v2 : 0;
                                break;
                            case "Divide_1.05":
                                result = v1 / 1.05;
                                break;
                        }

                        TextBox resultBox = textBoxes[2];
                        resultBox.Text = result.ToString();

                        // Kopyalama
                        if (resultBox.Tag is string targetName)
                        {
                            var target = this.FindName(targetName) as TextBox;
                            if (target != null)
                            {
                                target.Text = resultBox.Text;
                            }
                        }
                    }
                }
            }
        }
    }
}
