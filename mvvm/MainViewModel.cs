using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace MaliyeHesaplama.mvvm
{
    public partial class MainViewModel : ObservableObject
    {
        /* 09.09.2025 üretim hesaplama ekranındaki atki2 hesaplamasından devam edilecek */
        public MainViewModel()
        {
            Cozgu1IpBilBolen = 1; Cozgu1IpBilBolunen = 1; Cozgu2IpBilBolen = 1; Cozgu2IpBilBolunen = 1; Atki1IpBilBolen = 1; Atki1IpBilBolunen = 1; Atki2IpBilBolen = 1; Atki2IpBilBolunen = 1; Atki3IpBilBolen = 1; Atki3IpBilBolunen = 1; Atki4IpBilBolen = 1; Atki4IpBilBolunen = 1;
            TarakNo1Carpan = 0; TarakNo1Carpim = 0; TarakNo2Carpan = 0; TarakNo2Carpim = 0;
            TarakEn = 0; HamBoy = 0; BoySacak = 0; EnSacak = 0; HamEn = 0; MamulBoy = 0; MamulEn = 0;
            Cozgu1Gramaj = 0.00; Cozgu2Gramaj = 0.00; Atki1Gramaj = 0.00; Atki2Gramaj = 0.00; Atki3Gramaj = 0.00; Atki4Gramaj = 0.00;
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
        partial void OnAtki1IpBilSonucChanged(double value) => Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak)/100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
        partial void OnAtki2IpBilBolenChanged(double value) => Atki2IpBilSonuc = Atki2IpBilBolen / Atki2IpBilBolunen;
        partial void OnAtki2IpBilBolunenChanged(double value) => Atki2IpBilSonuc = Atki2IpBilBolen / Atki2IpBilBolunen;
        partial void OnAtki3IpBilBolenChanged(double value) => Atki3IpBilSonuc = Atki3IpBilBolen / Atki3IpBilBolunen;
        partial void OnAtki3IpBilBolunenChanged(double value) => Atki3IpBilSonuc = Atki3IpBilBolen / Atki3IpBilBolunen;
        partial void OnAtki4IpBilBolenChanged(double value) => Atki4IpBilSonuc = Atki4IpBilBolen / Atki4IpBilBolunen;
        partial void OnAtki4IpBilBolunenChanged(double value) => Atki4IpBilSonuc = Atki4IpBilBolen / Atki4IpBilBolunen;

        /********************************* ÜRETİM BİLGİLERİ - DOKUMA BİLGİLERİ **********************************/
        [ObservableProperty] //değişkenler
        private double tarakNo1Carpan, tarakNo1Carpim, tarakNo1Sonuc, tarakNo2Carpan, tarakNo2Carpim, tarakNo2Sonuc, tarakEn, hamEn, hamBoy, boySacak, enSacak, mamulBoy, mamulEn;
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
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak)/100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
        }
        partial void OnHamBoyChanged(double value)
        {
            Atki1TelSay = HamBoy * Atki1Siklik; Atki2TelSay = HamBoy * Atki2Siklik; Atki3TelSay = HamBoy * Atki3Siklik; Atki4TelSay = HamBoy * Atki4Siklik;
            Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
            Cozgu2Gramaj = (HamBoy * Cozgu2TelSay * (60 / Cozgu2IpBilSonuc) * 1.05 / 10000000);
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak)/100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
        }
        partial void OnBoySacakChanged(double value) => Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
        partial void OnEnSacakChanged(double value) => Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak)/100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;

        /********************************* ÜRETİM BİLGİLERİ - SIKLIKLAR **********************************/
        [ObservableProperty] //değişkenler
        private double cozgu1Siklik, cozgu2Siklik, atki1Siklik, atki2Siklik, atki3Siklik, atki4Siklik;
        partial void OnAtki1SiklikChanged(double value)
        {
            Atki1TelSay = HamBoy * Atki1Siklik;
            Atki1Gramaj = (Atki1Siklik * (TarakEn + EnSacak)/100 * (HamBoy / 100) * (60 / Atki1IpBilSonuc) * 1.05) / 1000;
        }
        partial void OnAtki2SiklikChanged(double value) => Atki2TelSay = HamBoy * Atki2Siklik;
        partial void OnAtki3SiklikChanged(double value) => Atki3TelSay = HamBoy * Atki3Siklik;
        partial void OnAtki4SiklikChanged(double value) => Atki4TelSay = HamBoy * Atki4Siklik;

        /********************************* ÜRETİM BİLGİLERİ - TEL SAYILARI **********************************/
        [ObservableProperty] //değişkenler
        private double cozgu1TelSay, cozgu2TelSay, atki1TelSay, atki2TelSay, atki3TelSay, atki4TelSay;
        partial void OnCozgu1TelSayChanged(double value) => Cozgu1Gramaj = ((HamBoy + BoySacak) * Cozgu1TelSay * (60 / Cozgu1IpBilSonuc) * 1.05 / 10000000);
        partial void OnCozgu2TelSayChanged(double value) => Cozgu2Gramaj = (HamBoy * Cozgu2TelSay * (60 / Cozgu2IpBilSonuc) * 1.05 / 10000000);

        /********************************* ÜRETİM HESAPLAMA - GRAMAJ HESAPLAMA **********************************/
        [ObservableProperty]
        private double cozgu1Gramaj, cozgu2Gramaj, atki1Gramaj, atki2Gramaj, atki3Gramaj, atki4Gramaj, toplamGramaj;
    }
}