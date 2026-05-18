using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;

namespace MaliyeHesaplama.v2.UserControls
{
    public partial class UC_DepoKartiV2 : System.Windows.Controls.UserControl, IPageCommands
    {
        private readonly WarehouseRepository _repo;
        private int _currentId = 0;

        public UC_DepoKartiV2()
        {
            InitializeComponent();
            _repo = new WarehouseRepository();
            ButtonBar.PageCommands = this;
            Yeni();
        }

        private void LoadRecord(Warehouse record)
        {
            _currentId = record.Id;
            txtKodu.Text = record.Code ?? "";
            txtAdi.Text = record.Name ?? "";
            chkKullanimda.IsChecked = record.IsUse;
            txtAdres1.Text = record.Address1 ?? "";
            txtAdres2.Text = record.Address2 ?? "";
            txtIlce.Text = record.District ?? "";
            txtIl.Text = record.City ?? "";
            txtPostaKodu.Text = record.PostalCode ?? "";
            txtTelefon.Text = record.Phone ?? "";
            txtEmail.Text = record.Email ?? "";
        }

        public void Yeni()
        {
            _currentId = 0;
            txtKodu.Text = "";
            txtAdi.Text = "";
            chkKullanimda.IsChecked = true;
            txtAdres1.Text = "";
            txtAdres2.Text = "";
            txtIlce.Text = "";
            txtIl.Text = "";
            txtPostaKodu.Text = "";
            txtTelefon.Text = "";
            txtEmail.Text = "";
            txtKodu.Focus();
        }

        public void Kaydet()
        {
            if (string.IsNullOrWhiteSpace(txtKodu.Text))
            {
                System.Windows.MessageBox.Show("Depo kodu girilmelidir!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var data = new System.Collections.Generic.Dictionary<string, object>
            {
                { "Id", _currentId },
                { "Code", txtKodu.Text.Trim() },
                { "Name", txtAdi.Text?.Trim() ?? "" },
                { "IsUse", chkKullanimda.IsChecked == true },
                { "Address1", txtAdres1.Text?.Trim() ?? "" },
                { "Address2", txtAdres2.Text?.Trim() ?? "" },
                { "District", txtIlce.Text?.Trim() ?? "" },
                { "City", txtIl.Text?.Trim() ?? "" },
                { "PostalCode", txtPostaKodu.Text?.Trim() ?? "" },
                { "Phone", txtTelefon.Text?.Trim() ?? "" },
                { "Email", txtEmail.Text?.Trim() ?? "" }
            };

            _currentId = _repo.Save(data);
            System.Windows.MessageBox.Show("Kaydedildi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Sil()
        {
            if (_currentId == 0) return;

            var result = System.Windows.MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(_currentId);
                Yeni();
            }
        }

        public void Yazdir()
        {
            if (_currentId == 0)
            {
                System.Windows.MessageBox.Show("Rapor alabilmek için lütfen kayıt seçiniz", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            new wins.winRaporSecimi("Depo Kartı", _currentId).ShowDialog();
        }

        public void Ileri()
        {
            var list = _repo.GetAll().ToList();
            if (list.Count == 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex < 0 || currentIndex >= list.Count - 1)
            {
                System.Windows.MessageBox.Show("Gösterilecek başka bir kayıt yok!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            LoadRecord(list[currentIndex + 1]);
        }

        public void Geri()
        {
            var list = _repo.GetAll().ToList();
            if (list.Count == 0)
            {
                System.Windows.MessageBox.Show("Kayıt bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var currentIndex = list.FindIndex(x => x.Id == _currentId);
            if (currentIndex <= 0)
            {
                System.Windows.MessageBox.Show("Gösterilecek başka bir kayıt yok!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            LoadRecord(list[currentIndex - 1]);
        }

        public void Listele()
        {
            var pencere = new winDepoListesiV2();
            pencere.Owner = Window.GetWindow(this);
            if (pencere.ShowDialog() == true)
            {
                var record = _repo.GetById(pencere.SecilenId);
                if (record != null)
                    LoadRecord(record);
            }
        }
    }
}