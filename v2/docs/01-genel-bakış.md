# Textile ERP - Genel Bakış

## Sistem Özellikleri

Bu doküman kumaş stok takibi için modüler ERP veritabanı mimarisini tanımlar.

Desteklenen özellikler:

- Kumaş stok takibi
- Boyama tedarikçileri (fason)
- Üretim süreçleri
- Çoklu depo yönetimi
- Satış ve satın alma
- Stok izlenebilirliği

---

## 12 Ana Modül

1. **Master Data** - Temel veriler
2. **Products** - Ürünler
3. **Warehouses** - Depolar
4. **Stock Management** - Stok yönetimi
5. **Purchasing** - Satın alma
6. **Sales** - Satış
7. **Warehouse Transfers** - Depo transferleri
8. **Fason** - Tedarikçi yönetimi
9. **Dyeing** - Boyama süreci
10. **Production** - Üretim
11. **Inventory Counting** - Stok sayımı
12. **Reporting / Archive** - Raporlama / Arşiv

---

## Temel ERP Kuralları

1. Stok hareket tabanlı olmalı
2. StockMovements asla düzenlenemez
3. Düzeltmeler fark hareketi oluşturur
4. Tüm operasyonlar transaction kullanır
5. Stock snapshot tablosu hareketlerle güncellenir

---

## Performans İndeksleri

Önerilen indeksler:

- StockMovements(ProductId)
- StockMovements(WarehouseId)
- StockMovements(CreatedAt)

---

## Mimari Prensip

ERP envanter sistemi ayırmalıdır:

- İş Belgeleri (Business Documents)
- Kullanıcı Düzenlenebilir Satırlar (User Editable Lines)
- Stok Hareket Geçmişi (Stock Movement History)
- Stok Anlık Görüntü (Stock Snapshot)