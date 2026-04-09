namespace MaliyeHesaplama.helpers.StokIslemleri.DTO
{
    public class StokGirisParametreleri
    {
        public int InventoryId { get; set; }
        public int WareHouseId { get; set; }
        public int? VariantId { get; set; }
        public string? BatchNo { get; set; }
        public string? OrderNo { get; set; }
        public decimal MiktarKg { get; set; }
        public decimal MiktarMeter { get; set; }
        public int MiktarAdet { get; set; }
    }

    public class StokCikisParametreleri
    {
        public int InventoryId { get; set; }
        public int WareHouseId { get; set; }
        public int? VariantId { get; set; }
        public string? BatchNo { get; set; }
        public string? OrderNo { get; set; }
        public decimal MiktarKg { get; set; }
        public decimal MiktarMeter { get; set; }
        public int MiktarAdet { get; set; }
        public int ReceiptId { get; set; }
        public int ReceiptItemId { get; set; }
    }

    public class StokGunsonuModel
    {
        public string MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string DepoKodu { get; set; }
        public string DepoAdi { get; set; }
        public decimal OncekiKg { get; set; }
        public decimal GirisKg { get; set; }
        public decimal CikisKg { get; set; }
        public decimal SonKg { get; set; }
    }
}