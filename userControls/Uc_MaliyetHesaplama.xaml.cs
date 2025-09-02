using System.Windows.Controls;

namespace MaliyeHesaplama.userControls
{
    public partial class Uc_MaliyetHesaplama : UserControl
    {
        public Uc_MaliyetHesaplama()
        {
            InitializeComponent();
        }

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
        private void HamEnHesapla(TextBox source, TextBox target)
        {
            if (double.TryParse(source.Text, out double carpan1) && double.TryParse(target.Text, out double sonuc))
            {
                sonuc = carpan1 / (1.05);
            }
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
            if (changedTextBox == txtTarak2DokBil1 || changedTextBox == txtTarak2DokBil2)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak2DokBil1, txtTarak2DokBil2);
                txtTarak2DokBil3.Text = hesapSonucu.ToString();
                txtCozguSiklik2.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtTarak1DokBil3 || changedTextBox == txtTarakEn)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak1DokBil3, txtTarakEn);
                txtCozgu1TelSayisi.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtTarak2DokBil3 || changedTextBox == txtTarakEn)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtTarak2DokBil3, txtTarakEn);
                txtCozgu2TelSayisi.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtHamBoy || changedTextBox == txtAtki1Siklik)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtHamBoy, txtAtki1Siklik);
                txtAtki1TelSay.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtHamBoy || changedTextBox == txtAtki2Siklik)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtHamBoy, txtAtki2Siklik);
                txtAtki2TelSay.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtHamBoy || changedTextBox == txtAtki3Siklik)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtHamBoy, txtAtki3Siklik);
                txtAtki3TelSay.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtHamBoy || changedTextBox == txtAtki4Siklik)
            {
                double hesapSonucu = IkiSayiyiCarpVeYansit(txtHamBoy, txtAtki4Siklik);
                txtAtki4TelSay.Text = hesapSonucu.ToString();
            }
            if (changedTextBox == txtTarakEn)
            {
                HamEnHesapla(txtTarakEn, txtHamEn);
            }
        }
        private void DivideOnchange(object sender, TextChangedEventArgs e)
        {
           
        }
    }
}
