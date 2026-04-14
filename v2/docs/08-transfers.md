# Warehouse Transfer Modülü

Dahili depo transferlerini yönetir.

## Transfers (Transferler)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| FromWarehouseId | INT | Gönderen depo |
| ToWarehouseId | INT | Alan depo |
| Date | DATETIME | Tarih |

---

## TransferLines (Transfer Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| TransferId | INT | Transfer |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketleri

- `FromWarehouse → -Quantity` (Azalış)
- `ToWarehouse → +Quantity` (Artış)