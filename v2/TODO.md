# v2 Geliştirme Notları

## Tamamlanan İşlemler

### 1. MVVM Yapısı
- `ReceiptItemViewModel.cs` oluşturuldu
- `INotifyPropertyChanged` ile otomatik hesaplama
- Satır Tutari = (Kg/Mt/Adet) × BirimFiyat × (1 + KDV/100)

### 2. UC_MalzemeFisV2 Güncellemeleri
- DataTable → ObservableCollection<ReceiptItemViewModel>
- Boş satır kontrolü (CanUserAddRows="False")
- İşlem Tipi seçilince otomatik boş satır ekleme

### 3. Grid Özellikleri
- Kodu sütunu: TextBox + Button (malzeme seçimi)
- Fiyat Birimi: ComboBox (Kg/Mt/Adet)
- Enter tuşu ile navigasyon

---

## Yapılacaklar

### 1. Hesaplama Sorunu (Öncelikli)
**Problem:** Alt satıra inmeden (hücreden çıkmadan) hesaplama yapılmıyor
**Çözüm:** XAML binding'lerine `UpdateSourceTrigger="PropertyChanged"` ekle

### 2. Enter ile Navigasyon Testi
- PreviewKeyDown eventi mevcut
- Test edilmesi gerekiyor

### 3. Malzeme Seçim Penceresi Veri Aktarımı
- winMalzemeListesiV2 çift tıklama ile seçim yapıyor
- Veri aktarımı kontrol edilmeli

---

## İlgili Dosyalar

- `v2/Models/ReceiptItemViewModel.cs` - Satır ViewModel
- `v2/UserControls/UC_MalzemeFisV2.xaml` - Fiş formu
- `v2/UserControls/UC_MalzemeFisV2.xaml.cs` - Code-behind
- `v2/Windows/winMalzemeListesiV2.xaml/cs` - Malzeme seçim penceresi
