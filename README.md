# Transformer Quality Control System

A mobile and web-based quality control application developed for a transformer manufacturing facility. The system pulls production records from the factory server, allows QC staff to submit test results via a mobile interface, and persists all data to a separate reporting database.

## Features

- **Production Data Fetching** — Automatically retrieves work orders from the factory server
- **QC Test Entry** — Operators submit test results directly from the mobile app
- **Dual Database Sync** — Product and test data are written to a secondary server for traceability and reporting
- **Mobile Interface (MAUI)** — Designed for on-site quality control personnel
- **RESTful API** — ASP.NET Core middleware bridging the production and quality databases
- **Clean Architecture** — MVVM + Repository pattern for long-term maintainability

## Tech Stack

| Layer | Technology |
|---|---|
| Mobile Frontend | .NET MAUI |
| Backend | ASP.NET Core Web API |
| Database | Microsoft SQL Server (×2) |
| Architecture | MVVM + Repository Pattern |
| Language | C# |
| Tools | Visual Studio 2022, SSMS 21, GitHub |

## Screenshots

> Screenshots were taken during active development. Some UI text remains in Turkish, and confidential company information has been redacted.

<table>
  <tr>
    <td><img src="screenshots/Search Page 1.png" width="200"/></td>
    <td><img src="screenshots/Search Page 2.png" width="200"/></td>
    <td><img src="screenshots/Search Page 3.png" width="200"/></td>
    <td><img src="screenshots/Work Order Detail Page.png" width="200"/></td>
  </tr>
  <tr>
    <td><img src="screenshots/Work Order Detail Page 2.png" width="200"/></td>
    <td><img src="screenshots/Add QC Page.png" width="200"/></td>
    <td><img src="screenshots/Add QC Page 2.png" width="200"/></td>
    <td><img src="screenshots/Add QC Page 3.png" width="200"/></td>
  </tr>
</table>

## Getting Started

```bash
git clone https://github.com/yourusername/qualityControlProject.git
```

1. Open the solution in Visual Studio 2022.
2. Update `appsettings.json` with your connection strings for both SQL Server instances (Production and Quality).
3. Configure the API base URL in `MauiProgram.cs` or the relevant config file.
4. Build and deploy the mobile app to an emulator or physical device.

> **Note:** The project expects two separate SQL Server databases. Local dummy databases can be used for development and testing.

## Roadmap

- [ ] Authentication with role-based access control
- [ ] Advanced reporting and analytics dashboard
- [ ] Cloud database synchronization
- [ ] Offline data entry with background sync

## Author

**Doğu Erbaş**  
doguerbass@gmail.com  
Developed as a professional internship project.
