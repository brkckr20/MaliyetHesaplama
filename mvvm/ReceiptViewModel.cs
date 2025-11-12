using CommunityToolkit.Mvvm.Input;
using Dapper;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using static MaliyeHesaplama.helpers.Enums;

namespace MaliyeHesaplama.mvvm
{
    public class ReceiptViewModel : INotifyPropertyChanged
    {
        public models.Receipt Receipt { get; set; } = new();
        public ObservableCollection<Dictionary<string, object>> ReceiptItems { get; set; } = new();

        public ICommand SaveCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand SelectMaterialCommand { get; }

        public ReceiptViewModel()
        {
            Receipt.ReceiptDate = DateTime.Now;
            SaveCommand = new RelayCommand(SaveToDatabase);
            AddItemCommand = new RelayCommand(AddNewItem);
            SelectMaterialCommand = new RelayCommand<object>(OpenMaterialSelectionExecute);
        }

        private void AddNewItem()
        {
            var item = new Dictionary<string, object>
            {
                ["OperationType"] = "",
                ["InventoryId"] = "",
                ["InventoryCode"] = "",
                ["InventoryName"] = "",
                ["GrossWeight"] = 0m
            };
            ReceiptItems.Add(item);
        }

        private void OpenMaterialSelectionExecute(object param)
        {
            if (param is not ReceiptItem item)
                return;

            var dialog = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            if (dialog.ShowDialog() == true)
            {
                item.InventoryId = dialog.Id;
                //item.InventoryCode = dialog.Code;
                //item.InventoryName = dialog.Name;
            }
        }
        public void SaveToDatabase()
        {
            using var conn = new SqlConnection("Server=.;Database=Hesap;Trusted_Connection=True;TrustServerCertificate=True;");
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                var insertReceipt = @"INSERT INTO Receipt (ReceiptNo, ReceiptDate, CompanyId)
                                      OUTPUT INSERTED.Id
                                      VALUES (@ReceiptNo, @ReceiptDate, @CompanyId);";

                var receiptId = conn.ExecuteScalar<long>(insertReceipt, new
                {
                    Receipt.ReceiptNo,
                    Receipt.ReceiptDate,
                    Receipt.CompanyId
                }, transaction: tran);

                var insertItem = @"INSERT INTO ReceiptItem (ReceiptId, OperationType, InventoryId, GrossWeight)
                                   VALUES (@ReceiptId, @OperationType, @InventoryId, @GrossWeight);";

                foreach (var item in ReceiptItems)
                {
                    conn.Execute(insertItem, new
                    {
                        ReceiptId = receiptId,
                        OperationType = item["OperationType"] ?? "",
                        InventoryId = item["InventoryId"] ?? 0,
                        GrossWeight = item["GrossWeight"] ?? 0m
                    }, transaction: tran);
                }

                tran.Commit();
                MessageBox.Show("Kayıt başarıyla tamamlandı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show($"Kayıt sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
