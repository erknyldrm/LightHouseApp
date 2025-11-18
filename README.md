# LightHouse Application

Bu proje, deniz fenerleri hakkında bilgi paylaşımı yapabilen bir .NET 8.0 uygulama paketidir. Clean Architecture prensiplerine uygun olarak geliştirilmiştir ve aşağıdaki bileşenleri içerir:

- **LightHouseWebApi**: RESTful Web API
- **LightHouseODataApi**: OData Web API (sorgulama, filtreleme, sayfalama)
- **LightHouseBackOffice**: Razor Pages web uygulaması (Yönetim paneli)
- **TerminalApp**: Console uygulaması

## Docker ile Çalıştırma

### Gereksinimler
- Docker Desktop
- Docker Compose

### Servisler
1. **PostgreSQL Database** - Port 5432
2. **pgAdmin** - Port 5050 (admin@admin.com / 123456)
3. **LightHouse Web API** - Port 5000 (HTTP) ve 5001 (HTTPS)
4. **LightHouse OData API** - OData sorgulama endpoint'leri
5. **LightHouse BackOffice** - Razor Pages web uygulaması (Yönetim paneli)

### Başlatma
```bash
# Tüm servisleri başlat
docker-compose up -d

# Sadece belirli servisleri başlat
docker-compose up -d postgres
docker-compose up -d lighthouse-app

# Logları görüntüle
docker-compose logs -f lighthouse-app
```

### Durdurma
```bash
# Tüm servisleri durdur
docker-compose down

# Veritabanı verilerini koruyarak durdur
docker-compose down

# Veritabanı verilerini de silerek durdur
docker-compose down -v
```

### API Endpoints

#### LightHouse Web API (RESTful)
- **GET** `/api/lighthouse` - Tüm deniz fenerlerini listele
- **POST** `/api/lighthouse` - Yeni deniz feneri ekle
- **GET** `/api/lighthouse/{id}` - Belirli bir deniz fenerini getir
- **PUT** `/api/lighthouse/{id}` - Deniz fenerini güncelle
- **DELETE** `/api/lighthouse/{id}` - Deniz fenerini sil

#### LightHouse OData API
- **GET** `/odata/LightHouses` - OData sorgulama desteği ile deniz fenerlerini listele
  - Örnek: `/odata/LightHouses?$filter=Name eq 'Test'&$orderby=Name&$top=10`

### Veritabanı Bağlantısı
- **Host**: localhost (veya postgres container adı)
- **Port**: 5432
- **Database**: lighthousedb
- **Username**: admin
- **Password**: admin

### pgAdmin Erişimi
- **URL**: http://localhost:5050
- **Email**: admin@admin.com
- **Password**: 123456

## Geliştirme

### Proje Yapısı
```
src/
├── LightHouseDomain/              # Domain entities, interfaces ve value objects
├── LightHouseApplication/         # Application services, DTOs ve validators
├── LightHouseInfrastructure/      # Infrastructure services (caching, storage, identity)
├── LightHouseData/                # Data access layer (repositories)
├── LightHouseWebApi/              # RESTful Web API
├── LightHouseODataApi/            # OData Web API
├── LightHouseBackOffice/          # Razor Pages web uygulaması (Yönetim paneli)
├── Clients/
│   └── TerminalApp/               # Console uygulaması
├── LightHouseTests/               # Unit testler
└── LightHouseIntegrationTests/    # Integration testler
```

### Build
```bash
# Solution'ı build et
dotnet build src/LightHouseApp.sln

# Web API'yi çalıştır
cd src/LightHouseWebApi
dotnet run

# OData API'yi çalıştır
cd src/LightHouseODataApi
dotnet run

# BackOffice uygulamasını çalıştır
cd src/LightHouseBackOffice
dotnet run

# Terminal uygulamasını çalıştır
cd src/Clients/TerminalApp
dotnet run
```

### Test
```bash
# Unit testleri çalıştır
dotnet test src/LightHouseTests/LightHouseTests.csproj

# Integration testleri çalıştır
dotnet test src/LightHouseIntegrationTests/LightHouseIntegrationTests.csproj
```

## LightHouse BackOffice

LightHouse BackOffice, deniz fenerlerini yönetmek için kullanılan Razor Pages tabanlı web uygulamasıdır.

### Özellikler
- Deniz feneri oluşturma (Create)
- Deniz feneri listeleme (List)
- Form validasyonu
- Web API entegrasyonu

### Çalıştırma
```bash
cd src/LightHouseBackOffice
dotnet run
```

Uygulama varsayılan olarak `http://localhost:5000` veya `https://localhost:5001` adresinde çalışacaktır.

### Yapılandırma
`appsettings.json` dosyasında API base URL'ini yapılandırmanız gerekebilir:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

### Sayfalar
- **Ana Sayfa** (`/`) - Dashboard
- **Create LightHouse** (`/Shared/LightHouse/Create`) - Yeni deniz feneri oluşturma
- **List LightHouse** (`/Shared/LightHouse/List`) - Deniz fenerlerini listeleme

## LightHouse OData API

OData protokolü ile deniz fenerlerini sorgulama, filtreleme ve sayfalama işlemleri yapabileceğiniz API.

### Özellikler
- OData v4 desteği
- Filter, OrderBy, Select, Expand, Count işlemleri
- Maksimum 100 kayıt limiti ($top)

### Çalıştırma
```bash
cd src/LightHouseODataApi
dotnet run
```

### Örnek Sorgular
```
GET /odata/LightHouses
GET /odata/LightHouses?$filter=Name eq 'Test'
GET /odata/LightHouses?$orderby=Name desc
GET /odata/LightHouses?$top=10&$skip=0
GET /odata/LightHouses?$count=true
```

## Teknolojiler

- **.NET 8.0**
- **ASP.NET Core** (Web API, Razor Pages)
- **PostgreSQL** (Veritabanı)
- **OData** (Sorgulama API)
- **Serilog** (Logging)
- **Swagger** (API Dokümantasyonu)
- **Docker** (Containerization)
