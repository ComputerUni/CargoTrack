# CargoTrack - Kargo Takip ve Yönetim Sistemi

CargoTrack, modern bir kargo takip ve yönetim platformudur. Kullanıcılar kargolarını takip edebilir, işletmeciler ve yöneticiler ise tam operasyonel kontrol sağlayabilir.

## 🎯 Temel Özellikler

### Müşteri Paneli (User)
- **Kargo Takip**: Gönderilen, alınan ve teslim edilen kargolarını gerçek zamanlı takip et
- **Kargo Detayları**: Konumlandırma verisi, tahmini teslim tarihi ve tarihçe görüntüle
- **Canlı Harita**: Kargo konumunu canlı olarak takip et
- **Kargo Yönetimi**: Tüm kargolarını kategorilere göre filtrele ve ara

### Şube Müdürü Paneli (Manager)
- **Kargo Yönetimi**: Gelen ve giden kargolar için işlemler gerçekleştir
- **Kargo Hareketi**: Kargolara merkez ara transferleri kaydet
- **Teslimat Kontrolü**: Başarılı/başarısız teslimatları doğrula
- **Şube Kapasite İzleme**: Şubedeki kargo yoğunluğunu görselleştir
- **Kurye Performansı**: Kuriye göre performans analitiği

### Yönetici Paneli (Admin)
- **Sistem Yönetimi**: Şubeler, çalışanlar, şehirler ve transfer merkezlerini yönet
- **Kargo Kontrolü**: Sistem genelinde tüm kargolara erişim
- **Fiyatlandırma Kuralları**: Kargo türlerine göre fiyat tetikleyicileri belirle
- **İşletme Raporları**: Detaylı performans, şube ve kuriye raporları
- **Denetim Günlüğü**: Sistem aktivitelerinin tam kaydı
- **Dashboard**: Gerçek zamanlı KPI metrikleri ve grafikler

## 📊 Sistem Mimarisi

CargoTrack, **N-Tier Architecture** ile geliştirilmiştir:

- **CargoTrack.Entity**: Veritabanı modelleri ve enumerasyonlar
- **CargoTrack.DataAccess**: Veri erişim katmanı ve repository pattern
- **CargoTrack.Business**: İş mantığı ve servisleri
- **CargoTrack.DTO**: Veri transfer nesneleri
- **CargoTrack.WebUI**: Razor Pages tabanlı web arayüzü

## 🛠️ Teknik Yığın

- **Framework**: .NET 8
- **Web**: ASP.NET Core Razor Pages
- **Veritabanı**: Entity Framework Core
- **Doğrulama**: Fluent Validation
- **Raporlama**: QuestPDF
- **Stil**: Bootstrap 5
- **Validation**: jQuery Validation

## 🚀 Kurulum

### Adımlar

1. **Repository'yi Klonla**
```bash
git clone https://github.com/ComputerUni/CargoTrack.git
cd CargoTrack
```

2. **Bağımlılıkları Yükle**
```bash
dotnet restore
```

3. **Veritabanı Bağlantısını Yapılandır**
- `appsettings.json` dosyasında bağlantı stringini güncelle
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=CargoTrackDB;Trusted_Connection=true;"
}
```

4. **Veritabanını Oluştur**
```bash
dotnet ef database update
```

5. **Uygulamayı Çalıştır**
```bash
cd CargoTrack/CargoTrack.WebUI
dotnet run
```

Uygulama şu adreste çalışacaktır: `https://localhost:5001`

## 👥 Kullanıcı Rolleri

| Rol | Erişim |
|-----|--------|
| **Admin** | Sistem geneli kontrol, rapor, yönetim |
| **Manager** | Şube operasyonları, kargo kontrolü |
| **User** | Kendi kargolarını takip etme |

## 📸 Ekran Görüntüleri

<img src="screenshots/Ekran görüntüsü 2026-10-04 174523.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174535.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174557.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174607.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174623.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174659.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174731.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174747.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174758.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174826.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174839.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174850.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174907.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174916.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174940.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 174950.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 175017.png" width="800"/>

<img src="screenshots/Ekran görüntüsü 2026-10-04 175110.png" width="800"/>

## 📁 Proje Yapısı

```
CargoTrack/
├── CargoTrack.Entity/          # Veri modelleri
├── CargoTrack.DataAccess/      # Veri erişim katmanı
├── CargoTrack.Business/        # İş mantığı ve servisler
├── CargoTrack.DTO/             # Veri transfer nesneleri
└── CargoTrack.WebUI/           # Web arayüzü
	├── Areas/Admin/            # Yönetici paneli
	├── Areas/Manager/          # Şube müdürü paneli
	├── Areas/User/             # Müşteri paneli
	└── wwwroot/                # Statik dosyalar
```

## 🔑 Ana Varlıklar

- **Cargo**: Kargo bilgileri, konumu ve durumu
- **CargoMovement**: Kargonun hareketi (şubeler arası transfer)
- **Delivery**: Teslimat detayları ve sonuçları
- **Employee**: Çalışan bilgileri
- **Branch**: Şube yönetimi
- **City**: Şehir tanımlaması
- **TransferCenter**: Orta aktarma merkezleri
- **User**: Kullanıcı hesapları ve roller


---

**Repository**: [https://github.com/ComputerUni/CargoTrack](https://github.com/ComputerUni/CargoTrack)
