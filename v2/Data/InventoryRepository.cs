using System.Collections.Generic;
using System.Linq;
using MaliyeHesaplama.v2.Models;

namespace MaliyeHesaplama.v2.Data
{
    public class InventoryRepository
    {
        private readonly MiniOrm _orm;

        public InventoryRepository()
        {
            _orm = new MiniOrm();
        }

        public int Save(Dictionary<string, object> data)
        {
            return _orm.Save("Inventory", data);
        }

        public IEnumerable<Inventory> GetAll()
        {
            return _orm.GetAll<Inventory>("Inventory");
        }

        public IEnumerable<Inventory> GetAll(string whereClause)
        {
            return _orm.QueryRaw<Inventory>($"SELECT * FROM Inventory WHERE {whereClause}");
        }

        public IEnumerable<Inventory> GetAllWithDetails()
        {
            var sql = @"
                SELECT 
                    i.Id, i.Code, i.Name, i.Type, i.CategoryId, i.UnitId, i.Barcode,
                    i.VatRate, i.MinStock, i.MaxStock, i.IsActive,
                    CASE i.Type 
                        WHEN 1 THEN 'Ham Madde'
                        WHEN 2 THEN 'Yarı Mamul'
                        WHEN 3 THEN 'Mamul'
                        WHEN 4 THEN 'Sarf Malzeme'
                    END as TypeName,
                    COALESCE(c.Name, '') as CategoryName,
                    COALESCE(u.Name, '') as UnitName
                FROM Inventory i
                LEFT JOIN Category c ON i.CategoryId = c.Id
                LEFT JOIN Unit u ON i.UnitId = u.Id";

            return _orm.QueryRaw<Inventory>(sql);
        }

        public Inventory GetById(int id)
        {
            return _orm.GetById<Inventory>("Inventory", id, "Id");
        }

        public void Delete(int id)
        {
            _orm.ExecuteRaw($"DELETE FROM Inventory WHERE Id = {id}");
        }

        public IEnumerable<Receipt> GetMovementList(int receiptType)
        {
            var sql = $@"
                SELECT 
                    ISNULL(R.Id,0) [Id],
                    ISNULL(R.ReceiptNo,'') [ReceiptNo],
                    ISNULL(R.ReceiptType,'') [ReceiptType],
                    ISNULL(R.ReceiptDate,'') [ReceiptDate],
                    ISNULL(C.Id,'') [CompanyId],
                    ISNULL(C.CompanyName,'') [CompanyName],
                    ISNULL(C.CompanyCode,'') [CompanyCode],
                    ISNULL(R.Explanation,'') [Explanation],
                    ISNULL(R.Approved,0) [Approved],
                    ISNULL(RI.Id,'') [ReceiptItemId],
                    ISNULL(RI.OperationType,'') [OperationType],
                    ISNULL(I.Id,'') [InventoryId],
                    ISNULL(I.Code,'') [InventoryCode],
                    ISNULL(I.Name,'') [InventoryName],
                    ISNULL(RI.NetMeter,0) [NetMeter],
                    ISNULL(RI.NetWeight,0) [NetWeight],
                    ISNULL(RI.Piece,0) [Piece],
                    ISNULL(RI.UnitPrice,0) [UnitPrice],
                    ISNULL(RI.Vat,0) [Vat],
                    ISNULL(RI.RowAmount,0) [RowAmount],
                    ISNULL(RI.RowExplanation,'') [RowExplanation],
                    ISNULL(W.Id,'') [WareHouseId],
                    ISNULL(W.Code,'') [WareHouseCode],
                    ISNULL(W.Name,'') [WareHouseName],
                    ISNULL(RI.TrackingNumber,'') [TrackingNumber],
                    ISNULL(RI.OrderNo,'') [OrderNo],
                    ISNULL(RI.CustomerOrderNo,'') [CustomerOrderNo],
                    ISNULL(RI.Receiver,0) [Receiver]
                FROM Receipt R
                LEFT JOIN Company C WITH(nolock) ON C.Id = R.CompanyId
                LEFT JOIN ReceiptItem RI WITH(nolock) ON RI.ReceiptId = R.Id
                LEFT JOIN Inventory I WITH(nolock) ON I.Id = RI.InventoryId
                LEFT JOIN Warehouse W ON R.WareHouseId = W.Id
                WHERE R.ReceiptType = {receiptType}";

            return _orm.QueryRaw<Receipt>(sql);
        }
    }
}