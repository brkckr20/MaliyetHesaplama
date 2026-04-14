# Fason Modülü

Tedarikçi ( subcontractor) operasyonlarını yönetir.

## FasonPartners (Fason Partnerler)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| Name | NVARCHAR | Fason adı |
| Address | NVARCHAR | Adres |

---

## FasonSendDocuments (Fason Gönderim Belgeleri)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| FasonPartnerId | INT | Partner |
| Date | DATETIME | Tarih |

---

## FasonSendLines (Fason Gönderim Satırları)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| DocumentId | INT | Belge |
| ProductId | INT | Ürün |
| Quantity | DECIMAL | Miktar |

---

## Stok Hareketleri

- `MainWarehouse → -Quantity` (Gönderim)
- `FasonWarehouse → +Quantity` (Alış)