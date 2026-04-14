# Products Modülü

Ürün tanımlarını saklar.

## Products (Ürünler)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ProductCode | NVARCHAR | Ürün kodu |
| Name | NVARCHAR | Ürün adı |
| ProductType | INT | Ürün tipi |
| UnitId | INT | Birim |

---

## ProductTypes (Ürün Tipleri)

| Değer | Açıklama |
|-------|----------|
| 1 | Ham Kumaş (RawFabric) |
| 2 | Boyalı Kumaş (DyedFabric) |
| 3 | Aksesuar (Accessory) |
| 4 | Mamul (FinishedProduct) |

---

## Opsiyonel Tekstil Alanları

| Alan | Tür | Açıklama |
|------|-----|----------|
| ColorId | INT | Renk |
| VariantId | INT | Varyant |
| LotTrackingEnabled | BIT | Lot takibi aktif |