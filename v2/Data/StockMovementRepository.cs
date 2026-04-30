using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class StockMovementRepository
    {
        private readonly MiniOrm _orm;

        public StockMovementRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("StockMovement", data);
        }

        public IEnumerable<StockMovement> GetByMaterial(int materialId)
        {
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE InventoryId = {materialId}");
        }

        public IEnumerable<StockMovement> GetByWarehouse(int warehouseId)
        {
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE WarehouseId = {warehouseId}");
        }

        public IEnumerable<StockMovement> GetByDocument(int documentType, int documentId)
        {
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE DocumentType = {documentType} AND DocumentId = {documentId}");
        }

        public decimal GetStockQuantity(int materialId, int warehouseId)
        {
            string sql = $@"SELECT ISNULL(SUM(DeltaKg), 0) as TotalKg,
                          ISNULL(SUM(DeltaMeter), 0) as TotalMeter,
                          ISNULL(SUM(DeltaPiece), 0) as TotalPiece
                          FROM StockMovement WHERE InventoryId = {materialId} AND WarehouseId = {warehouseId}";
            var result = _orm.QueryRaw<dynamic>(sql);
            return result.Any() ? (decimal)result.First().TotalKg : 0;
        }

        public (decimal kg, decimal meter, int piece) GetStock(int materialId, int warehouseId)
        {
            string sql = $@"SELECT ISNULL(SUM(DeltaKg), 0) as TotalKg,
                          ISNULL(SUM(DeltaMeter), 0) as TotalMeter,
                          ISNULL(SUM(DeltaPiece), 0) as TotalPiece
                          FROM StockMovement WHERE InventoryId = {materialId} AND WarehouseId = {warehouseId}";
            var result = _orm.QueryRaw<dynamic>(sql);
            if (result.Any())
            {
                return ((decimal)result.First().TotalKg, (decimal)result.First().TotalMeter, (int)result.First().TotalPiece);
            }
            return (0, 0, 0);
        }

        public void DeleteByDocument(int documentType, int documentId)
        {
            _orm.ExecuteRaw($"DELETE FROM StockMovement WHERE DocumentType = {documentType} AND DocumentId = {documentId}");
        }
    }
}