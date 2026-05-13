# v2 Stock Sistemi Basitleştirme

## Tamamlananlar

### Stock Sistemi Basitleştirme
- [x] Stock.cs silindi (fiziksel tablo yoktu, [NotMapped] idi)
- [x] StockMovement.cs ve StockMovementRepository.cs silindi
- [x] StockRepository.cs Güncellendi: StockSummary DTO eklendi
- [x] winStokSecimiV2.xaml.cs: Stock yerine StockSummary kullanılıyor
- [x] tables.txt'ye DROP TABLE komutları eklendi

### Fiş Listesi Excel Aktarım
- [x] winFisListesiV2.xaml: Excel Aktar butonu eklendi
- [x] winFisListesiV2.xaml.cs: ExportToExcel metodu eklendi
- [x] Context menu'ye Excel Aktar eklendi

---

## Yapılacaklar

### 1. Fiş Listesi Sorgu Performansı
- Sorgu çok satırlı dönüyor (her kalem için)
- Indexed View veya Receipt bazlı sorgu düşünülebilir

### 2. İleride Basitleştirilecek
- [ ] GetStock metodu ile anlık stok hesaplama
- [ ] Raporlama ekle (depo/malzeme bazlı)