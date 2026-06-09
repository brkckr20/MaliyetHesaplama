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
    }

    public class StokGunsonuModel
    {
        public int InventoryId { get; set; }
        public string? InventoryCode { get; set; }
        public string? InventoryName { get; set; }
        public decimal ToplamKg { get; set; }
        public decimal ToplamMeter { get; set; }
        public int ToplamAdet { get; set; }
    }
}
