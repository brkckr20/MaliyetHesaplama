# Stock Management Modülü

Çekirdek stok mimarisi.

## Tablolar

- StockMovements (Stok Hareketleri)
- Stock (Stok Anlık Görüntüsü)

---

## StockMovements (Stok Hareketleri) - Kritik Tablo

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ProductId | INT | Ürün |
| WarehouseId | INT | Depo |
| LotId | INT | Lot |
| ColorId | INT | Renk |
| Quantity | DECIMAL | Miktar |
| MovementType | NVARCHAR | Hareket tipi |
| DocumentType | NVARCHAR | Belge tipi |
| DocumentId | INT | Belge Id |
| DocumentLineId | INT | Belge satır Id |
| CreatedAt | DATETIME | Oluşturulma tarihi |

---

## Miktar Kuralları

- **Pozitif** = Stok artışı
- **Negatif** = Stok azalışı

---

## Stock (Stok Anlık Görüntü)

| Alan | Tür | Açıklama |
|------|-----|----------|
| ProductId | INT | Ürün (PK) |
| WarehouseId | INT | Depo (PK) |
| LotId | INT | Lot (PK) |
| Quantity | DECIMAL | Miktar |

---

## winStokSecimiV2 (Stok Seçimi Penceresi)

Malzeme çıkışı yapılırken depodaki stokları listelemek için kullanılan pencere.

### Özellikler
- FilterDataGrid ile filtreleme (winDepoListesi pattern)
- Context menu: Kolon Seçicisi, Excel Aktar, Seçili Satırı Aktar
- Aynı malzeme tek satırda toplanır (GROUP BY)
- Kalan = Giriş - Çıkış (TrackingNumber ile eşleştirme)
- Aktarımda en küçük Id kullanılır (FirstId)

### Sorgu (StockRepository.GetByWarehouseId)

```sql
;WITH StokData AS (
    SELECT
        giris.Id,
        giris.InventoryId,
        i.InventoryCode,
        i.InventoryName,
        i.VatRate,
        giris.UnitPrice,
        giris.Piece AS GirisAdet,
        rec.WareHouseId,
        rec.ReceiptDate,
        ISNULL(giris.Piece, 0) - ISNULL((...çıkış...), 0) AS Kalan
    FROM ReceiptItem giris
    INNER JOIN Receipt rec ON giris.ReceiptId = rec.Id
    INNER JOIN Inventory i ON giris.InventoryId = i.Id
    WHERE rec.ReceiptType = 1 AND giris.Piece > 0
)
SELECT
    InventoryId, InventoryCode, InventoryName,
    SUM(GirisAdet) AS GirisAdet,
    SUM(Kalan) AS QuantityPiece,
    MAX(VatRate) AS Vat,
    UnitPrice (en küçük Id'li),
    Ids (virgülle ayrılmış ID'ler),
    Tarihler (virgülle ayrılmış tarihler)
FROM StokData
WHERE WareHouseId = @depoId
GROUP BY ...
HAVING SUM(Kalan) > 0
```

### Kolonlar
| Kolon | Açıklama |
|-------|----------|
| Kodu | Malzeme kodu |
| Adı | Malzeme adı |
| Kalan | Toplam kalan miktar |
| ID'ler | Tüm giriş ID'leri (virgülle) |
| Tarihler | Tüm giriş tarihleri (virgülle) |
| Giriş Id | En küçük ID (aktarımda kullanılır) |
| UnitPrice | Birim fiyat (gizli) |
| Vat | KDV oranı (gizli) |
| GirisAdet | Toplam giriş adedi (gizli) |

### Dosyalar
- `v2/Windows/winStokSecimiV2.xaml` - XAML
- `v2/Windows/winStokSecimiV2.xaml.cs` - Code-behind
- `v2/Data/StockRepository.cs` - GetByWarehouseId metodu