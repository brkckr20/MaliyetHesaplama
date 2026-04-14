# Master Data Modülü

Sistem'in temel veri tablolarını saklar.

## Tablolar

- Companies (Firmalar)
- Customers (Müşteriler)
- Suppliers (Tedarikçiler)
- Units (Birimler)
- Colors (Renkler)
- Variants (Varyantlar)
- Lots (Lotlar)
- Users (Kullanıcılar)
- Roles (Roller)

---

## Companies (Firmalar)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| Name | NVARCHAR | Firma adı |
| TaxNumber | NVARCHAR | Vergi numarası |

---

## Customers (Müşteriler)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| Name | NVARCHAR | Müşteri adı |
| Phone | NVARCHAR | Telefon |

---

## Suppliers (Tedarikçiler)

| Alan | Tür | Açıklama |
|------|-----|----------|
| Id | INT | Primary Key |
| Name | NVARCHAR | Tedarikçi adı |
| Address | NVARCHAR | Adres |