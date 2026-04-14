# Dyeing (Boyama) Modülü

Boyama süreçlerini takip eder.

## DyeOrders (Boyama Siparişleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| FasonPartnerId | INT | Fason partner |
| ColorId | INT | Renk |
| Date | DATETIME | Tarih |

---

## DyeOrderLines (Boyama Sipariş Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| DyeOrderId | INT | Boyama siparişi |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## DyeReturns (Boyama İadeleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| DyeOrderId | INT | Boyama siparişi |
| ReturnDate | DATETIME | İade tarihi |

---

## DyeReturnLines (Boyama İade Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ReturnId | INT | İade |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketleri

- `FasonWarehouse → -Quantity` (Gönderim)
- `MainWarehouse → +Quantity` (Dönüş)