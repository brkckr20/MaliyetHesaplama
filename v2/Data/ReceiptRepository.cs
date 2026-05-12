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

        public void DeleteItems(int receiptId)
        {
            _orm.ExecuteRaw($"DELETE FROM ReceiptItem WHERE ReceiptId = {receiptId}");
        }

        public void DeleteItem(int itemId)
        {
            _orm.ExecuteRaw($"DELETE FROM ReceiptItem WHERE Id = {itemId}");
        }

        public int SaveItem(Dictionary<string, object> data)
        {
            return _orm.Save("ReceiptItem", data);
        }

        public IEnumerable<ReceiptItem> GetItemsByReceiptId(int receiptId)
        {
            return _orm.QueryRaw<ReceiptItem>($@"SELECT RI.*, I.InventoryCode, I.InventoryName 
                    FROM ReceiptItem RI
                    LEFT JOIN Inventory I ON RI.InventoryId = I.Id
                    WHERE RI.ReceiptId = {receiptId}");
        }

        public IEnumerable<ReceiptItem> GetFasonGidenler(int companyId)
        {
            string sql = @"
                SELECT 
                    RI.Id, RI.InventoryId, I.InventoryCode, I.InventoryName, 
                    RI.OperationType, RI.NetWeight, RI.NetMeter, RI.Piece, RI.UnitPrice, RI.Vat, RI.TrackingNumber, RI.IsWithChip,
                    C.CompanyCode, C.CompanyName,
                    RI.Piece - ISNULL((SELECT SUM(RI2.Piece) FROM ReceiptItem RI2 
                        INNER JOIN Receipt R2 ON RI2.ReceiptId = R2.Id 
                        WHERE RI2.TrackingNumber = CAST(RI.Id AS NVARCHAR(50)) AND R2.ReceiptType = 1), 0) AS KalanAdet
                FROM ReceiptItem RI
                INNER JOIN Receipt R ON RI.ReceiptId = R.Id
                INNER JOIN Inventory I ON RI.InventoryId = I.Id
                INNER JOIN Company C ON R.CompanyId = C.Id
                WHERE R.ReceiptType = 2 AND R.CompanyId = " + companyId + @"
                AND RI.Piece > ISNULL((SELECT SUM(RI2.Piece) FROM ReceiptItem RI2 
                    INNER JOIN Receipt R2 ON RI2.ReceiptId = R2.Id 
                    WHERE RI2.TrackingNumber = CAST(RI.Id AS NVARCHAR(50)) AND R2.ReceiptType = 1), 0)";
            return _orm.QueryRaw<ReceiptItem>(sql);
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

        public Receipt GetPrevious(int currentId, int receiptType)
        {
            var sql = $"SELECT TOP 1 * FROM Receipt WHERE ReceiptType = {receiptType} AND Id < {currentId} ORDER BY Id DESC";
            var result = _orm.QueryRaw<Receipt>(sql);
            return result.FirstOrDefault();
        }

        public Receipt GetNext(int currentId, int receiptType)
        {
            var sql = $"SELECT TOP 1 * FROM Receipt WHERE ReceiptType = {receiptType} AND Id > {currentId} ORDER BY Id ASC";
            var result = _orm.QueryRaw<Receipt>(sql);
            return result.FirstOrDefault();
        }
    }
}