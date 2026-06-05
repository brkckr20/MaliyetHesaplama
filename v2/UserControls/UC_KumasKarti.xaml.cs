using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaliyeHesaplama.helpers;
using MaliyeHesaplama.Interfaces;
using MaliyeHesaplama.v2.Data;
using MaliyeHesaplama.v2.Models;
using MaliyeHesaplama.v2.Windows;

namespace MaliyeHesaplama.v2.UserControls
{
    public partial class UC_KumasKarti : System.Windows.Controls.UserControl, IPageCommands
    {
        private readonly InventoryRepository _repo;
        private readonly MiniOrm _orm;
        int Id = 0, PrefixId, DokumaCinsiId, DesenId;

        public UC_KumasKarti()
        {
            InitializeComponent();
            _repo = new InventoryRepository();
            _orm = new MiniOrm();
            ButtonBar.PageCommands = this;
            this.DataContext = this;
            Temizle();
        }

        void LoadRecord(Inventory record)
        {
            Id = record.Id;
            txtKodu.Text = record.InventoryCode ?? "";
            lblKumasAdi.Text = record.InventoryName ?? "";
            txtAciklama.Text = record.Explanation ?? "";
            chckKullanimda.IsChecked = record.IsUse;

            if (!string.IsNullOrEmpty(record.CombinedCode))
            {
                var parts = record.CombinedCode.Split('-');
                if (parts.Length == 4)
                {
                    if (int.TryParse(parts[0], out int prefix))
                        PrefixId = prefix;
                    if (int.TryParse(parts[1], out int cinsi))
                    {
                        DokumaCinsiId = cinsi;
                        var feature = _orm.GetById<dynamic>("FeatureCoding", cinsi);
                        if (feature != null)
                        {
                            txtDokumaCinsi.Text = feature.Explanation ?? "";
                            lblCinsi.Text = feature.Explanation ?? "";
                        }
                    }
                    if (int.TryParse(parts[2], out int desen))
                    {
                        DesenId = desen;
                        var feature = _orm.GetById<dynamic>("FeatureCoding", desen);
                        if (feature != null)
                            txtKumasDesen.Text = feature.Explanation ?? "";
                    }
                    chckIpBoyali.IsChecked = parts[3] == "1";
                }
            }
        }

        void Temizle()
        {
            Id = 0;
            PrefixId = 0;
            DokumaCinsiId = 0;
            DesenId = 0;
            txtKodu.Text = "";
            lblKumasAdi.Text = "";
            txtDokumaCinsi.Text = "";
            lblCinsi.Text = "";
            txtKumasDesen.Text = "";
            chckIpBoyali.IsChecked = false;
            txtAciklama.Text = "";
            chckKullanimda.IsChecked = true;
        }

        public void Yeni()
        {
            Temizle();
        }

        public void Kaydet()
        {
            if (string.IsNullOrWhiteSpace(txtKodu.Text))
            {
                Bildirim.Uyari2("Lütfen bir kumaş kodu seçiniz!");
                return;
            }
            if (DokumaCinsiId == 0)
            {
                Bildirim.Uyari2("Lütfen kumaş cinsi seçiniz!");
                return;
            }

            try
            {
                string combinedCode = $"{PrefixId:D3}-{DokumaCinsiId:D3}-{DesenId:D3}-{(chckIpBoyali.IsChecked == true ? "1" : "0")}";
                string inventoryName = $"{txtDokumaCinsi.Text} {lblKumasAdi.Text} {txtKumasDesen.Text} {(chckIpBoyali.IsChecked == true ? "İpliği Boyalı" : "")}";

                var existingCode = _repo.GetInventoryCodeByCombinedCode(combinedCode, Id > 0 ? Id : null);
                if (!string.IsNullOrEmpty(existingCode))
                {
                    Bildirim.Uyari2($"Belirtmiş olduğunuz özelliklere göre daha önceden bir kumaş kartı kayıt edilmiş.\nLütfen: {existingCode} nolu kaydı kontrol ediniz!");
                    return;
                }

                var dict = new Dictionary<string, object>
                {
                    { "Id", Id },
                    { "InventoryCode", txtKodu.Text },
                    { "InventoryName", inventoryName },
                    { "CombinedCode", combinedCode },
                    { "IsPrefix", false },
                    { "Explanation", txtAciklama.Text },
                    { "IsUse", chckKullanimda.IsChecked },
                    { "Unit", string.Empty },
                    { "IsStock", true },
                    { "Type", Convert.ToInt32(Enums.Inventory.Kumas) },
                };

                Id = _repo.Save(dict);
                _orm.Save("Numerator", new Dictionary<string, object> { { "Id", PrefixId }, { "Number", Convert.ToInt32(txtKodu.Text.Substring(3, 3)) } });
                Bildirim.Bilgilendirme2("Kumaş kayıt işlemi başarılı bir şekilde gerçekleştirildi.");
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2($"Kayıt sırasında bir hata oluştu: {ex.Message}");
            }
        }

        public void Sil()
        {
            if (Id == 0)
            {
                Bildirim.Uyari2("Silmek için bir kayıt seçiniz!");
                return;
            }

            if (Bildirim.SilmeOnayi2())
            {
                try
                {
                    _repo.Delete(Id);
                    Bildirim.SilmeBasarili2();
                    Temizle();
                }
                catch (Exception ex)
                {
                    Bildirim.Uyari2($"Silme sırasında bir hata oluştu: {ex.Message}");
                }
            }
        }

        public void Yazdir()
        {
            if (Id == 0)
            {
                Bildirim.Uyari2("Form görüntüleyebilmek için bir kayıt seçiniz!");
            }
            else
            {
                wins.winRaporSecimi win = new wins.winRaporSecimi("Kumaş Kartı", Id);
                win.ShowDialog();
            }
        }

        public void Ileri()
        {
            try
            {
                var list = _repo.GetAll("Type = " + Convert.ToInt32(Enums.Inventory.Kumas)).ToList();
                if (list.Count == 0)
                {
                    Bildirim.Bilgilendirme2("Gösterilecek başka bir kayıt bulunamadı!");
                    return;
                }
                var currentIndex = list.FindIndex(x => x.Id == Id);
                if (currentIndex < 0)
                {
                    LoadRecord(list.First());
                    return;
                }
                if (currentIndex < list.Count - 1)
                    LoadRecord(list[currentIndex + 1]);
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2($"Hata: {ex.Message}");
            }
        }

        public void Geri()
        {
            try
            {
                var list = _repo.GetAll("Type = " + Convert.ToInt32(Enums.Inventory.Kumas)).ToList();
                if (list.Count == 0)
                {
                    Bildirim.Bilgilendirme2("Gösterilecek başka bir kayıt bulunamadı!");
                    return;
                }
                var currentIndex = list.FindIndex(x => x.Id == Id);
                if (currentIndex < 0)
                {
                    LoadRecord(list.Last());
                    return;
                }
                if (currentIndex > 0)
                    LoadRecord(list[currentIndex - 1]);
            }
            catch (Exception ex)
            {
                Bildirim.Uyari2($"Hata: {ex.Message}");
            }
        }

        public void Listele()
        {
            var pencere = new winMalzemeListesiV2(Convert.ToInt32(Enums.Inventory.Kumas));
            pencere.Owner = Window.GetWindow(this);
            if (pencere.ShowDialog() == true)
            {
                var record = _repo.GetById(pencere.SecilenId);
                if (record != null)
                    LoadRecord(record);
            }
        }

        private void btnKumasKodu_Click(object sender, RoutedEventArgs e)
        {
            wins.winNumaratorListesi win = new wins.winNumaratorListesi(Enums.Inventory.Kumas);
            win.ShowDialog();
            if (win.SatirSecildi)
            {
                string number = (win.Number + 1).ToString("D3");
                txtKodu.Text = win.Prefix + number;
                lblKumasAdi.Text = win.NameX;
                PrefixId = win.Id;
            }
        }

        private void btnDesen_Click(object sender, RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("Desen", Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                DesenId = win.Id;
                txtKumasDesen.Text = win.Explanation;
            }
        }

        private void btnDokumaCinsi_Click(object sender, RoutedEventArgs e)
        {
            wins.winOzellikSecimi win = new wins.winOzellikSecimi("Dokuma Cinsi", Convert.ToInt32(Enums.Inventory.Kumas));
            win.ShowDialog();
            if (win.SecimYapildi)
            {
                DokumaCinsiId = win.Id;
                txtDokumaCinsi.Text = win.Explanation;
                lblCinsi.Text = win.Explanation;
            }
        }
    }
}
