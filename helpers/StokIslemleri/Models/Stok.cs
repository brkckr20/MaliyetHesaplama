using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MaliyeHesaplama.helpers.StokIslemleri.Models
{
    public class Stok : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private int _inventoryId;
        public int InventoryId
        {
            get => _inventoryId;
            set => SetField(ref _inventoryId, value);
        }

        private int _wareHouseId;
        public int WareHouseId
        {
            get => _wareHouseId;
            set => SetField(ref _wareHouseId, value);
        }

        private int? _variantId;
        public int? VariantId
        {
            get => _variantId;
            set => SetField(ref _variantId, value);
        }

        private string _batchNo;
        public string BatchNo
        {
            get => _batchNo ?? string.Empty;
            set => SetField(ref _batchNo, value);
        }

        private string _orderNo;
        public string OrderNo
        {
            get => _orderNo ?? string.Empty;
            set => SetField(ref _orderNo, value);
        }

        private decimal _quantityKg;
        public decimal QuantityKg
        {
            get => _quantityKg;
            set => SetField(ref _quantityKg, value);
        }

        private decimal _quantityMeter;
        public decimal QuantityMeter
        {
            get => _quantityMeter;
            set => SetField(ref _quantityMeter, value);
        }

        private int _quantityPiece;
        public int QuantityPiece
        {
            get => _quantityPiece;
            set => SetField(ref _quantityPiece, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}