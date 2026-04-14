# Warehouses Modülü

Fiziksel stok lokasyonlarını saklar.

## Warehouses (Depolar)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| Name | NVARCHAR | Depo adı |
| WarehouseType | INT | Depo tipi |

---

## WarehouseTypes (Depo Tipleri)

| Değer | Açıklama |
|-------|----------|
| 1 | Ana Depo (MainWarehouse) |
| 2 | Boyama Fason (DyeingFason) |
| 3 | Kesim Ünitesi (CuttingUnit) |
| 4 | Üretim Ünitesi (ProductionUnit) |
| 5 | Satış Deposu (SalesWarehouse) |