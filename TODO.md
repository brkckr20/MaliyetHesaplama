# Malzeme Giriş/Çıkış - Renk/Beden Entegrasyonu

## Hedef
ReceiptItem'da malzeme seçildiğinde, o malzemenin tanımlı Renk/Beden'leri DataGrid'e dinamik olarak ekransın.

## Yapılacaklar

### 1. UC_MalzemeGirisCikis.xaml
- [ ] DataGrid sütunlarını dinamik oluşturacak şekilde düzenle
- [ ] Malzeme Kodu sütunu → seçim sonrası Renk/Beden kolonları ekle

### 2. UC_MalzemeGirisCikis.xaml.cs
- [ ] Malzeme seçim event'i → Variant tablosundan Renk/Beden çek
- [ ] `LoadDynamicColumns(inventoryId)` metodu oluştur
  - Renkleri kolon olarak ekle
  - Bedenleri satır olarak ekle (veya tersi)
- [ ] Her hücre → ColorId + VariantId binding içerir

### 3. DataTable/Model Yapısı
- [ ] ReceiptItem tablosuna ColorId ve VariantId alanları eklenmeli
- [ ] Veya ayrı bir Detay tablosu

### 4. Kaydet() Metodu
- [ ] Transaction içinde çalışacak şekilde düzenle
- [ ] ReceiptItem + Stock güncelleme tek transaction'da

### 5. StokServisi (Yeni)
- [ ] `GirisYap(inventoryId, colorId, variantId, wareHouseId, kg, meter, adet)`
- [ ] `CikisYap(inventoryId, colorId, variantId, wareHouseId, kg, meter, adet)`
- [ ] Eksiye düşme kontrolü

## Tablo Yapısı

```
Variant: InventoryId, RenkId, BedenId, Barkod, Fiyat, Aktif
Stock:   InventoryId, ColorId, VariantId, WareHouseId, QuantityKg, QuantityMeter, QuantityPiece
```

## Notlar
- MiniOrm.cs → Stock tablosuna ColorId eklendi (satır 684, 702)
- Renk ve Beden tabloları database'de mevcut

---

# UC_MalzemeFisV2 - Boş Satır Sorunu

## Sorun
`LoadReceipt()` metodunda kayıt yüklenirken boş satır eklenmiyor. İleri/Geri navigasyon sonrası grid'de boş satır kayboluyor.

## Çözüm
`LoadReceipt()` ve `Listele()` metodlarında foreach döngüsü **biter bitmez** değil, tüm kalemler eklendikten **sonra** en alta boş satır eklenmeli.

### Örnek Düzeltme:
```csharp
// Mevcut (hatalı - her iteration'da ekliyor)
foreach (var item in items)
{
    _items.Add(...);
    _items.Add(new ReceiptItemViewModel()); // ❌ Yanlış
}

// Doğru
foreach (var item in items)
{
    _items.Add(...);
}
_items.Add(new ReceiptItemViewModel()); // ✓ Doğru - döngü sonunda
```