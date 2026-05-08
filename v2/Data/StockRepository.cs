using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class StockRepository
    {
        private readonly MiniOrm _orm;

        public StockRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("Stock", data);
        }

        public Stock GetByInventoryAndWarehouse(int inventoryId, int warehouseId)
        {
            var sql = $"SELECT * FROM Stock WHERE InventoryId = {inventoryId} AND WareHouseId = {warehouseId}";
            var result = _orm.QueryRaw<Stock>(sql);
            return result.FirstOrDefault();
        }

        public void UpdateQuantities(int inventoryId, int warehouseId, decimal deltaKg, decimal deltaMeter, int deltaPiece)
        {
            if (inventoryId <= 0 || warehouseId <= 0) return;

            var stock = GetByInventoryAndWarehouse(inventoryId, warehouseId);
            
            if (stock == null)
            {
                var data = new Dictionary<string, object>
                {
                    { "Id", 0 },
                    { "InventoryId", inventoryId },
                    { "WareHouseId", warehouseId },
                    { "QuantityKg", deltaKg },
                    { "QuantityMeter", deltaMeter },
                    { "QuantityPiece", deltaPiece },
                    { "LastUpdatedAt", DateTime.Now }
                };
                Save(data);
            }
            else
            {
                string sql = "UPDATE Stock SET QuantityKg = QuantityKg + " + deltaKg.ToString().Replace(",", ".") + 
                            ", QuantityMeter = QuantityMeter + " + deltaMeter.ToString().Replace(",", ".") + 
                            ", QuantityPiece = QuantityPiece + " + deltaPiece + 
                            ", LastUpdatedAt = GETDATE() WHERE InventoryId = " + inventoryId + " AND WareHouseId = " + warehouseId;
                _orm.ExecuteRaw(sql);
            }
        }

        public List<Stock> GetByWarehouseId(int warehouseId)
        {
            var sql = $@"
SELECT 
    giris.Id AS Id,
    giris.InventoryId,
    i.InventoryCode,
    i.InventoryName,
    w.Code AS WarehouseCode,
    w.Name AS WarehouseName,
    giris.Piece AS GirisAdet,
    rec.ReceiptDate AS ReceiptDate,
    ISNULL(giris.Piece, 0) - ISNULL((
        SELECT SUM(cikis.Piece) 
        FROM ReceiptItem cikis
        INNER JOIN Receipt r2 ON cikis.ReceiptId = r2.Id 
        WHERE cikis.TrackingNumber = CAST(giris.Id AS NVARCHAR(50)) 
        AND r2.ReceiptType = 2
    ), 0) AS QuantityPiece,
    giris.NetWeight AS QuantityKg,
    rec.WareHouseId
FROM ReceiptItem giris
INNER JOIN Receipt rec ON giris.ReceiptId = rec.Id
INNER JOIN Inventory i ON giris.InventoryId = i.Id
LEFT JOIN Warehouse w ON w.Id = rec.WareHouseId
WHERE rec.ReceiptType = 1
AND rec.WareHouseId = {warehouseId}
AND giris.Piece > 0
AND (ISNULL(giris.Piece, 0) - ISNULL((
    SELECT SUM(cikis.Piece) 
    FROM ReceiptItem cikis
    INNER JOIN Receipt r2 ON cikis.ReceiptId = r2.Id 
    WHERE cikis.TrackingNumber = CAST(giris.Id AS NVARCHAR(50)) 
    AND r2.ReceiptType = 2
), 0)) > 0
ORDER BY giris.Id ASC";
            return _orm.QueryRaw<Stock>(sql).ToList();
        }
    }
}