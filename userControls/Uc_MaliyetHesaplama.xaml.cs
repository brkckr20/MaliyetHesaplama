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

        #region deneme amaçlı açıldı
        private void IkiSayiyiBolVeYansit(TextBox bolunenTextBox, TextBox bolenTextBox, TextBox sonucTextBlock)
        {
            if (double.TryParse(bolunenTextBox.Text, out double bolunen) && double.TryParse(bolenTextBox.Text, out double bolen))
            {
                if (bolen != 0)
                {
                    sonucTextBlock.Text = (bolunen / bolen).ToString();
                }
                else
                {
                    sonucTextBlock.Text = "Sıfıra bölünemez";
                }
            }
            else
            {
                //sonucTextBlock.Text = "Geçersiz giriş";
            }
        }

        private double IkiSayiyiCarpVeYansit(TextBox tbCarpan1, TextBox tbCarpan2)
        {
            if (double.TryParse(tbCarpan1.Text, out double carpan1) && double.TryParse(tbCarpan2.Text, out double carpan2))
            {
                return carpan1 * carpan2;
            }
            return 1;
        }

        private void MultipleOnChange(object sender, TextChangedEventArgs e)
        {
            var changedTextBox = sender as TextBox;
            if (changedTextBox == txtTarak1DokBil1 || changedTextBox == txtTarak1DokBil2)
            {

                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak1DokBil1, txtTarak1DokBil2);
                txtTarak1DokBil3.Text = hesapSonucu.ToString();
                txtCozguSiklik1.Text = hesapSonucu.ToString();               
            }
            else if (changedTextBox == txtTarak2DokBil1 || changedTextBox == txtTarak2DokBil2)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak2DokBil1, txtTarak2DokBil2);
                txtTarak2DokBil3.Text = hesapSonucu.ToString();
                txtCozguSiklik2.Text = hesapSonucu.ToString();
            }
            else if (changedTextBox == txtTarak1DokBil3 || changedTextBox == txtTarakEn)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak1DokBil3, txtTarakEn);
                txtCozgu1TelSayisi.Text = hesapSonucu.ToString();
            }
            else if (changedTextBox == txtTarak2DokBil3 || changedTextBox == txtTarakEn)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak2DokBil3, txtTarakEn);
                txtCozgu2TelSayisi.Text = hesapSonucu.ToString();
            } // tarak no iki change ile hesaplanan çözgü tel sayılarını kontrol et.
        }

        private void DivideOnchange(object sender, TextChangedEventArgs e)
        {
            var changedTextBox = sender as TextBox;
            if (changedTextBox == txtCozgu1IpBil1 || changedTextBox == txtCozgu1IpBil2)
            {
                IkiSayiyiBolVeYansit(txtCozgu1IpBil1, txtCozgu1IpBil2, txtCozgu1IpBil3);
            }
            else if (changedTextBox == txtCozgu2IpBil1 || changedTextBox == txtCozgu2IpBil2)
            {
                IkiSayiyiBolVeYansit(txtCozgu2IpBil1, txtCozgu2IpBil2, txtCozgu2IpBil3);
            }
            else if (changedTextBox == txtAtki1IpBil1 || changedTextBox == txtAtki1IpBil2)
            {
                IkiSayiyiBolVeYansit(txtAtki1IpBil1, txtAtki1IpBil2, txtAtki1IpBil3);
            }
            else if (changedTextBox == txtAtki2IpBil1 || changedTextBox == txtAtki2IpBil2)
            {
                IkiSayiyiBolVeYansit(txtAtki2IpBil1, txtAtki2IpBil2, txtAtki2IpBil3);
            }
            else if (changedTextBox == txtAtki3IpBil1 || changedTextBox == txtAtki3IpBil2)
            {
                IkiSayiyiBolVeYansit(txtAtki3IpBil1, txtAtki3IpBil2, txtAtki3IpBil3);
            }
            else if (changedTextBox == txtAtki4IpBil1 || changedTextBox == txtAtki4IpBil2)
            {
                IkiSayiyiBolVeYansit(txtAtki4IpBil1, txtAtki4IpBil2, txtAtki4IpBil3);
            }
        }

        #endregion
        private void YansitHamEn(object sender, EventArgs e)
        {
            if (sender is TextBox source && double.TryParse(source.Text, out double value))
            {
                string targetName = source.Tag?.ToString();
                if (!string.IsNullOrEmpty(targetName))
                {
                    var target = this.FindName(targetName) as TextBox;
                    if (target != null)
                    {
                        double result = value / 1.05; // sabit oran
                        target.Text = result.ToString("0");
                    }
                }
            }
            else if (sender is TextBox src)
            {
                string targetName = src.Tag?.ToString();
                var target = this.FindName(targetName) as TextBox;
                target?.Clear();
            }
        }
        private void CarpVeYansit(TextBox kaynak1, TextBox kaynak2, TextBox hedef)
        {
            double deger1;
            if (!double.TryParse(kaynak1.Text, out deger1))
            {
                deger1 = 0; // Geçersizse 0 olarak kabul et
            }

            // Kaynak 2 TextBox'ın değerini al
            double deger2;
            if (!double.TryParse(kaynak2.Text, out deger2))
            {
                deger2 = 0; // Geçersizse 0 olarak kabul et
            }

            // Değerleri topla
            double toplam = deger1 + deger2;

            // Sonucu hedef TextBox'a yaz
            hedef.Text = toplam.ToString();
        }
        private void MultipleTwoTextBox(object sender, TextChangedEventArgs e)
        {
            YansitHamEn(sender, e);
            //ValueTextBox_TextChanged(sender, e);
            //CarpVeYansit(txtResult1, txtTarakEn, txtCozgu1TelSayisi);
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
