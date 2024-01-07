# [Proje Adı]: .NET 7 ile Geliştirilmiş Bankacılık API'si

## Giriş
Bu proje, günümüz bankacılık ihtiyaçlarına uygun olarak geliştirilmiş, .NET 7 ve Entity Framework Core kullanılarak inşa edilmiş bir RESTful API'dir. Modern web teknolojileri ve uygulama güvenliğine odaklanan bu API, gerçek dünya bankacılık işlevlerini simüle eder.

## Özellikler
- Kullanıcı Yönetimi: Kayıt, giriş, bilgi güncelleme, rollerle erişim kontrolü.
- Hesap Yönetimi: Yeni hesap açma, bakiye sorgulama ve güncelleme.
- Para İşlemleri: Yatırma, çekme, transferler ve işlem kayıtları.
- Kredi İşlemleri: Başvurular, ödeme planları ve sorgulama.
- Otomatik Ödemeler: Ayarlar ve yönetim.
- Destek Talepleri: Müşteri desteği ve durum takibi.

## Teknolojiler
- .NET 7
- Entity Framework Core
- SQL Server
- Docker / Docker Compose
- JWT (JSON Web Tokens) için kimlik doğrulama

## Mimari

Projede, .NET Core tabanlı clean architecture yaklaşımı benimsenmiştir. Aşağıda projenin ana bileşenleri ve bu bileşenlerin rolleri açıklanmıştır:

### Repository Katmanı
- **Generic Repository**: CRUD (Create, Read, Update, Delete) işlemleri için genel bir temel oluşturur. `GenericRepository<TEntity, TKey>` sınıfı, tüm varlıklar için ortak olan operasyonları içerir.
- **Uzmanlaşmış Repository'ler**: Belirli varlık türleri için özelleştirilmiş işlemleri gerçekleştirir. Örneğin, `AccountRepository` sınıfı, hesaplarla ilgili özelleştirilmiş sorgular ve işlemleri içerir.

### Unit of Work
- **UnitOfWork**: Veritabanı işlemlerinin atomic (bütünlüklü) olarak yönetilmesini sağlar. `UnitOfWork` sınıfı, veritabanı işlemlerinin başlatılması, commit edilmesi ve geri alınması işlevlerini yönetir.

### Servis Katmanı
- **Servisler**: İş mantığını içeren sınıflardır. `AccountService` gibi servisler, belirli işlevler için gerekli iş mantığını ve iş akışını yönetir. Servisler, repository'leri ve diğer gerekli bileşenleri kullanarak iş mantığını uygular.

### API Katmanı
- **Controller'lar**: HTTP isteklerini karşılayan ve uygun servis metodlarını çağıran sınıflardır. `AccountsController` gibi controller'lar, API endpoint'leri olarak hizmet eder ve gelen isteklere göre uygun servis metodlarını tetikler.

### Veri Aktarım Objeleri (DTOs)
- **DTOs**: Servis katmanı ile API katmanı arasında veri taşıyan objelerdir. Örneğin, `CreateAccountRequestDto` sınıfı, bir hesap oluşturmak için gerekli verileri taşır.

### AutoMapper
- AutoMapper, nesneler arasındaki veri transferini kolaylaştırmak için kullanılır. Bu, özellikle veritabanı varlıkları ve DTO'lar arasında veri dönüşümü gerektiğinde yararlıdır.

### Kimlik Yönetimi ve Güvenlik
- **Identity Framework**: Kullanıcı kimlik doğrulaması ve yönetimi için kullanılır. 
- **JWT (JSON Web Tokens)**: API kimlik doğrulaması ve yetkilendirmesi için kullanılır.

### Validasyon
- **FluentValidation**: Gelen verilerin doğruluğunu ve tutarlılığını kontrol etmek için kullanılır. Bu, API katmanında veri validasyonu için etkin bir yol sağlar ve iş mantığı katmanına geçmeden önce girdilerin uygunluğunu garanti eder.

Bu mimari, projenin genişleyebilir, sürdürülebilir ve test edilebilir olmasını sağlar. Ayrıca, farklı katmanların ayrılması, projenin daha kolay yönetilmesine ve geliştirilmesine olanak tanır.

## API Dokümantasyonu

## Kullanıcı Yönetimi (`api/users`)

- `POST /api/users`: Yeni bir kullanıcı oluşturur.
- `PUT /api/users/{id}`: Belirli bir kullanıcının bilgilerini günceller.

## Oturum Yönetimi (`AuthController`)

- `POST /login`: Kullanıcı girişi yapar ve JWT token döndürür.

## Hesap İşlemleri (`AccountsController`)

- `GET /{id}/balance`: Belirli bir hesabın bakiyesini sorgular.
- `POST /`: Yeni bir hesap oluşturur (Yalnızca Admin).
- `PUT /{id}/balance`: Belirli bir hesabın bakiyesini günceller (Yalnızca Admin).
- `PUT /{accountId}/loan-payment`: Bir hesaptan kredi ödemesi yapar.

## Para İşlemleri (`TransactionsController`)

- `POST /api/transactions/deposit`: Hesaba para yatırma.
- `POST /api/transactions/withdraw`: Hesaptan para çekme.
- `POST /api/transactions/transfer/internal`: Hesaplar arası iç transfer.
- `POST /api/transactions/transfer/external`: Hesaplar arası dış transfer.

## Kredi Başvuruları (`LoanApplicationsController`)

- `POST /api/loanapplications`: Yeni kredi başvurusu oluşturur.
- `GET /api/loanapplications/{id}/status`: Belirli bir kredi başvurusunun durumunu sorgular.
- `GET /api/loanapplications/{id}/recommendation`: Belirli bir kredi başvurusu için tavsiye alır (Admin, Auditor).
- `PUT /api/loanapplications/{id}/reject`: Belirli bir kredi başvurusunu reddeder (Yalnızca Admin).
- `POST /api/loanapplications/{id}/approve`: Belirli bir kredi başvurusunu onaylar ve kredi oluşturur (Yalnızca Admin).

## Ödemeler (`PaymentsController`)

- `POST /create-payment`: Yeni bir ödeme oluşturur.
- `PUT /{id}/update`: Belirli bir ödemeyi günceller.
- `DELETE /{id}/delete`: Belirli bir ödemeyi siler.

## Destek Talepleri (`SupportTicketsController`)

- `POST /`: Yeni destek talebi oluşturur.
- `GET /{id}`: Belirli bir destek talebini sorgular.
- `GET /user/{userId}`: Belirli bir kullanıcının tüm destek taleplerini sorgular.
- `GET /`: Tüm destek taleplerini sorgular (Admin, Auditor).
- `PUT /{id}/status`: Belirli bir destek talebinin durumunu günceller (Yalnızca Admin).
- `PUT /{id}/priority`: Belirli bir destek talebinin önceliğini günceller (Yalnızca Admin).

## İşlem Başvuruları (`TransactionApplicationsController`)

- `POST /{id}/approve`: Belirli bir işlem başvurusunu onaylar (Yalnızca Admin).
- `POST /{id}/reject`: Belirli bir işlem başvurusunu reddeder (Yalnızca Admin).

## Kurulum
### Önkoşullar
- .NET 7 SDK
- Docker ve Docker Compose
- SQL Server (Docker üzerinde veya yerel)

### Yerel Kurulum
1. Projeyi GitHub'dan klonlayın: `git clone [repo-link]`
2. Proje dizinine gidin: `cd [proje-adı]`
3. Uygulamayı başlatın: `dotnet run`

### Docker ile Kurulum
Bu proje Docker kullanılarak kolayca kurulup çalıştırılabilir. Projeyi Docker ile çalıştırmak için aşağıdaki adımları izleyin:

1. Docker ve Docker Compose yüklü olduğundan emin olun.
2. Bu komutu çalıştırarak uygulamayı başlatın: `docker-compose up`

#### Docker Compose Yapılandırması
Projenin Docker Compose yapılandırması aşağıdaki gibidir:

```yaml
version: '3.4'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    volumes:
      - sqlserver_volume:/var/opt/mssql
  bankproject.api:
    image: buraksenses/bankprojectapi:${version}
    container_name: bankprojectapi-container
    build:
      context: .
      dockerfile: BankProject.API/Dockerfile

volumes:
  sqlserver_volume:
```
