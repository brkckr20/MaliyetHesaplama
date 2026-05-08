# Yapılacaklar

## ✅ Stok Seçimi (Tamamlandı - 08.05.2026)

### Yapılan İşlemler
- StockRepository.GetByWarehouseId güncellendi
- Giriş-Çıkış takibi eklendi (TrackingNumber ile)
- Kalan hesaplama: Giriş Adet - Çıkış Toplamı
- Tarih (ReceiptDate) eklendi
- FIFO sıralama (ORDER BY Id ASC)
- 0 kalan stoklar filtrelendi

### Dosyalar
- `v2/Data/StockRepository.cs`
- `v2/Models/Stock.cs`
- `v2/Windows/winStokSecimiV2.xaml.cs`
- `v2/Windows/winStokSecimiV2.xaml`

---

## 📝 Loglama Hatası (Düzeltilecek)

### Sorun
- Mevcut kalem güncellendiğinde (ör: 10-10-10 → 20-30-40) önceki kayıt "Silme" olarak kaydediliyor
- Beklenen: Mevcut kalem güncellendiğinde "Güncelleme" olarak kaydedilmeli

### Örnek Akış
1. Yeni kalem ekle (10 kg, 10 mt, 10 adet) → "Yeni Kayıt" ✓
2. Aynı kalemi güncelle (20 kg, 30 mt, 40 adet) → "Güncelleme" olmalı
3. Mevcut kayıt silik olarak görünüyor → "Silme" yazıyor ✗

### Mantık Düzeltme
- `item.Id > 0` (mevcut kalem) → "Güncelleme"
- `item.Id == 0` (yeni kalem) → "Yeni Kayıt"
- Silme işlemi → "Silme" (doğru)

---

## 📝 Kaldırılacak Tablolar
- Stock
- StockMovement
- StockTransaction

---

## 📝 MiniOrm Notu
MiniOrm dosyası hiç bozulmamıştı (kullanıcı isteği), ama ileride transaction desteği gerekebilir.