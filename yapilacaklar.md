# Yapılacaklar

## ✅ v2 Malzeme Kartı - FilterDataGrid Filtreleme (Çözüldü)

### Sorun
- `winMalzemeListesiV2` penceresinde FilterDataGrid filtreleme çalışmıyor
- Sütun başlıklarına tıklanınca "Boş" ve "Hepsini Seç" seçenekleri geliyor ama veri filtrelenmiyor

### Örnek (Çalışan)
- `winDepoListesi` - FilterDataGrid doğru çalışıyor, `CollectionViewSource.GetDefaultView` kullanıyor

### Yapılacak
- FilterDataGrid'in neden çalışmadığını analiz et
- winDepoListesi ile karşılaştır (farklılıkları bul)
- Çözüm uygula

---

## ✅ Yapılanlar - Login Ekranı

### winLogin.xaml İyileştirmeleri
- Header (36px) eklendi - #2d5a56 rengi
- Header üzerinden sürükleme özelliği eklendi
- Kapatma (✕) butonu eklendi (hover efektsiz)
- Giriş butonu rengi #2d5a56 yapıldı
- Material Design tarzı modern görünüm
- Border-radius ve gölge efektleri

### winLogin.xaml.cs Değişiklikleri
- `Header_MouseLeftButtonDown` eklendi (sürükleme)
- `BtnCancel_Click` eklendi (kapatma)

---

## 📝 MiniOrm Notu
MiniOrm dosyası hiç bozulmamıştı (kullanıcı isteği), ama ileride transaction desteği gerekebilir.
