using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class ReceiptLogRepository
    {
        private readonly MiniOrm _orm;

        public ReceiptLogRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("ReceiptLog", data);
        }

        public bool ExistsByReceiptItemId(int receiptItemId)
        {
            var result = _orm.QueryRaw<ReceiptLog>($"SELECT TOP 1 Id FROM ReceiptLog WHERE ReceiptItemId = {receiptItemId}");
            return result.Any();
        }

        public IEnumerable<ReceiptLog> GetByReceiptItemId(int receiptItemId)
        {
            return _orm.QueryRaw<ReceiptLog>($"SELECT * FROM ReceiptLog WHERE ReceiptItemId = {receiptItemId}");
        }

        public IEnumerable<ReceiptLog> GetByReceiptId(int receiptId)
        {
            return _orm.QueryRaw<ReceiptLog>($"SELECT * FROM ReceiptLog WHERE ReceiptId = {receiptId}");
        }
    }
}