# Fiş Başlık Tasarımı

## Yapılanlar

### 1. GroupBox Yapısı
- [x] Başlık Bilgileri GroupBox
  - Fiş No
  - Tarih
  - İrsaliye Tarihi
  - Belge No
  - İrsaliye Belge No

- [x] Firma ve Depo Bilgileri GroupBox
  - Firma Kodu (SearchButton ile 🔍)
  - Depo Kodu (SearchButton ile 🔍)
  - Seçim sonrası TextBlock ile isim gösterimi

- [x] Açıklama alanı

### 2. UI Elementleri
- [x] Büyüteç (🔍) butonu - TextBox içinde sağda
- [x] White background - yazı örtmemesi için
- [x] Hover efekti kaldırıldı
- [x] 1-2px sola kaydırma

---

## Yapılacak

### 1. Code-Behind
- [ ] btnFirma_Click → Firma seçim penceresi açılması
- [ ] btnDepo_Click → Depo seçim penceresi açılması
- [ ] Seçim sonrası lblFirmaAdi doldurulması
- [ ] Seçim sonrası lblDepoAdi doldurulması
- [ ] FirmaId ve DepoId değişkenlerinin tutulması

### 2. Veritabanı Entegrasyonu
- [ ] Receipt kaydetme
- [ ] ReceiptItem kaydetme
- [ ] StockMovement oluşturma

### 3. Kalemler (DataGrid)
- [ ] Malzeme seçim penceresi
- [ ] RowAmount hesaplama
- [ ] İrsaliye Belge No alanı ekleme

### 4. Yazdırma
- [ ] Rapor oluşturma

---

## Element Yapısı (XAML)

```
Grid (Üst Bilgi)
├── Başlık Bilgileri GroupBox (Grid.Row=0)
│   ├── Fiş No
│   ├── Tarih
│   ├── İrsaliye Tarihi
│   ├── Belge No
│   └── İrsaliye Belge No
├── Firma ve Depo Bilgileri GroupBox (Grid.Row=1)
│   ├── Firma Kodu + 🔍 + lblFirmaAdi
│   └── Depo Kodu + 🔍 + lblDepoAdi
└── Açıklama (Grid.Row=2)

DataGrid (Kalemler)
Button (+ Malzeme Ekle)
Button (- Satır Sil)
```