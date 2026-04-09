using MaliyeHesaplama.helpers.StokIslemleri.DTO;
using MaliyeHesaplama.helpers.StokIslemleri.Models;

namespace MaliyeHesaplama.helpers.StokIslemleri
{
    public class StokHelper
    {
        private readonly MiniOrm _orm;

        public StokHelper(MiniOrm orm)
        {
            _orm = orm;
        }

        public int StokGiris(StokGirisParametreleri p)
        {
            var mevcut = GetirStok(p.InventoryId, p.WareHouseId, p.VariantId, p.BatchNo, p.OrderNo);
            decimal yeniKg = (mevcut?.QuantityKg ?? 0) + p.MiktarKg;
            decimal yeniMeter = (mevcut?.QuantityMeter ?? 0) + p.MiktarMeter;
            int yeniAdet = (mevcut?.QuantityPiece ?? 0) + p.MiktarAdet;

            if (mevcut != null)
            {
                var sql = $@"UPDATE Stock SET 
                    QuantityKg = {yeniKg}, 
                    QuantityMeter = {yeniMeter}, 
                    QuantityPiece = {yeniAdet} 
                    WHERE Id = {mevcut.Id}";
                _orm.ExecuteRaw(sql);
                return mevcut.Id;
            }
            else
            {
                var sql = $@"INSERT INTO Stock (InventoryId, WareHouseId, VariantId, BatchNo, OrderNo, QuantityKg, QuantityMeter, QuantityPiece)
                    VALUES ({p.InventoryId}, {p.WareHouseId}, {(p.VariantId.HasValue ? p.VariantId.ToString() : "NULL")}, 
                    {(p.BatchNo != null ? $"'{p.BatchNo}'" : "NULL")}, {(p.OrderNo != null ? $"'{p.OrderNo}'" : "NULL")}, 
                    {p.MiktarKg}, {p.MiktarMeter}, {p.MiktarAdet})";
                _orm.ExecuteRaw(sql);
                return 0;
            }
        }

        public int StokCikis(StokCikisParametreleri p)
        {
            var mevcut = GetirStok(p.InventoryId, p.WareHouseId, p.VariantId, p.BatchNo, p.OrderNo);
            if (mevcut == null)
                throw new Exception("Stok bulunamadı!");

            decimal yeniKg = mevcut.QuantityKg - p.MiktarKg;
            decimal yeniMeter = mevcut.QuantityMeter - p.MiktarMeter;
            int yeniAdet = mevcut.QuantityPiece - p.MiktarAdet;

            var sql = $@"UPDATE Stock SET 
                QuantityKg = {yeniKg}, 
                QuantityMeter = {yeniMeter}, 
                QuantityPiece = {yeniAdet} 
                WHERE Id = {mevcut.Id}";
            _orm.ExecuteRaw(sql);
            return mevcut.Id;
        }

        public Stok? GetirStok(int inventoryId, int wareHouseId, int? variantId, string batchNo, string orderNo)
        {
            var sql = $@"SELECT * FROM Stock 
                        WHERE InventoryId = {inventoryId} AND WareHouseId = {wareHouseId} 
                        AND (VariantId = {(variantId.HasValue ? variantId.ToString() : "NULL")} OR (VariantId IS NULL AND {(variantId.HasValue ? variantId.ToString() : "NULL")} IS NULL))
                        AND (BatchNo = '{(batchNo ?? "")}' OR (BatchNo IS NULL AND '{(batchNo ?? "")}' = ''))
                        AND (OrderNo = '{(orderNo ?? "")}' OR (OrderNo IS NULL AND '{(orderNo ?? "")}' = ''))";

            return _orm.QueryRaw<Stok>(sql).FirstOrDefault();
        }

        public IEnumerable<Stok> GetirTumStoklar(int? wareHouseId = null, int? inventoryId = null)
        {
            string sql = "SELECT * FROM Stock WHERE 1=1";
            if (wareHouseId.HasValue) sql += $" AND WareHouseId = {wareHouseId}";
            if (inventoryId.HasValue) sql += $" AND InventoryId = {inventoryId}";
            return _orm.QueryRaw<Stok>(sql);
        }

        public IEnumerable<StokHareket> GetirHareketler(int? inventoryId = null, int? wareHouseId = null)
        {
            string sql = "SELECT * FROM StockMovement WHERE 1=1";
            if (inventoryId.HasValue) sql += $" AND InventoryId = {inventoryId}";
            if (wareHouseId.HasValue) sql += $" AND WareHouseId = {wareHouseId}";
            sql += " ORDER BY CreatedAt DESC";
            return _orm.QueryRaw<StokHareket>(sql);
        }

        public void HareketKaydet(StokHareket hareket)
        {
            var sql = $@"INSERT INTO StockMovement 
                (StockId, ReceiptId, ReceiptItemId, InventoryId, WareHouseId, VariantId, BatchNo, OrderNo,
                DeltaKg, DeltaMeter, DeltaPiece, BeforeKg, AfterKg, BeforeMeter, AfterMeter, BeforePiece, AfterPiece, UserId, CreatedAt)
                VALUES ({hareket.StockId}, {hareket.ReceiptId}, {hareket.ReceiptItemId}, {hareket.InventoryId}, {hareket.WareHouseId}, 
                {(hareket.VariantId.HasValue ? hareket.VariantId.ToString() : "NULL")}, 
                '{(hareket.BatchNo ?? "")}', '{(hareket.OrderNo ?? "")}',
                {hareket.DeltaKg}, {hareket.DeltaMeter}, {hareket.DeltaPiece}, {hareket.BeforeKg}, {hareket.AfterKg}, 
                {hareket.BeforeMeter}, {hareket.AfterMeter}, {hareket.BeforePiece}, {hareket.AfterPiece}, 
                {(hareket.UserId.HasValue ? hareket.UserId.ToString() : "NULL")}, GETDATE())";
            _orm.ExecuteRaw(sql);
        }
    }
}