using CommunityToolkit.Mvvm.Input;
using Dapper;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MaliyeHesaplama.mvvm
{
    public class MVM : IPageCommands
    {
        public ObservableCollection<ReceiptItem> ReceiptItems { get; } = new ObservableCollection<ReceiptItem>();
        public RelayyCommand SelectMaterialCommand { get; }
        public ICommand SaveCommand { get; }

        public models.Receipt Receipt { get; set; } = new();
        public MVM()
        {
            ReceiptItems = new ObservableCollection<ReceiptItem>();
            SelectMaterialCommand = new RelayyCommand(OpenMaterialSelectionExecute);
            SaveCommand = new RelayCommand(SaveToDatabase);
        }
        private void OpenMaterialSelectionExecute(object param)
        {
            if (param is not ReceiptItem item)
            {
                System.Windows.MessageBox.Show("Param ReceiptItem değil!");
                return;
            }
            var dialog = new wins.winMalzemeListesi(Convert.ToInt32(Enums.Inventory.Kumas));
            if (dialog.ShowDialog() == true)
            {
                item.InventoryId = dialog.Id;
                item.InventoryCode = dialog.Code;
                item.InventoryName = dialog.Name;
            }
        }
        
         public void SaveToDatabase()
        {
            using var conn = new SqlConnection("Server=.;Database=Hesap;Trusted_Connection=True;TrustServerCertificate=True;");
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var insertReceipt = @"INSERT INTO Receipt (ReceiptNo, ReceiptDate, CompanyId,ReceiptType,Authorized)
                                      OUTPUT INSERTED.Id
                                      VALUES (@ReceiptNo, @ReceiptDate, @CompanyId,@ReceiptType,@Authorized);";
                var receiptId = conn.ExecuteScalar<long>(insertReceipt, 
                    new
                    {
                        Receipt.ReceiptNo,
                        Receipt.ReceiptDate,
                        Receipt.CompanyId,
                        Receipt.ReceiptType,
                        Receipt.Authorized,
                    },transaction:tran
                );
                var insertItem = @"INSERT INTO ReceiptItem (ReceiptId, OperationType, InventoryId, GrossWeight)
                                   VALUES (@ReceiptId, @OperationType, @InventoryId, @GrossWeight);";
                foreach (var item in ReceiptItems)
                {
                    conn.Execute(insertItem, new
                    {
                        ReceiptId = receiptId,
                        OperationType = item.OperationType ?? "",
                        InventoryId = item.InventoryId,
                        GrossWeight = item.GrossWeight
                    }, transaction: tran);
                }
                tran.Commit();
                System.Windows.MessageBox.Show("Kayıt başarıyla tamamlandı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                System.Windows.MessageBox.Show($"Kayıt sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Yeni()
        {
            throw new NotImplementedException();
        }

        public void Kaydet()
        {
            SaveToDatabase();
        }

        public void Sil()
        {
            throw new NotImplementedException();
        }

        public void Yazdir()
        {
            throw new NotImplementedException();
        }

        public void Ileri()
        {
            throw new NotImplementedException();
        }

        public void Geri()
        {
            throw new NotImplementedException();
        }

        public void Listele()
        {
            throw new NotImplementedException();
        }
    }
}
