using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MaliyeHesaplama.models
{
    public class InventoryReceipt : INotifyPropertyChanged
    {
        private int _id, _inventoryId, _ownerInventoryId, _type;
        private string _genus, _forex, _operationType, _inventoryCode, _inventoryName;
        private decimal _quantity, _quantityPrice, _forexPrice;

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
        public string Genus
        {
            get => _genus;
            set { _genus = value; OnPropertyChanged(); }
        }
        public string Forex
        {
            get => _forex;
            set { _forex = value; OnPropertyChanged(); }
        }
        public decimal Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }
        public decimal UnitPrice
        {
            get => _quantityPrice;
            set { _quantityPrice = value; OnPropertyChanged(); }
        }
        public decimal ForexPrice
        {
            get => _forexPrice;
            set { _forexPrice = value; OnPropertyChanged(); }
        }
        public int OwnerInventoryId
        {
            get => _ownerInventoryId;
            set { _ownerInventoryId = value; OnPropertyChanged(); }
        }

        public string OperationType
        {
            get => _operationType;
            set { _operationType = value; OnPropertyChanged(); }
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
        public int Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
