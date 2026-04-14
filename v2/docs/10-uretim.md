# Production Modülü

Kesim ve üretim süreçlerini takip eder.

## ProductionOrders (Üretim Siparişleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ProductId | INT | Ürün |
| PlannedQuantity | DECIMAL | Planlanan miktar |
| Status | INT | Durum |

---

## ProductionMaterials (Üretim Malzemeleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ProductionOrderId | INT | Üretim siparişi |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## ProductionOutputs (Üretim Çıktıları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ProductionOrderId | INT | Üretim siparişi |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketleri

- `RawMaterialWarehouse → -Quantity` (Hammadde çıkışı)
- `FinishedGoodsWarehouse → +Quantity` (Mamul girişi)