using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MaliyeHesaplama.models
{
    public class ReceiptItem : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _receiptId;
        public int ReceiptId
        {
            get => _receiptId;
            set => SetProperty(ref _receiptId, value);
        }

        private string _operationType = string.Empty;
        public string OperationType
        {
            get => _operationType;
            set => SetProperty(ref _operationType, value);
        }

        private int _inventoryId;
        public int InventoryId
        {
            get => _inventoryId;
            set => SetProperty(ref _inventoryId, value);
        }

        private string _inventoryCode = "";
        public string InventoryCode
        {
            get => _inventoryCode;
            set { _inventoryCode = value; OnPropertyChanged(nameof(InventoryCode)); }
        }

        private string _inventoryName = "";
        public string InventoryName
        {
            get => _inventoryName;
            set { _inventoryName = value; OnPropertyChanged(nameof(InventoryName)); }
        }

        private int _grM2;
        public int GrM2
        {
            get => _grM2;
            set => SetProperty(ref _grM2, value);
        }

        private decimal _grossWeight;
        public decimal GrossWeight
        {
            get => _grossWeight;
            set => SetProperty(ref _grossWeight, value);
        }

        private decimal _netWeight;
        public decimal NetWeight
        {
            get => _netWeight;
            set => SetProperty(ref _netWeight, value);
        }

        private decimal _netMeter;
        public decimal NetMeter
        {
            get => _netMeter;
            set => SetProperty(ref _netMeter, value);
        }

        private decimal _cashPayment;
        public decimal CashPayment
        {
            get => _cashPayment;
            set => SetProperty(ref _cashPayment, value);
        }

        private decimal _deferredPayment;
        public decimal DeferredPayment
        {
            get => _deferredPayment;
            set => SetProperty(ref _deferredPayment, value);
        }

        private string _forex = string.Empty;
        public string Forex
        {
            get => _forex;
            set => SetProperty(ref _forex, value);
        }

        private string _rowExplanation = string.Empty;
        public string RowExplanation
        {
            get => _rowExplanation;
            set => SetProperty(ref _rowExplanation, value);
        }

        private int _variantId;
        public int VariantId
        {
            get => _variantId;
            set => SetProperty(ref _variantId, value);
        }

        private string _variantCode = string.Empty;
        public string VariantCode
        {
            get => _variantCode;
            set
            {
                if (_variantCode != value)
                {
                    _variantCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _variant = string.Empty;
        public string Variant
        {
            get => _variant;
            set
            {
                if (_variant != value)
                {
                    _variant = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
