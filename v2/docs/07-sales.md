# Sales Modülü

Müşteri siparişlerini ve sevkiyatlarını yönetir.

## SalesOrders (Satış Siparişleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| CustomerId | INT | Müşteri |
| OrderDate | DATETIME | Sipariş tarihi |
| Status | INT | Durum |

---

## SalesOrderLines (Satış Sipariş Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| OrderId | INT | Sipariş |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Shipments (Sevkiyatlar)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| CustomerId | INT | Müşteri |
| ShipmentDate | DATETIME | Sevk tarihi |

---

## ShipmentLines (Sevk Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| ShipmentId | INT | Sevk |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketi

`Depo → Müşteri`