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

### 5. Otomatik Boş Satır Eklenmesi (Çözüldü)
- Problem: İşlem Tipi dropdownundan seçim yapınca boş satır eklenmiyordu
- Çözüm: `gridKalemler_CellEditEnding` ve `gridKalemler_CurrentCellChanged` event'leri eklendi
- Logic:
  - `CellEditEnding`: İşlem Tipi değişince `CommitEdit` + `CheckAndAddEmptyRow()` çağrılır
  - `CurrentCellChanged`: Hücre değişince `CommitEdit` + `CheckAndAddEmptyRow()` çağrılır
  - `PreviewKeyDown`: Enter tuşunda son sütundaysa yeni satır eklenir

### 6. Kayıt Listeleme ve Yükleme (Devam Edilecek)
- `Listele()`: Fiş listesi penceresi açılır, seçilen kayıt yüklenir
- `LoadFromId()`: Receipt + ReceiptItem + Firma + Depo bilgileri yüklenir
- `GetItemsByReceiptId()`: ReceiptItem + Malzeme bilgileri sorgulanır
- Model güncellemeleri: `ReceiptItem.MeasurementUnit`, `ReceiptItem.MaterialCode`, `ReceiptItem.MaterialName`

### 7. Fiş Listesi Boş Geliyor (Araştırılacak)
- Problem: `winFisListesiV2` açılıyor ama grid boş geliyor
- `GetByTypeList()` metodu çalışıyor ama veri dönmüyor olabilir
- Veya `Window_Loaded` eventi tetiklenmiyor olabilir
- Debug için label'a durum yazdırıldı ama hiçbirşey görünmedi
- Yarın kontrol edilecek: Grid'in ItemsSource'u doğru set ediliyor mu?

### 4. Sayı Formatı (Çözüldü)
- Problem: Kg, Mt, Adet, Birim Fiyat, KDV alanlarında ondalık ayracı sorunu
- Çözüm: XAML binding'lerine `ConverterCulture=tr-TR` eklendi
- Örnek: `StringFormat={}{0:N2}, ConverterCulture=tr-TR`
- Sonuç: 1035 yazınca 1.035,00 olarak görünür

---

## Yapılacaklar

### 1. Hesaplama Sorunu (Çözüldü)
**Problem:** Alt satıra inmeden (hücreden çıkmadan) hesaplama yapılmıyor
**Çözüm:** XAML binding'lerine `UpdateSourceTrigger="PropertyChanged"` eklendi

### 2. Enter ile Navigasyon Testi
- PreviewKeyDown eventi mevcut
- Test edilmesi gerekiyor

### 3. Malzeme Seçim Penceresi Veri Aktarımı
- winMalzemeListesiV2 çift tıklama ile seçim yapıyor
- Veri aktarımı kontrol edilmeli

### 4. İşlem Tiplerini Database'den Yükleme (Tamamlandı)
- `UC_MalzemeFisV2.xaml.cs` için `LoadOperationTypes()` metodu `UtilityHelpers._uh.GetOperationTypeList` kullanıyor
- DataGridComboBoxColumn ile çalışıyor
- Diğer v2 pencereleri için de aynı yöntem kullanılabilir

---

---

### 8. Receipt ve ReceiptItem Modelleri DB Uyumlu Güncelleme (Tamamlandı)
- `Receipt.cs` güncellendi:
  - DispatchNo, DispatchDate, DuaDate, Authorized, Approved, IsFinished, PaymentType
  - SavedUser, SavedDate, UpdatedUser, UpdatedDate
  - CarrierId, TransporterId eklendi
  - Kalem alanları (OperationType, InventoryId, WareHouseId, NetMeter, NetWeight, Piece, UnitPrice, Vat, RowAmount, CashPayment, DeferredPayment, TrackingNumber, InvoiceNo, Receiver, DocumentName, OrderNo) NotMapped olarak eklendi
- `ReceiptItem.cs` güncellendi:
  - GrM2, GrossWeight, NetWeight, GrossMeter, ForexPrice, Forex
  - VariantId, ColorId, Explanation, UUID, PatternId, ProcessId, Brand, Wastage, Quantity
  - Variant, BatchNo, OrderNo, CustomerOrderNo, IsCostCalculated, ReceiptNo eklendi

### 9. winFisListesiV2 Güncellemeleri (Tamamlandı)
- `SecilenKalemler` property'si eklendi (List<ReceiptItemViewModel> döndürür)
- `grid_MouseDoubleClick` metodu güncellendi:
  - Seçilen fişin kalemlerini `_repo.GetItemsByReceiptId` ile yükler
  - ReceiptItemViewModel'e dönüştürür
  - `DialogResult = true` ve `Close()` ile pencereyi kapatır

### 10. UC_MalzemeFisV2 Listele() Aktif Edildi (Tamamlandı)
- `Listele()` metodu satır 461-500 arası aktif edildi (yorum satırı kaldırıldı)
- `_items` ObservableCollection kullanılıyor (DataTable yerine)
- Fiş seçildiğinde kalemler yükleniyor

### 11. ReceiptListDto Oluşturuldu (Tamamlandı)
- Problem: MiniOrm.GetMovementList JOIN ile CompanyName, WareHouseName gibi alanları döndürüyor
- Receipt modelinde NotMapped olduğu için dynamic erişim hata veriyordu
- Çözüm: `ReceiptListDto.cs` oluşturuldu
- Türkçe Display attribute'ları eklendi (Fiş No, Firma Adı, Malzeme Kodu, vb.)

---

## İlgili Dosyalar

- `v2/Models/ReceiptItemViewModel.cs` - Satır ViewModel
- `v2/UserControls/UC_MalzemeFisV2.xaml` - Fiş formu
- `v2/UserControls/UC_MalzemeFisV2.xaml.cs` - Code-behind
- `v2/Windows/winMalzemeListesiV2.xaml/cs` - Malzeme seçim penceresi
- `v2/Windows/winFisListesiV2.xaml/cs` - Fiş listesi penceresi
- `v2/Models/Receipt.cs` - Fiş modeli
- `v2/Models/ReceiptItem.cs` - Kalem modeli
- `v2/Models/ReceiptListDto.cs` - Liste için DTO (yeni)
