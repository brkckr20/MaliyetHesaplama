using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class AllLogItemsRepository
    {
        private readonly MiniOrm _orm;

        public AllLogItemsRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(AllLogItems item)
        {
            var data = new Dictionary<string, object>
            {
                { "Id", item.Id },
                { "LogId", item.LogId },
                { "DocumentId", item.DocumentId },
                { "DocumentLineId", item.DocumentLineId },
                { "OperationType", item.OperationType ?? "" },
                { "InventoryId", item.InventoryId },
                { "GrM2", item.GrM2 },
                { "GrossWeight", item.GrossWeight },
                { "NetWeight", item.NetWeight },
                { "GrossMeter", item.GrossMeter },
                { "NetMeter", item.NetMeter },
                { "Piece", item.Piece },
                { "Forex", item.Forex },
                { "ForexPrice", item.ForexPrice },
                { "UnitPrice", item.UnitPrice },
                { "VariantId", item.VariantId },
                { "ColorId", item.ColorId },
                { "RowExplanation", item.RowExplanation ?? "" },
                { "Receiver", item.Receiver ?? "" },
                { "MeasurementUnit", item.MeasurementUnit ?? "" },
                { "Brand", item.Brand ?? "" },
                { "BatchNo", item.BatchNo ?? "" },
                { "OrderNo", item.OrderNo ?? "" },
                { "CustomerOrderNo", item.CustomerOrderNo ?? "" }
            };
            return _orm.Save("AllLogItems", data);
        }

        public IEnumerable<AllLogItems> GetByLogId(int logId)
        {
            return _orm.QueryRaw<AllLogItems>($"SELECT * FROM AllLogItems WHERE LogId = {logId}");
        }

        public void DeleteByLogId(int logId)
        {
           // _orm.Execute($"DELETE FROM AllLogItems WHERE LogId = {logId}");
        }
    }
}