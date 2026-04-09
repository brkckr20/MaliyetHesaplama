# Yapılacaklar - Stok Entegrasyonu

## 🙋‍♂️ Sorun
Malzeme Giriş/Güncelleme sırasında stok tablosu güncellenmiyor.

## 🔍 Kök Neden
Güncelleme sırasında `orm.GetById<dynamic>("ReceiptItem", satirId, "Id")` çağrısı çalışıyor ancak:
1. Ya eski değerler DB'den gelmiyor (null)
2. Ya da fark hesabı doğru yapılmıyor

## 📋 Yapılacaklar

### 1. Debug Log Ekleme
- `UC_MalzemeGirisCikis.xaml.cs` içinde güncelleme sırasında:
  - satirId değerini logla
  - eskiKalem'i logla (null mı, değerleri var mı)
  - oncekiAdet, oncekiKg, oncekiMeter değerlerini logla
  - yeniAdet, yeniKg, yeniMeter değerlerini logla
  - farkAdet, farkKg, farkMeter değerlerini logla

### 2. DB Sorgusunu Kontrol Et
- `MiniOrm.GetById` metodunun dynamic döndürüp döndürmediğini kontrol et
- Belki generic tip mapping çalışmıyor

### 3. Alternatif Çözüm - Direkt Sorgu
Eski değeri çekmek yerine direkt SQL sorgusu ile çekmeyi dene:

```csharp
string sql = $"SELECT ISNULL(Piece,0) as Piece, ISNULL(NetWeight,0) as NetWeight, ISNULL(NetMeter,0) as NetMeter FROM ReceiptItem WHERE Id = {satirId}";
dynamic eskiKalem = orm.QueryRaw<dynamic>(sql).FirstOrDefault();
```

### 4. Stok Güncelleme Mantığını Kontrol Et
- INSERT durumunda çalışıyor (yeni kayıt)
- UPDATE durumunda çalışmıyor (güncelleme)
- UPDATE'te fark 0 geliyor olabilir

---

## 📝 MiniOrm Notu
MiniOrm dosyası hiç bozulmamıştı (kullanıcı isteği), ama ileride transaction desteği gerekebilir.