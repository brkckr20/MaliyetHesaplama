# Textile ERP – Complete Database Architecture

## Overview

This document describes a modular ERP database architecture designed for textile businesses.

The system supports:

* Fabric stock tracking
* Dyeing subcontractors (fason)
* Production processes
* Multi-warehouse management
* Sales and purchasing
* Inventory traceability

---

# System Modules

The ERP is divided into **12 main modules**:

1. Master Data
2. Products
3. Warehouses
4. Stock Management
5. Purchasing
6. Sales
7. Warehouse Transfers
8. Fason (Subcontractor Management)
9. Dyeing Process
10. Production
11. Inventory Counting
12. Reporting / Archive

---

# 1. Master Data Module

Stores fundamental system entities.

### Tables

Master data tables:

```
Companies
Customers
Suppliers
Units
Colors
Variants
Lots
Users
Roles
```

Example structure:

Companies

* Id
* Name
* TaxNumber

Customers

* Id
* Name
* Phone

Suppliers

* Id
* Name
* Address

---

# 2. Products Module

Stores product definitions.

Products

* Id
* ProductCode
* Name
* ProductType
* UnitId

ProductTypes:

* RawFabric
* DyedFabric
* Accessory
* FinishedProduct

Optional textile attributes:

* ColorId
* VariantId
* LotTrackingEnabled

---

# 3. Warehouses Module

Stores physical stock locations.

Warehouses

* Id
* Name
* WarehouseType

Warehouse types:

* MainWarehouse
* DyeingFason
* CuttingUnit
* ProductionUnit
* SalesWarehouse

---

# 4. Stock Management Module

Core stock architecture.

### Tables

StockMovements
Stock

StockMovements (critical table)

Fields:

* Id
* ProductId
* WarehouseId
* LotId
* ColorId
* Quantity
* MovementType
* DocumentType
* DocumentId
* DocumentLineId
* CreatedAt

Quantity rules:

Positive = stock increase
Negative = stock decrease

Stock snapshot table:

Stock

Fields:

* ProductId
* WarehouseId
* LotId
* Quantity

---

# 5. Purchasing Module

Handles raw material purchases.

PurchaseOrders

* Id
* SupplierId
* OrderDate
* Status

PurchaseOrderLines

* Id
* OrderId
* ProductId
* Quantity

PurchaseReceipts

* Id
* SupplierId
* ReceiptDate

PurchaseReceiptLines

* Id
* ReceiptId
* ProductId
* Quantity

Stock movement created:

Supplier → Warehouse

---

# 6. Sales Module

Handles customer orders and shipments.

SalesOrders

* Id
* CustomerId
* OrderDate
* Status

SalesOrderLines

* Id
* OrderId
* ProductId
* Quantity

Shipments

* Id
* CustomerId
* ShipmentDate

ShipmentLines

* Id
* ShipmentId
* ProductId
* Quantity

Stock movement created:

Warehouse → Customer

---

# 7. Warehouse Transfer Module

Handles internal warehouse transfers.

Transfers

* Id
* FromWarehouseId
* ToWarehouseId
* Date

TransferLines

* Id
* TransferId
* ProductId
* Quantity

Stock movements created:

FromWarehouse → -Quantity
ToWarehouse → +Quantity

---

# 8. Fason Module

Manages subcontractor operations.

FasonPartners

* Id
* Name
* Address

FasonSendDocuments

* Id
* FasonPartnerId
* Date

FasonSendLines

* Id
* DocumentId
* ProductId
* Quantity

Stock movements:

MainWarehouse → -Quantity
FasonWarehouse → +Quantity

---

# 9. Dyeing Module

Tracks dyeing operations.

DyeOrders

* Id
* FasonPartnerId
* ColorId
* Date

DyeOrderLines

* Id
* DyeOrderId
* ProductId
* Quantity

DyeReturns

* Id
* DyeOrderId
* ReturnDate

DyeReturnLines

* Id
* ReturnId
* ProductId
* Quantity

Stock movements:

FasonWarehouse → -Quantity
MainWarehouse → +Quantity

---

# 10. Production Module

Tracks cutting and manufacturing.

ProductionOrders

* Id
* ProductId
* PlannedQuantity
* Status

ProductionMaterials

* Id
* ProductionOrderId
* ProductId
* Quantity

ProductionOutputs

* Id
* ProductionOrderId
* ProductId
* Quantity

Stock movements:

RawMaterialWarehouse → -Quantity
FinishedGoodsWarehouse → +Quantity

---

# 11. Inventory Counting Module

Handles stock counting operations.

InventoryCounts

* Id
* WarehouseId
* CountDate

InventoryCountLines

* Id
* CountId
* ProductId
* SystemQuantity
* CountedQuantity

Adjustment logic:

Difference = Counted - System

StockMovement created for difference.

---

# 12. Reporting / Archive Module

Stores archived historical data.

Archive tables:

StockMovements_2023
StockMovements_2024
StockMovements_2025

Used for:

* performance optimization
* historical reporting

---

# Core ERP Rules

1. Stock must be movement-based
2. StockMovements must never be edited
3. Corrections must create difference movements
4. All operations must use database transactions
5. Stock snapshot table must be updated with movements

---

# Performance Indexes

Recommended indexes:

StockMovements(ProductId)
StockMovements(WarehouseId)
StockMovements(CreatedAt)

---

# Final Architecture Principle

ERP inventory system must separate:

Business Documents
User Editable Lines
Stock Movement History
Stock Snapshot
