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
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE MaterialId = {materialId}");
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
            var sql = $"SELECT ISNULL(SUM(Quantity), 0) as Total FROM StockMovement WHERE MaterialId = {materialId} AND WarehouseId = {warehouseId}";
            var result = _orm.QueryRaw<dynamic>(sql);
            if (result.Any())
                return (decimal)result.First().Total;
            return 0;
        }

        public void DeleteByDocument(int documentType, int documentId)
        {
            _orm.ExecuteRaw($"DELETE FROM StockMovement WHERE DocumentType = {documentType} AND DocumentId = {documentId}");
        }
    }
}