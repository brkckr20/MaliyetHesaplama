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

        public List<StockSummary> GetByWarehouseId(int warehouseId)
        {
            var sql = $@"
;WITH StokData AS (
    SELECT 
        giris.Id,
        giris.InventoryId,
        i.InventoryCode,
        i.InventoryName,
        i.VatRate,
        giris.UnitPrice,
        giris.Piece AS GirisAdet,
        rec.WareHouseId,
        rec.ReceiptDate,
        ISNULL(giris.Piece, 0) - ISNULL((
            SELECT SUM(cikis.Piece)
            FROM ReceiptItem cikis
            INNER JOIN Receipt r2 ON cikis.ReceiptId = r2.Id
            WHERE cikis.TrackingNumber = CAST(giris.Id AS NVARCHAR(50))
            AND r2.ReceiptType = 2
        ), 0) AS Kalan
    FROM ReceiptItem giris
    INNER JOIN Receipt rec ON giris.ReceiptId = rec.Id
    INNER JOIN Inventory i ON giris.InventoryId = i.Id
    WHERE rec.ReceiptType = 1
        AND giris.Piece > 0
)
SELECT 
    sd.InventoryId,
    sd.InventoryCode,
    sd.InventoryName,
    SUM(sd.GirisAdet) AS GirisAdet,
    SUM(sd.Kalan) AS QuantityPiece,
    MAX(sd.VatRate) AS Vat,
    (
        SELECT TOP 1 giris.UnitPrice
        FROM ReceiptItem giris
        INNER JOIN Receipt rec ON giris.ReceiptId = rec.Id
        WHERE giris.InventoryId = sd.InventoryId
        AND rec.WareHouseId = sd.WareHouseId
        AND rec.ReceiptType = 1
        ORDER BY giris.Id ASC
    ) AS UnitPrice,
    (
        SELECT STUFF((
            SELECT ', ' + CAST(Id AS NVARCHAR(20))
            FROM StokData sd2
            WHERE sd2.InventoryId = sd.InventoryId AND sd2.WareHouseId = sd.WareHouseId
            ORDER BY Id
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
    ) AS Ids,
    (
        SELECT STUFF((
            SELECT ', ' + CONVERT(NVARCHAR(10), ReceiptDate, 104)
            FROM StokData sd2
            WHERE sd2.InventoryId = sd.InventoryId AND sd2.WareHouseId = sd.WareHouseId
            ORDER BY Id
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
    ) AS Tarihler
FROM StokData sd
WHERE sd.WareHouseId = {warehouseId}
GROUP BY sd.InventoryId, sd.InventoryCode, sd.InventoryName, sd.WareHouseId
HAVING SUM(sd.Kalan) > 0
ORDER BY sd.InventoryCode ASC";
            return _orm.QueryRaw<StockSummary>(sql).ToList();
        }
    }

    public class StockSummary
    {
        public int InventoryId { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public int GirisAdet { get; set; }
        public decimal QuantityPiece { get; set; }
        public decimal Vat { get; set; }
        public decimal UnitPrice { get; set; }
        public string Ids { get; set; }
        public string Tarihler { get; set; }
    }
}