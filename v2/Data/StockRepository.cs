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
            var sql = @"SELECT s.Id, i.Code AS InventoryCode, i.Name AS InventoryName, w.Code AS WarehouseCode, w.Name AS WarehouseName, 
                        s.QuantityKg, s.QuantityMeter, s.QuantityPiece
                        FROM Stock s 
                        LEFT JOIN Inventory i ON i.Id = s.InventoryId
                        LEFT JOIN Warehouse w ON w.Id = s.WareHouseId
                        WHERE s.WareHouseId = " + warehouseId + " AND (s.QuantityKg > 0 OR s.QuantityMeter > 0 OR s.QuantityPiece > 0)";
            return _orm.QueryRaw<Stock>(sql).ToList();
        }
    }
}