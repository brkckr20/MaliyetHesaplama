using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MaliyeHesaplama.helpers.StokIslemleri.Models
{
    public class StokHareket : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private int _stockId;
        public int StockId
        {
            get => _stockId;
            set => SetField(ref _stockId, value);
        }

        private int _receiptId;
        public int ReceiptId
        {
            get => _receiptId;
            set => SetField(ref _receiptId, value);
        }

        private int _receiptItemId;
        public int ReceiptItemId
        {
            get => _receiptItemId;
            set => SetField(ref _receiptItemId, value);
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

        private decimal _deltaKg;
        public decimal DeltaKg
        {
            get => _deltaKg;
            set => SetField(ref _deltaKg, value);
        }

        private decimal _deltaMeter;
        public decimal DeltaMeter
        {
            get => _deltaMeter;
            set => SetField(ref _deltaMeter, value);
        }

        private int _deltaPiece;
        public int DeltaPiece
        {
            get => _deltaPiece;
            set => SetField(ref _deltaPiece, value);
        }

        private decimal _beforeKg;
        public decimal BeforeKg
        {
            get => _beforeKg;
            set => SetField(ref _beforeKg, value);
        }

        private decimal _afterKg;
        public decimal AfterKg
        {
            get => _afterKg;
            set => SetField(ref _afterKg, value);
        }

        private decimal _beforeMeter;
        public decimal BeforeMeter
        {
            get => _beforeMeter;
            set => SetField(ref _beforeMeter, value);
        }

        private decimal _afterMeter;
        public decimal AfterMeter
        {
            get => _afterMeter;
            set => SetField(ref _afterMeter, value);
        }

        private int _beforePiece;
        public int BeforePiece
        {
            get => _beforePiece;
            set => SetField(ref _beforePiece, value);
        }

        private int _afterPiece;
        public int AfterPiece
        {
            get => _afterPiece;
            set => SetField(ref _afterPiece, value);
        }

        private int? _userId;
        public int? UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetField(ref _createdAt, value);
        }

        private string _receiptNo;
        public string ReceiptNo
        {
            get => _receiptNo ?? string.Empty;
            set => SetField(ref _receiptNo, value);
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