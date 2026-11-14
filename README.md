# BakuRent — Bakı kirayə elanları

ASP.NET Core MVC ilə hazırlanmış web tətbiqi. İstifadəçilər Bakıda kirayə mənzil elanlarını görə, yarada, redaktə edə və silə bilərlər.

**Fənn:** Proqramlaşdırma texnologiyaları
**İş:** Laboratoriya işi № 9 — ASP.NET Core MVC ilə web tətbiqinin hazırlanması
**Müəllif:** Aitaj

---

## İstifadə olunan texnologiyalar

| Hissə | Texnologiya |
|---|---|
| Backend | C#, ASP.NET Core MVC |
| Frontend | Razor View, HTML, CSS, Bootstrap 5 |
| Verilənlər bazası | SQLite |
| ORM | Entity Framework Core |
| Validasiya | Data Annotations (server) + jQuery Validation (brauzer) |

---

## Funksiyalar

- Ana səhifə: ümumi, aktiv və kirayə verilmiş elanların sayı
- Elanların siyahısı
- Yeni elan yaratmaq
- Elanın ətraflı məlumatına baxmaq
- Elanı redaktə etmək
- Elanı silmək (təsdiq səhifəsi ilə)
- Statusu dəyişmək: **Aktiv ↔ Kirayə verilib** (siyahıda düymə ilə və redaktə səhifəsində)
- Form validasiyası: başlıq, təsvir, rayon, otaq sayı, qiymət, telefon

### Əlavə fərdi funksiyalar

- Başlıq və təsvirə görə axtarış
- Rayona görə filtr
- Statusa görə filtr
- Tarixə və qiymətə görə sıralama
- Ana səhifədə ümumi elan sayının göstərilməsi

---

## MVC arxitekturasının tətbiqi

| Komponent | Fayllar | Vəzifəsi |
|---|---|---|
| **Model** | `Models/Listing.cs`, `Models/ListingOptions.cs` | Elanın məlumat strukturu, validasiya qaydaları, rayon və status siyahıları |
| **View** | `Views/Home/*`, `Views/Listings/*`, `Views/Shared/*` | İstifadəçinin brauzerdə gördüyü səhifələr (Razor) |
| **Controller** | `Controllers/HomeController.cs`, `Controllers/ListingsController.cs` | Sorğunu qəbul edir, bazadan məlumatı alır və uyğun View-a ötürür |
| **Data** | `Data/AppDbContext.cs` | Entity Framework Core vasitəsilə verilənlər bazası ilə əlaqə |

Verilənlər bazası ilə əlaqə yalnız backend-də (Controller və `AppDbContext`) qurulur. View səhifələri bazaya birbaşa müraciət etmir.

---

## Qovluq strukturu

```
BakuRent/
├── .gitignore
├── README.md
└── BakuRentWeb/
    ├── BakuRentWeb.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── Properties/
    │   └── launchSettings.json
    ├── Controllers/
    │   ├── HomeController.cs
    │   └── ListingsController.cs
    ├── Data/
    │   └── AppDbContext.cs
    ├── Models/
    │   ├── Listing.cs
    │   ├── ListingOptions.cs
    │   └── ErrorViewModel.cs
    ├── Views/
    │   ├── _ViewImports.cshtml
    │   ├── _ViewStart.cshtml
    │   ├── Home/
    │   │   └── Index.cshtml
    │   ├── Listings/
    │   │   ├── Index.cshtml
    │   │   ├── _Form.cshtml
    │   │   ├── Create.cshtml
    │   │   ├── Edit.cshtml
    │   │   ├── Details.cshtml
    │   │   └── Delete.cshtml
    │   └── Shared/
    │       ├── _Layout.cshtml
    │       ├── _ValidationScriptsPartial.cshtml
    │       └── Error.cshtml
    └── wwwroot/
        └── css/
            └── site.css
```

---

## Verilənlər bazası

SQLite istifadə olunur. Ayrıca server quraşdırmaq lazım deyil, baza adi fayl kimi saxlanılır.

- Bağlantı sətri `appsettings.json` faylındadır: `Data Source=bakurent.db`
- Tətbiq ilk dəfə işə düşəndə `bakurent.db` faylı avtomatik yaranır (`EnsureCreated()`)
- Cədvəl boşdursa, 3 test elanı avtomatik əlavə olunur

**Elan cədvəli (`Listings`):**

| Sahə | Tip | Qayda |
|---|---|---|
| Id | int | Unikal identifikator |
| Title | string | Məcburi, maksimum 120 simvol |
| Description | string | Məcburi |
| District | string | Məcburi (Bakı rayonlarından biri) |
| Rooms | int | 1 – 10 |
| Price | int | 50 – 20000 AZN |
| Phone | string | Məcburi, telefon formatı yoxlanılır |
| Status | string | `Active` və ya `Rented` |
| CreatedAt | DateTime | Yaradılma tarixi |

---

## Necə işə salmaq olar

### Tələblər

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) (və ya daha yeni)
- Git

Quraşdırılmış versiyanı yoxlamaq üçün:

```bash
dotnet --version
```

### Addımlar

```bash
git clone https://github.com/<istifadəçi-adı>/BakuRent.git
cd BakuRent/BakuRentWeb
dotnet run
```

İlk işə salınmada paketlər yüklənir (internet lazımdır). Terminalda belə sətir görünəndən sonra tətbiq hazırdır:

```
Now listening on: http://localhost:5080
```

Brauzerdə **http://localhost:5080** ünvanını aç.

HTTPS ilə işə salmaq üçün:

```bash
dotnet run --launch-profile https
```

Ünvan: **https://localhost:7080**. Brauzer sertifikat xəbərdarlığı versə, bu lokal inkişaf sertifikatıdır. Əgər xəbərdarlıq çıxırsa, bir dəfə `dotnet dev-certs https --trust` əmrini icra et.

### Qeyd: .NET 10 istifadə edirsənsə

`BakuRentWeb/BakuRentWeb.csproj` faylında bu dəyişiklikləri et:

- `net8.0` → `net10.0`
- `Microsoft.EntityFrameworkCore.Sqlite` və `Microsoft.EntityFrameworkCore.Design` paketlərinin `8.0.*` versiyasını `10.0.*` ilə əvəz et.

---

## Əsas səhifələr

| Ünvan | Təsvir |
|---|---|
| `/` | Ana səhifə |
| `/Listings` | Elanların siyahısı, axtarış, filtr, sıralama |
| `/Listings/Create` | Yeni elan |
| `/Listings/Details/{id}` | Elanın detalları |
| `/Listings/Edit/{id}` | Redaktə |
| `/Listings/Delete/{id}` | Silmə təsdiqi |

---

## Ekran görüntüləri

> Ekran görüntüləri `screenshots/` qovluğuna əlavə olunacaq.

- [ ] Ana səhifə
- [ ] Elanların siyahısı
- [ ] Yeni elan forması
- [ ] Elanın detalları
- [ ] Redaktə səhifəsi
- [ ] Silmə əməliyyatı
- [ ] Validasiya nəticəsi

---

## Məhdudiyyətlər və gələcək işlər

- Bazanın strukturu `EnsureCreated()` ilə yaradılır. Sonrakı layihələrdə strukturu dəyişmək üçün EF Core Migrations istifadə olunmalıdır.
- Hələlik istifadəçi qeydiyyatı (autentifikasiya) yoxdur.
- Şəkil yükləmə və xəritə inteqrasiyası əlavə oluna bilər.
