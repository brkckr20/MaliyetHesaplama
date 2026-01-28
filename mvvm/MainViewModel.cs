using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Windows;
using System.Windows.Input;


namespace MaliyeHesaplama.mvvm
{
    public partial class MainViewModel : ObservableObject
    {
        MiniOrm _orm = new MiniOrm();
        public ICommand AcButtonCommand { get; }
        public MainViewModel()
        {
            Cozgu1IpBilBolen = 1; Cozgu1IpBilBolunen = 1; Cozgu2IpBilBolen = 1; Cozgu2IpBilBolunen = 1; Atki1IpBilBolen = 1; Atki1IpBilBolunen = 1; Atki2IpBilBolen = 1; Atki2IpBilBolunen = 1; Atki3IpBilBolen = 1; Atki3IpBilBolunen = 1; Atki4IpBilBolen = 1; Atki4IpBilBolunen = 1;
            TarakNo1Carpan = 0; TarakNo1Carpim = 0; TarakNo2Carpan = 0; TarakNo2Carpim = 0;
            TarakEn = 0; HamBoy = 0; BoySacak = 0; EnSacak = 0; HamEn = 0; MamulBoy = 0; MamulEn = 0;
            Cozgu1Gramaj = 0.00; Cozgu2Gramaj = 0.00; Atki1Gramaj = 0.00; Atki2Gramaj = 0.00; Atki3Gramaj = 0.00; Atki4Gramaj = 0.00;
            KurUrFiy = 36;
            BoySacakText = "0"; EnSacakText = "0";
            Cozgu1IpFiyText = "0.00"; Cozgu2IpFiyText = "0.00"; Atki1IpFiyText = "0.00"; Atki2IpFiyText = "0.00"; Atki3IpFiyText = "0.00"; Atki4IpFiyText = "0.00";
            Cozgu1IpBoyText = "0.00"; Cozgu2IpBoyText = "0.00"; Atki1IpBoyText = "0.00"; Atki2IpBoyText = "0.00"; Atki3IpBoyText = "0.00"; Atki4IpBoyText = "0.00";
            atkiUrFiyText = "0.00"; CozguUrFiyText = "0.00"; ParcaYikamaUrFiyText = "0.00"; KumasBoyamaUrFiyText = "0.00"; DokumaFiresiUrFiyText = "0.00"; BoyaFiresiUrFiyText = "0.00"; KonfMaliyetiUrFiyText = "0.00"; IkinciKaliyeMaliyetiUrFiyText = "0.00"; KarUrFiyText = "0.00"; KdvUrFiyText = "0.00"; KurUrFiyText = _orm.GetEURCurrency(); PariteUrFiyText = "0.00"; EurUrFiyText = "0.00";
            BelirlenenFiyatText = "0.00";
            AcButtonCommand = new RelayCommand<object>(OnAcButon);
        }
        private void OnAcButon(object obj)
        {

            //MessageBox.Show($"Butona tıklandı: ");
        }

        /********************************* ÜRETİM BİLGİLERİ - İPLİK BİLGİLERİ **********************************/
        [ObservableProperty] //değişkenler
        private double cozgu1IpBilBolen, cozgu1IpBilBolunen, cozgu1IpBilSonuc, cozgu2IpBilBolen, cozgu2IpBilBolunen, cozgu2IpBilSonuc, atki1IpBilBolen, atki1IpBilBolunen, atki1IpBilSonuc, atki2IpBilBolen, atki2IpBilBolunen, atki2IpBilSonuc, atki3IpBilBolen, atki3IpBilBolunen, atki3IpBilSonuc, atki4IpBilBolen, atki4IpBilBolunen, atki4IpBilSonuc;

        partial void OnCozgu1IpBilBolenChanged(double value) => Cozgu1IpBilSonuc = Cozgu1IpBilBolen / Cozgu1IpBilBolunen;
        partial void OnCozgu1IpBilBolunenChanged(double value) => Cozgu1IpBilSonuc = Cozgu1IpBilBolen / Cozgu1IpBilBolunen;
        partial void OnCozgu1IpBilSonucChanged(double value) => Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000); //1.05 parametre olarak ayarla vedüzelt hepsni
        partial void OnCozgu2IpBilBolenChanged(double value) => Cozgu2IpBilSonuc = Cozgu2IpBilBolen / Cozgu2IpBilBolunen;
        partial void OnCozgu2IpBilSonucChanged(double value) => Cozgu2Gramaj = (HamBoy * Cozgu2TelSay * (60 / Cozgu2IpBilSonuc) * 1.05 / 10000000);  //1.05 parametre olarak ayarla vedüzelt hepsni
        partial void OnCozgu2IpBilBolunenChanged(double value) => Cozgu2IpBilSonuc = Cozgu2IpBilBolen / Cozgu2IpBilBolunen;
        partial void OnAtki1IpBilBolenChanged(double value) => Atki1IpBilSonuc = Atki1IpBilBolen / Atki1IpBilBolunen;
        partial void OnAtki1IpBilBolunenChanged(double value) => Atki1IpBilSonuc = Atki1IpBilBolen / Atki1IpBilBolunen;
        partial void OnAtki1IpBilSonucChanged(double value) => Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak) / 100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
        partial void OnAtki2IpBilBolenChanged(double value) => Atki2IpBilSonuc = Atki2IpBilBolen / Atki2IpBilBolunen;
        partial void OnAtki2IpBilBolunenChanged(double value) => Atki2IpBilSonuc = Atki2IpBilBolen / Atki2IpBilBolunen;
        partial void OnAtki3IpBilBolenChanged(double value) => Atki3IpBilSonuc = Atki3IpBilBolen / Atki3IpBilBolunen;
        partial void OnAtki3IpBilBolunenChanged(double value) => Atki3IpBilSonuc = Atki3IpBilBolen / Atki3IpBilBolunen;
        partial void OnAtki4IpBilBolenChanged(double value) => Atki4IpBilSonuc = Atki4IpBilBolen / Atki4IpBilBolunen;
        partial void OnAtki4IpBilBolunenChanged(double value) => Atki4IpBilSonuc = Atki4IpBilBolen / Atki4IpBilBolunen;

        /********************************* ÜRETİM BİLGİLERİ - DOKUMA BİLGİLERİ **********************************/
        [ObservableProperty] //değişkenler
        private double tarakNo1Carpan, tarakNo1Carpim, tarakNo1Sonuc, tarakNo2Carpan, tarakNo2Carpim, tarakNo2Sonuc, tarakEn, hamEn, hamBoy, boySacak, enSacak, mamulBoy, mamulEn;
        [ObservableProperty]
        private string boySacakText, enSacakText;
        partial void OnBoySacakTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    BoySacak = d;
                }
            }
        }
        partial void OnEnSacakTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    EnSacak = d;
                }
            }
        }
        partial void OnTarakNo1CarpanChanged(double value) => TarakNo1Sonuc = TarakNo1Carpan * TarakNo1Carpim;
        partial void OnTarakNo1CarpimChanged(double value) => TarakNo1Sonuc = TarakNo1Carpan * TarakNo1Carpim;
        partial void OnTarakNo2CarpanChanged(double value) => TarakNo2Sonuc = TarakNo2Carpan * TarakNo2Carpim;
        partial void OnTarakNo2CarpimChanged(double value) => TarakNo2Sonuc = TarakNo2Carpan * TarakNo2Carpim;
        partial void OnTarakNo1SonucChanged(double value)
        {
            Cozgu1Siklik = TarakNo1Sonuc; Cozgu1TelSay = TarakNo1Sonuc * TarakEn;
        }
        partial void OnTarakNo2SonucChanged(double value)
        {
            Cozgu2Siklik = TarakNo2Sonuc; Cozgu2TelSay = TarakNo2Sonuc * TarakEn;
        }
        partial void OnTarakEnChanged(double value)
        {
            HamEn = Math.Round(TarakEn / 1.05); Cozgu1TelSay = TarakEn * TarakNo1Sonuc; Cozgu2TelSay = TarakEn * TarakNo2Sonuc;
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak) / 100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
            Atki2Gramaj = (Atki2Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki2IpBilSonuc) * 1.05) / 1000;
            Atki3Gramaj = (Atki3Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki3IpBilSonuc) * 1.05) / 1000;
            Atki4Gramaj = (Atki4Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki4IpBilSonuc) * 1.05) / 1000;
            EndenCekmesi = TarakEn / MamulEn;
        }
        partial void OnHamBoyChanged(double value)
        {
            Atki1TelSay = HamBoy * Atki1Siklik; Atki2TelSay = HamBoy * Atki2Siklik; Atki3TelSay = HamBoy * Atki3Siklik; Atki4TelSay = HamBoy * Atki4Siklik;
            Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
            Cozgu2Gramaj = (HamBoy * Cozgu2TelSay * (60 / Cozgu2IpBilSonuc) * 1.05 / 10000000);
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak) / 100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
            Atki2Gramaj = (Atki2Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki2IpBilSonuc) * 1.05) / 1000;
            Atki3Gramaj = (Atki3Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki3IpBilSonuc) * 1.05) / 1000;
            Atki4Gramaj = (Atki4Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki4IpBilSonuc) * 1.05) / 1000;
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
            //=(B16/100)*Q7
            CozguDokMal = (HamBoy / 100) * CozguUrFiy;
        }
        partial void OnBoySacakChanged(double value)
        {
            Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }
        partial void OnEnSacakChanged(double value)
        {
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak) / 100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
            Atki2Gramaj = (Atki2Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki2IpBilSonuc) * 1.05) / 1000;
            Atki3Gramaj = (Atki3Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki3IpBilSonuc) * 1.05) / 1000;
            Atki4Gramaj = (Atki4Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki4IpBilSonuc) * 1.05) / 1000;
        }

        /********************************* ÜRETİM BİLGİLERİ - SIKLIKLAR **********************************/
        [ObservableProperty] //değişkenler
        private double cozgu1Siklik, cozgu2Siklik, atki1Siklik, atki2Siklik, atki3Siklik, atki4Siklik;
        partial void OnAtki1SiklikChanged(double value)
        {
            Atki1TelSay = HamBoy * Atki1Siklik;
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak) / 100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }
        partial void OnAtki2SiklikChanged(double value)
        {
            Atki2TelSay = HamBoy * Atki2Siklik;
            Atki2Gramaj = (Atki2Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki2IpBilSonuc) * 1.05) / 1000;
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }
        partial void OnAtki3SiklikChanged(double value)
        {
            Atki3TelSay = HamBoy * Atki3Siklik;
            Atki3Gramaj = (Atki3Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki3IpBilSonuc) * 1.05) / 1000;
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }
        partial void OnAtki4SiklikChanged(double value)
        {
            Atki4TelSay = HamBoy * Atki4Siklik;
            Atki4Gramaj = (Atki4Siklik * ((TarakEn + EnSacak) / 100) * (HamBoy / 100) * (60 / Atki4IpBilSonuc) * 1.05) / 1000;
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }

        partial void OnMamulEnChanged(double value) // bu changed çalışmadı - 28.01.2026
        {
            EndenCekmesi = TarakEn / MamulEn;
        }

        /********************************* ÜRETİM BİLGİLERİ - TEL SAYILARI **********************************/
        [ObservableProperty] //değişkenler
        private double cozgu1TelSay, cozgu2TelSay, atki1TelSay, atki2TelSay, atki3TelSay, atki4TelSay;
        partial void OnCozgu1TelSayChanged(double value) => Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
        partial void OnCozgu2TelSayChanged(double value) => Cozgu2Gramaj = (HamBoy * Cozgu2TelSay * (60 / Cozgu2IpBilSonuc) * 1.05 / 10000000);

        /********************************* ÜRETİM HESAPLAMA - GRAMAJ HESAPLAMA **********************************/
        [ObservableProperty]
        private double cozgu1Gramaj, cozgu2Gramaj, atki1Gramaj, atki2Gramaj, atki3Gramaj, atki4Gramaj, toplamGramaj;
        partial void OnCozgu1GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Cozgu1IpMal = (Cozgu1IpBoy + Cozgu1IpFiy) * Cozgu1Gramaj;
        }
        partial void OnCozgu2GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Cozgu2IpMal = (Cozgu2IpBoy + Cozgu2IpFiy) * Cozgu2Gramaj;
        }
        partial void OnAtki1GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Atki1IpMal = (Atki1IpBoy + Atki1IpFiy) * Atki1Gramaj;
        }
        partial void OnAtki2GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Atki2IpMal = (Atki2IpBoy + Atki2IpFiy) * Atki2Gramaj;
        }
        partial void OnAtki3GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Atki3IpMal = (Atki3IpBoy + Atki3IpFiy) * Atki3Gramaj;
        }
        partial void OnAtki4GramajChanged(double value)
        {
            ToplamGramaj = Cozgu1Gramaj + Cozgu2Gramaj + Atki1Gramaj + Atki2Gramaj + Atki3Gramaj + Atki4Gramaj;
            Atki4IpMal = (Atki4IpBoy + Atki4IpFiy) * Atki4Gramaj;
        }
        partial void OnToplamGramajChanged(double value)
        {
            IpBoySonuc = ToplamGramaj / HamEn * 100;
            ParcaYikamaYBM = ParcaYikamaUrFiy * ToplamGramaj;
            BoyanmisKumasYBM = (EurUrFiy * ToplamGramaj) + FireliUrMal;
        }

        /********************************* ÜRETİM HESAPLAMA - IPLIK FIYATLARI **********************************/
        [ObservableProperty]
        private double cozgu1IpFiy, cozgu2IpFiy, atki1IpFiy, atki2IpFiy, atki3IpFiy, atki4IpFiy;
        [ObservableProperty]
        private string cozgu1IpFiyText, cozgu2IpFiyText, atki1IpFiyText, atki2IpFiyText, atki3IpFiyText, atki4IpFiyText;
        partial void OnCozgu1IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Cozgu1IpFiy = d;
                }
            }
        }
        partial void OnCozgu2IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Cozgu2IpFiy = d;
                }
            }
        }
        partial void OnAtki1IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki1IpFiy = d;
                }
            }
        }
        partial void OnAtki2IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki2IpFiy = d;
                }
            }
        }
        partial void OnAtki3IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki3IpFiy = d;
                }
            }
        }
        partial void OnAtki4IpFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki4IpFiy = d;
                }
            }
        }
        partial void OnCozgu1IpFiyChanged(double value) => Cozgu1IpMal = (Cozgu1IpBoy + Cozgu1IpFiy) * Cozgu1Gramaj;
        partial void OnCozgu2IpFiyChanged(double value) => Cozgu2IpMal = (Cozgu2IpBoy + Cozgu2IpFiy) * Cozgu2Gramaj;
        partial void OnAtki1IpFiyChanged(double value) => Atki1IpMal = (Atki1IpBoy + Atki1IpFiy) * Atki1Gramaj;
        partial void OnAtki2IpFiyChanged(double value) => Atki2IpMal = (Atki2IpBoy + Atki2IpFiy) * Atki2Gramaj;
        partial void OnAtki3IpFiyChanged(double value) => Atki3IpMal = (Atki3IpBoy + Atki3IpFiy) * Atki3Gramaj;
        partial void OnAtki4IpFiyChanged(double value) => Atki4IpMal = (Atki4IpBoy + Atki4IpFiy) * Atki4Gramaj;

        /********************************* ÜRETİM HESAPLAMA - IPLIK BOYAMALARI **********************************/
        [ObservableProperty]
        private double cozgu1IpBoy, cozgu2IpBoy, atki1IpBoy, atki2IpBoy, atki3IpBoy, atki4IpBoy, ipBoySonuc;
        [ObservableProperty]
        private string cozgu1IpBoyText, cozgu2IpBoyText, atki1IpBoyText, atki2IpBoyText, atki3IpBoyText, atki4IpBoyText, ipBoySonucText;
        partial void OnCozgu1IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Cozgu1IpBoy = d;
                }
            }
        }
        partial void OnCozgu2IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Cozgu2IpBoy = d;
                }
            }
        }
        partial void OnAtki1IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki1IpBoy = d;
                }
            }
        }
        partial void OnAtki2IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki2IpBoy = d;
                }
            }
        }
        partial void OnAtki3IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki3IpBoy = d;
                }
            }
        }
        partial void OnAtki4IpBoyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    Atki4IpBoy = d;
                }
            }
        }
        partial void OnCozgu1IpBoyChanged(double value) => Cozgu1IpMal = (Cozgu1IpBoy + Cozgu1IpFiy) * Cozgu1Gramaj;
        partial void OnCozgu2IpBoyChanged(double value) => Cozgu2IpMal = (Cozgu2IpBoy + Cozgu2IpFiy) * Cozgu2Gramaj;
        partial void OnAtki1IpBoyChanged(double value) => Atki1IpMal = (Atki1IpBoy + Atki1IpFiy) * Atki1Gramaj;
        partial void OnAtki2IpBoyChanged(double value) => Atki2IpMal = (Atki2IpBoy + Atki2IpFiy) * Atki2Gramaj;
        partial void OnAtki3IpBoyChanged(double value) => Atki3IpMal = (Atki3IpBoy + Atki3IpFiy) * Atki3Gramaj;
        partial void OnAtki4IpBoyChanged(double value) => Atki4IpMal = (Atki4IpBoy + Atki4IpFiy) * Atki4Gramaj;

        /********************************* ÜRETİM HESAPLAMA - IPLIK MALİYET **********************************/
        [ObservableProperty]
        private double cozgu1IpMal, cozgu2IpMal, atki1IpMal, atki2IpMal, atki3IpMal, atki4IpMal, toplamIpMal;
        partial void OnToplamIpMalChanged(double value) => IplikMaliyetDokMal = ToplamIpMal;
        partial void OnCozgu1IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;
        partial void OnCozgu2IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;
        partial void OnAtki1IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;
        partial void OnAtki2IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;
        partial void OnAtki3IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;
        partial void OnAtki4IpMalChanged(double value) => ToplamIpMal = Cozgu1IpMal + Cozgu2IpMal + Atki1IpMal + Atki2IpMal + Atki3IpMal + Atki4IpMal;

        /********************************* MALİYET HESAPLAMA - ÜRETİM FİYATLARI **********************************/
        [ObservableProperty]
        private double atkiUrFiy, cozguUrFiy, parcaYikamaUrFiy, kumasBoyamaUrFiy, dokumaFiresiUrFiy, boyaFiresiUrFiy, konfMaliyetiUrFiy, _ikinciKaliyeMaliyetiUrFiy, karUrFiy, kdvUrFiy, kurUrFiy, pariteUrFiy, eurUrFiy;
        [ObservableProperty]
        private string atkiUrFiyText, cozguUrFiyText, parcaYikamaUrFiyText, kumasBoyamaUrFiyText, dokumaFiresiUrFiyText, boyaFiresiUrFiyText, konfMaliyetiUrFiyText, _ikinciKaliyeMaliyetiUrFiyText, karUrFiyText, kdvUrFiyText, kurUrFiyText, pariteUrFiyText, eurUrFiyText;
        partial void OnAtkiUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    AtkiUrFiy = d;
                }
            }
        }
        partial void OnCozguUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    CozguUrFiy = d;
                }
            }
        }
        partial void OnParcaYikamaUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    ParcaYikamaUrFiy = d;
                }
            }
        }
        partial void OnKumasBoyamaUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    KumasBoyamaUrFiy = d;
                }
            }
        }
        partial void OnDokumaFiresiUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    DokumaFiresiUrFiy = d;
                }
            }
        }
        partial void OnBoyaFiresiUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    BoyaFiresiUrFiy = d;
                }
            }
        }
        partial void OnKonfMaliyetiUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    KonfMaliyetiUrFiy = d;
                }
            }
        }
        partial void OnIkinciKaliyeMaliyetiUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    IkinciKaliyeMaliyetiUrFiy = d;
                }
            }
        }
        partial void OnKarUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    KarUrFiy = d;
                }
            }
        }
        partial void OnKdvUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    KdvUrFiy = d;
                }
            }
        }
        partial void OnKurUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    KurUrFiy = d;
                }
            }
        }
        partial void OnPariteUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    PariteUrFiy = d;
                }
            }
        }
        partial void OnEurUrFiyTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    EurUrFiy = d;
                }
            }
        }
        partial void OnAtkiUrFiyChanged(double value)
        {
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
        }
        partial void OnKurUrFiyChanged(double value)
        {
            DokumaDokMal = (((HamBoy + BoySacak) / 100) * ((Atki1Siklik + Atki2Siklik + Atki3Siklik + Atki4Siklik) * 1.05) * AtkiUrFiy) / KurUrFiy;
            KarliTLDikUr = KarliDikUr * (1 + (KarUrFiy / 100));
            BelirlenenFiyatTL = BelirlenenFiyat * KurUrFiy;
            KdvliBelirlenenFiyatTL = KdvliBelirlenFiyat + KurUrFiy;
        }
        partial void OnCozguUrFiyChanged(double value) => CozguDokMal = (HamBoy / 100) * CozguUrFiy;
        partial void OnDokumaFiresiUrFiyChanged(double value) => FireliUrMal = (ToplamUrMal * (DokumaFiresiUrFiy / 100)) + ToplamUrMal;
        partial void OnKarUrFiyChanged(double value)
        {
            KarliHamKumMal = (FireliUrMal * (KarUrFiy / 100)) + FireliUrMal;
            KarliYBM = (FireliYBM * (KarUrFiy / 100)) + FireliYBM;
            KarliDikUr = (IkinciKaliteMaliyetDikUr * (KarUrFiy / 100)) + IkinciKaliteMaliyetDikUr;
            KarliTLDikUr = KarliDikUr * KurUrFiy;
        }
        partial void OnParcaYikamaUrFiyChanged(double value) => ParcaYikamaYBM = ParcaYikamaUrFiy * ToplamGramaj;
        partial void OnEurUrFiyChanged(double value)
        {
            BoyanmisKumasYBM = (EurUrFiy * ToplamGramaj) + FireliUrMal;
            BoyanmisKumasTlYBM = BoyanmisKumasYBM * KurUrFiy;
        }
        partial void OnBoyaFiresiUrFiyChanged(double value)
        {
            FireliYBM = ((ParcaYikamaYBM + BoyanmisKumasYBM) * (BoyaFiresiUrFiy / 100)) + ParcaYikamaYBM + BoyanmisKumasYBM;
        }
        partial void OnKonfMaliyetiUrFiyChanged(double value) => KonfMaliyetiDikUr = BoyaliKumasDikUr + KonfMaliyetiUrFiy;
        partial void OnIkinciKaliyeMaliyetiUrFiyChanged(double value) => IkinciKaliteMaliyetDikUr = (KonfMaliyetiDikUr * (IkinciKaliyeMaliyetiUrFiy / 100)) + KonfMaliyetiDikUr;
        partial void OnKdvUrFiyChanged(double value)
        {
            KdvliDikUr = (KarliDikUr * (KdvUrFiy / 100)) + KarliDikUr;
            KdvliBelirlenFiyat = (BelirlenenFiyat * (KdvUrFiy / 100)) + BelirlenenFiyat;
        }

        /********************************* MALİYET HESAPLAMA - DOKUMA MALİYETİ **********************************/
        [ObservableProperty]
        private double dokumaDokMal, cozguDokMal, iplikMaliyetDokMal;
        partial void OnDokumaDokMalChanged(double value) => ToplamUrMal = DokumaDokMal + CozguDokMal + IplikMaliyetDokMal;
        partial void OnCozguDokMalChanged(double value) => ToplamUrMal = DokumaDokMal + CozguDokMal + IplikMaliyetDokMal;
        partial void OnIplikMaliyetDokMalChanged(double value) => ToplamUrMal = DokumaDokMal + CozguDokMal + IplikMaliyetDokMal;

        /********************************* MALİYET HESAPLAMA - ÜRETİM MALİYET  **********************************/
        [ObservableProperty]
        private double toplamUrMal, fireliUrMal;
        partial void OnToplamUrMalChanged(double value) => FireliUrMal = (ToplamUrMal * (DokumaFiresiUrFiy / 100)) + ToplamUrMal;
        partial void OnFireliUrMalChanged(double value)
        {
            KarliHamKumMal = (FireliUrMal * (KarUrFiy / 100)) + FireliUrMal;
            BoyanmisKumasYBM = (EurUrFiy * ToplamGramaj) + FireliUrMal;
        }


        /********************************* MALİYET HESAPLAMA - HAM KUMAŞ MALİYET  **********************************/
        [ObservableProperty]
        private double karliHamKumMal, karliHamKumMalTL;
        partial void OnKarliHamKumMalChanged(double value) => KarliHamKumMalTL = KurUrFiy * KarliHamKumMal;

        /********************************* MALİYET HESAPLAMA - YIKAMA VE BOYAMA MALİYET  **********************************/
        [ObservableProperty]
        private double parcaYikamaYBM, boyanmisKumasYBM, boyanmisKumasTlYBM, fireliYBM, karliYBM;
        partial void OnBoyanmisKumasYBMChanged(double value)
        {
            BoyanmisKumasTlYBM = BoyanmisKumasYBM * KurUrFiy;
            FireliYBM = ((ParcaYikamaYBM + BoyanmisKumasYBM) * (BoyaFiresiUrFiy / 100)) + ParcaYikamaYBM + BoyanmisKumasYBM;
        }
        partial void OnParcaYikamaYBMChanged(double value)
        {
            FireliYBM = ((ParcaYikamaYBM + BoyanmisKumasYBM) * (BoyaFiresiUrFiy / 100)) + ParcaYikamaYBM + BoyanmisKumasYBM;
        }
        partial void OnFireliYBMChanged(double value)
        {
            KarliYBM = (FireliYBM * (KarUrFiy / 100)) + FireliYBM;
            BoyaliKumasDikUr = FireliYBM;
        }

        /********************************* MALİYET HESAPLAMA - DİKİLMİŞ ÜRÜN  **********************************/
        [ObservableProperty]
        private double boyaliKumasDikUr, konfMaliyetiDikUr, ikinciKaliteMaliyetDikUr, karliDikUr, karliTLDikUr, kdvliDikUr, kdvliTLDikUr;
        partial void OnBoyaliKumasDikUrChanged(double value) => KonfMaliyetiDikUr = BoyaliKumasDikUr + KonfMaliyetiUrFiy;

        partial void OnKonfMaliyetiDikUrChanged(double value)
        {
            IkinciKaliteMaliyetDikUr = (KonfMaliyetiDikUr * (IkinciKaliyeMaliyetiUrFiy / 100)) + KonfMaliyetiDikUr;
        }
        partial void OnIkinciKaliteMaliyetDikUrChanged(double value)
        {
            KarliDikUr = (IkinciKaliteMaliyetDikUr * (KarUrFiy / 100)) + IkinciKaliteMaliyetDikUr;
        }
        partial void OnKarliDikUrChanged(double value)
        {
            KarliTLDikUr = KarliDikUr * KurUrFiy;
            KdvliDikUr = (KarliDikUr * (KdvUrFiy / 100)) + KarliDikUr;
        }
        partial void OnKdvliDikUrChanged(double value)
        {
            KdvliTLDikUr = KdvliDikUr * KurUrFiy;
        }

        /********************************* BELİRLENMİŞ FİYATLAR  **********************************/
        [ObservableProperty]
        private double belirlenenFiyat, belirlenenFiyatTL, kdvliBelirlenFiyat, kdvliBelirlenenFiyatTL;
        [ObservableProperty]
        private string belirlenenFiyatText;
        partial void OnBelirlenenFiyatTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string s = value.Replace(',', '.');
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                {
                    BelirlenenFiyat = d;
                }
            }
        }
        partial void OnBelirlenenFiyatChanged(double value)
        {
            BelirlenenFiyatTL = BelirlenenFiyat * KurUrFiy;
            KdvliBelirlenFiyat = (BelirlenenFiyat * (KdvUrFiy / 100)) + BelirlenenFiyat;
        }
        partial void OnKdvliBelirlenFiyatChanged(double value)
        {
            KdvliBelirlenenFiyatTL = KdvliBelirlenFiyat * KurUrFiy;
        }

        /********************************* BELİRLENMİŞ FİYATLAR  **********************************/
        [ObservableProperty]
        private double endenCekmesi;
    }
}