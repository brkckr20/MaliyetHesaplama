# Yapılacaklar

## ✅ Düzenlemeler (02.06.2026)

- [x] "📦 Malzeme Yönetimi" menüsü kaldırıldı (XAML + code-behind)
- [x] `UC_MalzemeGirisCikis.xaml` ve `.xaml.cs` silindi (artık referans edilmiyor)
- [x] `winRaporSecimi.xaml.cs` — ReportApp.exe yolu düzeltildi (`\\` → `\`)
- [x] `ReportApp/Program.cs` — try-catch eklendi, null + dosya kontrolleri eklendi
- [x] `Toner Çıkış Formu.frx` — FastReport IIF ifadesinde `''` → `""` düzeltildi
- [x] "🚀 v2" menü adı "🚀 v2 Kartlar" olarak güncellendi (XAML + code-behind)
- [x] "Firma Kartı" Kartlar menüsünden çıkarılıp v2 Kartlar altına taşındı

## ✅ Temizlik (01.06.2026)

- [x] `csproj`'dan silinmiş resim referansları temizlendi (Building.png, Filter_1.png, Libre Office Calc.png, Purchase Order.png, Towel.png)
- [x] Kullanılmayan kök dizin .png dosyaları silindi (Building.png, Filter_1.png, Libre Office Calc.png, Towel.png)
- [x] `PROJE_GELISTIRME_RAPORU.md` silindi
- [x] `v2/UserControls/alters.txt` → kök dizine taşındı
- [ ] `obj/` — 243 adet `*_wpftmp.csproj*` build artığı temizlenecek
- [ ] `MaliyeHesaplama.csproj.Backup.tmp` silinecek

## 🔄 UC_MalzemeKartiV2 - Renk / Beden Sekmesi

- [ ] XAML'deki boş `<TabItem Header="Renk / Beden">` içeriği sıfırdan düzenlenecek
- [ ] Code-behind sıfırlandı (22.05.2026), yeniden yazılacak
