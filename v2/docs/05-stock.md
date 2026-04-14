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