# LightHouse Application

Bu proje, deniz fenerleri hakkında bilgi paylaşımı yapabilen bir .NET 8.0 web API uygulamasıdır.

## Docker ile Çalıştırma

### Gereksinimler
- Docker Desktop
- Docker Compose

### Servisler
1. **PostgreSQL Database** - Port 5432
2. **pgAdmin** - Port 5050 (admin@admin.com / 123456)
3. **LightHouse Web API** - Port 5000 (HTTP) ve 5001 (HTTPS)

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
- **GET** `/api/lighthouse` - Tüm deniz fenerlerini listele
- **POST** `/api/lighthouse` - Yeni deniz feneri ekle

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
├── LightHouseDomain/          # Domain entities ve interfaces
├── LightHouseApplication/      # Application services ve DTOs
├── LightHouseInfrastructure/  # Infrastructure services
├── LightHouseData/            # Data access layer
└── LightHouseWebApi/          # Web API project
```

### Build
```bash
# Solution'ı build et
dotnet build src/LightHouseApp.sln

# Web API'yi çalıştır
cd src/LightHouseWebApi
dotnet run
```

### Test
```bash
# Unit testleri çalıştır
dotnet test src/LightHouseTests/LightHouseTests.csproj

# Integration testleri çalıştır
dotnet test src/LightHouseIntegrationTests/LightHouseIntegrationTests.csproj
```
