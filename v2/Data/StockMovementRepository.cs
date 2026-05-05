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

        public IEnumerable<StockMovement> GetByReceiptId(int receiptId)
        {
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE ReceiptId = {receiptId}");
        }

        public bool ExistsByReceiptItemId(int receiptItemId)
        {
            var result = _orm.QueryRaw<StockMovement>($"SELECT TOP 1 Id FROM StockMovement WHERE ReceiptItemId = {receiptItemId}");
            return result.Any();
        }

        public IEnumerable<StockMovement> GetByReceiptItemId(int receiptItemId)
        {
            return _orm.QueryRaw<StockMovement>($"SELECT * FROM StockMovement WHERE ReceiptItemId = {receiptItemId}");
        }
    }
}