using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MaliyeHesaplama.v2.Models
{
    public class ReceiptItemViewModel : INotifyPropertyChanged
    {
        private int _id;
        private int _inventoryId;
        private string _inventoryCode = "";
        private string _inventoryName = "";
        private string _operationType = "";
        private decimal _piece;
        private decimal _netMeter;
        private decimal _netWeight;
        private decimal _unitPrice;
        private string _priceUnit = "";
        private decimal _vat;
        private decimal _rowAmount;
        private string _rowExplanation = "";

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public int InventoryId
        {
            get => _inventoryId;
            set { _inventoryId = value; OnPropertyChanged(); }
        }
        public string InventoryCode
        {
            get => _inventoryCode;
            set { _inventoryCode = value; OnPropertyChanged(); }
        }
        public string InventoryName
        {
            get => _inventoryName;
            set { _inventoryName = value; OnPropertyChanged(); }
        }
        public string OperationType
        {
            get => _operationType;
            set { _operationType = value; OnPropertyChanged(); }
        }
        public decimal Piece
        {
            get => _piece;
            set { _piece = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public decimal NetMeter
        {
            get => _netMeter;
            set { _netMeter = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public decimal NetWeight
        {
            get => _netWeight;
            set { _netWeight = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public string PriceUnit
        {
            get => _priceUnit;
            set { _priceUnit = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public decimal Vat
        {
            get => _vat;
            set { _vat = value; OnPropertyChanged(); CalculateRowAmount(); }
        }
        public decimal RowAmount
        {
            get => _rowAmount;
            set { _rowAmount = value; OnPropertyChanged(); }
        }
        public string RowExplanation
        {
            get => _rowExplanation;
            set { _rowExplanation = value; OnPropertyChanged(); }
        }

        private void CalculateRowAmount()
        {
            if (string.IsNullOrEmpty(PriceUnit)) return;

            decimal quantity = 0;
            if (PriceUnit == "Kg") quantity = NetWeight;
            else if (PriceUnit == "Mt") quantity = NetMeter;
            else if (PriceUnit == "Adet") quantity = Piece;

            RowAmount = quantity * UnitPrice * (1 + Vat / 100);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}