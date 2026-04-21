using System.Collections.Generic;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class ReceiptRepository
    {
        private readonly MiniOrm _orm;

        public ReceiptRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("Receipt", data);
        }

        public Receipt GetById(int id)
        {
            return _orm.GetById<Receipt>("Receipt", id, "Id");
        }

        public IEnumerable<Receipt> GetByType(int receiptType)
        {
            return _orm.QueryRaw<Receipt>($"SELECT * FROM Receipt WHERE ReceiptType = {receiptType} ORDER BY Id DESC");
        }

        public IEnumerable<Receipt> GetByTypeList(int receiptType, int count = 100)
        {
            return _orm.QueryRaw<Receipt>($"SELECT TOP {count} * FROM Receipt WHERE ReceiptType = {receiptType} ORDER BY Id DESC");
        }

        public IEnumerable<Receipt> GetAll()
        {
            return _orm.GetAll<Receipt>("Receipt");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM Receipt WHERE Id = {id}");
        }

        public IEnumerable<ReceiptItem> GetItemsByReceiptId(int receiptId)
        {
            return _orm.QueryRaw<ReceiptItem>($"SELECT * FROM ReceiptItem WHERE ReceiptId = {receiptId}");
        }

        public string GetRecordNo(string tableName, string columnName, string typeColumn, int receiptType)
        {
            var sql = $"SELECT TOP 1 {columnName} FROM {tableName} WHERE {typeColumn} = {receiptType} ORDER BY Id DESC";
            var result = _orm.QueryRaw<dynamic>(sql);
            if (result.Any())
            {
                dynamic last = result.First();
                string lastNo = last.ReceiptNo;
                if (int.TryParse(lastNo, out int lastNum))
                    return (lastNum + 1).ToString();
            }
            return "1";
        }
    }
}