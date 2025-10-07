### 🇬🇧 English

# 🧾 Project Overview

**Transformer Quality Control System** is a mobile and web-based application developed for a transformer manufacturing plant.
The system retrieves production data from the factory server and allows quality control staff to enter test results through the mobile application.
After verification, both product information and test results are saved to another database hosted on a separate server.

---

### 🧩 Features

- 🏭 **Production Data Fetching** — Automatically retrieves production records from the factory server

- ✅ **Quality Test Entry** — Operators can input quality control test results via mobile interface

- 🔄 **Dual Database Sync** — Saves product and test data to a secondary server for reporting and traceability

- 📱 **Mobile Interface (MAUI)** — User-friendly app designed for on-site quality personnel

- 🌐 **API (ASP.NET Core)** — Handles data exchange between production and quality databases

- 💾 **Secure Data Management** — Reliable SQL Server integration

- 🧱 **Clean Architecture** — MVVM and Repository design for maintainability

---

### 🏗️ Tech Stack

- **Frontend:** .NET MAUI
- **Backend:** ASP.NET Core Web API
- **Database:** Microsoft SQL Server
- **Architecture:** MVMM + Repository Pattern
- **Language:** C#
- **Tools:** Visual Studio 2022, SQL Server Management Studio 21, GitHub

---

### 📸 Screenshots

#### 📱 Mobile Interface
*Note: The screenshots were taken during the development stage.
Some interface texts are in Turkish, and sensitive company details have been masked for confidentiality reasons.*

<table>
  <tr>
    <td><img src="screenshots/Search Page 1.png" alt="Mobil Görsel 1" width="200"/></td>
    <td><img src="screenshots/Search Page 2.png" alt="Mobil Görsel 2" width="200"/></td>
    <td><img src="screenshots/Search Page 3.png" alt="Mobil Görsel 3" width="200"/></td>
    <td><img src="screenshots/Work Order Detail Page.png" alt="Mobil Görsel 4" width="200"/></td>
  </tr>
  <tr>
    <td><img src="screenshots/Work Order Detail Page 2.png" alt="Mobil Görsel 5" width="200"/></td>
    <td><img src="screenshots/Add QC Page.png" alt="Mobil Görsel 6" width="200"/></td>
    <td><img src="screenshots/Add QC Page 2.png" alt="Mobil Görsel 7" width="200"/></td>
    <td><img src="screenshots/Add QC Page 3.png" alt="Mobil Görsel 8" width="200"/></td>
  </tr>
</table>

---

### 🚀 How to Run

1. **Clone the repository:**

```bash
git clone https://github.com/yourusername/qualityControlProject.git
```

2. **Setup the API:**
- Update appsettings.json with your server connection strings.
- The project is designed for two separate SQL Servers (Production & Quality).
- You can also test locally with dummy databases.

3. **Run the Mobile Application:**
- Open the project in Visual Studio.
- Build and deploy to an emulator or physical device.
- Update the API base URL in MauiProgram.cs or configuration files to point to your local API.

---

### 💡 Future Improvements

- User authentication and role-based access control
- Advanced reporting and analytics dashboards
- Cloud database synchronization
- Offline data entry with background sync

---

### 🧑‍💻 Author

Doğu Erbaş

📧 [doguerbass@gmail.com]  

💼 Developed as part of a professional internship project.

---

### 🇹🇷 Türkçe

# 🧾 Proje Özeti

**Trafo Kalite Kontrol Sistemi**, bir trafo üretim fabrikası için geliştirilmiş mobil ve web tabanlı bir uygulamadır.
Sistem, üretim verilerini fabrika sunucusundan çekerek kalite kontrol personelinin test sonuçlarını mobil uygulama üzerinden girmesine olanak tanır.
Doğrulama işlemi sonrasında hem ürün hem de test verileri başka bir sunucu üzerindeki farklı bir veritabanına kaydedilir.

---

### 🧩 Özellikler

- 🏭 **Üretim Verisi Çekme** — Fabrika sunucusundan otomatik olarak üretim kayıtlarını alır

- ✅ **Kalite Test Girişi** — Operatörler test sonuçlarını mobil arayüzden kolayca girebilir

- 🔄 **Çift Veritabanı Senkronizasyonu** — Ürün ve test verilerini ikinci bir sunucuya kaydeder

- 📱 **Mobil Arayüz (MAUI)** — Sahadaki kalite kontrol personeli için kullanıcı dostu tasarım

- 🌐 **API (ASP.NET Core)** — Üretim ve kalite veritabanları arasında veri alışverişini yönetir

- 💾 **Güvenli Veri Yönetimi** — SQL Server ile güvenilir veri saklama

- 🧱 **Temiz Mimari** — MVVM ve Repository tasarımıyla sürdürülebilir yapı

---

### 🏗️ Teknoloji Yığını

- **Frontend:** .NET MAUI
- **Backend:** ASP.NET Core Web API
- **Veritabanı:** Microsoft SQL Server
- **Mimari:** MVVM + Repository Pattern
- **Programlama Dili:** C#
- **Araçlar:** Visual Studio 2022, SQL Server Management Studio 21, GitHub

---

### 📸 Ekran Görüntüleri

#### 📱 Mobil Arayüz
*Not: Ekran görüntüleri geliştirme aşamasında alınmıştır.
Bazı arayüz metinleri Türkçe olarak kalmış, gizlilik nedeniyle firma ile ilgili hassas bilgiler kapatılmıştır.*

<table>
  <tr>
    <td><img src="screenshots/Search Page 1.png" alt="Mobil Görsel 1" width="200"/></td>
    <td><img src="screenshots/Search Page 2.png" alt="Mobil Görsel 2" width="200"/></td>
    <td><img src="screenshots/Search Page 3.png" alt="Mobil Görsel 3" width="200"/></td>
    <td><img src="screenshots/Work Order Detail Page.png" alt="Mobil Görsel 4" width="200"/></td>
  </tr>
  <tr>
    <td><img src="screenshots/Work Order Detail Page 2.png" alt="Mobil Görsel 5" width="200"/></td>
    <td><img src="screenshots/Add QC Page.png" alt="Mobil Görsel 6" width="200"/></td>
    <td><img src="screenshots/Add QC Page 2.png" alt="Mobil Görsel 7" width="200"/></td>
    <td><img src="screenshots/Add QC Page 3.png" alt="Mobil Görsel 8" width="200"/></td>
  </tr>
</table>

---

### 🚀 Çalıştırma Talimatları

1. **Depoyu Klonlayın:**

```bash
git clone https://github.com/yourusername/qualityControlProject.git
```

2. **API’yi yapılandırın:**
- appsettings.json dosyasına kendi sunucu bağlantı dizgilerinizi ekleyin.
- Proje iki ayrı SQL Server (Üretim ve Kalite) için tasarlanmıştır.
- İsterseniz yerel test için dummy veritabanları kullanabilirsiniz.

3. **Mobil Uygulamayı Çalıştırma:**
- Projeyi Visual Studio’da açın.
- Uygulamayı derleyin ve bir emülatörde veya gerçek cihazda başlatın.
- MauiProgram.cs veya konfigürasyon dosyalarındaki API adresini yerel API’ye yönlendirin.

---

### 💡 Gelecek İyileştirmeler

- Kimlik doğrulama ve rol tabanlı erişim
- Gelişmiş raporlama ve analiz panelleri
- Bulut veritabanı senkronizasyonu
- Çevrimdışı veri girişi ve arka plan senkronizasyonu

---
### 🧑‍💻 Yazar

Doğu Erbaş   

📧 [doguerbass@gmail.com]  

💼 Profesyonel staj projesi kapsamında geliştirilmiştir.
