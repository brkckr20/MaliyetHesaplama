using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class AllLogRepository
    {
        private readonly MiniOrm _orm;

        public AllLogRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(AllLog log)
        {
            var data = new Dictionary<string, object>
            {
                { "Id", log.Id },
                { "ReceiptId", log.ReceiptId },
                { "ReceiptType", log.ReceiptType },
                { "Operation", log.Operation },
                { "OperationDate", log.OperationDate },
                { "UserId", log.UserId },
                { "CompanyId", log.CompanyId },
                { "ComputerName", log.ComputerName ?? "" },
                { "ComputerIP", log.ComputerIP ?? "" },
                { "WareHouseId", log.WareHouseId },
                { "ReceiptNo", log.ReceiptNo ?? "" },
                { "InvoiceNo", log.InvoiceNo ?? "" }
            };
            return _orm.Save("AllLog", data);
        }

        public IEnumerable<AllLog> GetByReceiptId(int receiptId)
        {
            return _orm.QueryRaw<AllLog>($"SELECT * FROM AllLog WHERE ReceiptId = {receiptId} ORDER BY Id DESC");
        }

        public IEnumerable<AllLog> GetByReceiptIdAndType(int receiptId, int receiptType)
        {
            return _orm.QueryRaw<AllLog>($"SELECT * FROM AllLog WHERE ReceiptId = {receiptId} AND ReceiptType = {receiptType} ORDER BY Id DESC");
        }
    }
}