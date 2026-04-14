# Inventory Counting Modülü

Stok sayım operasyonlarını yönetir.

## InventoryCounts (Stok Sayımları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| WarehouseId | INT | Depo |
| CountDate | DATETIME | Sayım tarihi |

---

## InventoryCountLines (Stok Sayım Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| CountId | INT | Sayım |
| ProductId | INT | Ürün |
| SystemQuantity | DECIMAL | Sistem miktarı |
| CountedQuantity | DECIMAL | Sayılan miktar |

---

## Düzeltme Mantığı

`Fark = Sayılan - Sistem`

Stok hareketi fark için oluşturulur.