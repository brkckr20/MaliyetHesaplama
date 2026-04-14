# Purchasing Modülü

Hammadde satın almalarını yönetir.

## PurchaseOrders (Satın Alma Siparişleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| SupplierId | INT | Tedarikçi |
| OrderDate | DATETIME | Sipariş tarihi |
| Status | INT | Durum |

---

## PurchaseOrderLines (Satın Alma Sipariş Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| OrderId | INT | Sipariş |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## PurchaseReceipts (Satın Alma Kabul Belgeleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| SupplierId | INT | Tedarikçi |
| ReceiptDate | DATETIME | Kabul tarihi |

---

## PurchaseReceiptLines (Satın Alma Kabul Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ReceiptId | INT | Kabul |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketi

`Tedarikçi → Depo`