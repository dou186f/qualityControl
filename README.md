# Transformer Quality Control System

A .NET MAUI mobile application and ASP.NET Core API developed for quality control personnel at a transformer manufacturing facility. Personnel view production work orders, load the tests assigned to each product's quality control set, and save suitable/not suitable results. There is no separate web user interface; Swagger provides API documentation in Development.

## Features

- **Production Data Fetching** — Reads work orders, products, production quantities and test definitions when the app requests them
- **QC Test Entry** — Operators select suitable/not suitable results; the current interface does not accept numeric measurements, even though test definitions expose minimum and maximum values
- **Dual Database Sync** — Reads production data through the Read connection and writes quality control results through the separate Write connection. It does not copy the complete product records or synchronize the two databases
- **Mobile Interface (MAUI)** — Designed for on-site quality control personnel
- **RESTful API** — ASP.NET Core middleware bridging the production and quality databases
- **API–MOBILE–SHARED Structure** — API controllers and SQL repositories, MAUI pages with view models and HTTP services, and shared models, DTOs and repository interfaces

Data loads on navigation, search or manual refresh. Automatic background synchronization and live updates are not implemented. Selected results are submitted in one request and written in one transaction: a failed write rolls back the entire batch. The existing single-result endpoint remains available, and both endpoints validate the work order, related product and test set membership.

Completion means actual quantity is at least the planned quantity, with a positive planned quantity and a known actual quantity. Missing quantities or a zero/negative planned quantity display `N/A` and fall under unfinished orders. Finished and unfinished filters are mutually exclusive; the API rejects requests that enable both.

## Tech Stack

| Layer | Technology |
|---|---|
| Mobile Frontend | .NET MAUI |
| Backend | ASP.NET Core Web API |
| Shared Layer | Class library (common models & DTOs) |
| Data Access | Microsoft.Data.SqlClient (raw SQL) |
| Database | Microsoft SQL Server (×2) |
| Project Structure | API controllers/repositories, MOBILE pages/view models/services, SHARED contracts |
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
git clone https://github.com/dou186f/qualityControl.git
```

### SDK and mobile workloads

The API and shared library target .NET 8 (`net8.0`). The mobile project targets .NET 8 for Android, iOS, Mac Catalyst and, on Windows, Windows. Install the .NET 8 SDK and Visual Studio 2022 with .NET MAUI development support and the workloads for your target platform. The API alone does not require MAUI workloads. Android builds also require the Android SDK/JDK provided by the development tools; iOS and Mac Catalyst require a Mac with compatible Xcode and Apple tooling.

Open `qualityControl.sln`. With the .NET 8 SDK selected, the MAUI workloads can be restored using:

```powershell
dotnet workload restore qualityControl.MOBILE/qualityControl.MOBILE.csproj
```

### Database connections

Configure both `ConnectionStrings:Read` and `ConnectionStrings:Write` in `qualityControl.API/appsettings.Development.json`, or use environment variables in the shell that launches the API:

```powershell
$env:ConnectionStrings__Read = "Server=<production-server>;Database=<production-database>;Integrated Security=True;Encrypt=True"
$env:ConnectionStrings__Write = "Server=<results-server>;Database=<results-database>;Integrated Security=True;Encrypt=True"
```

Replace the placeholders and choose credentials and certificate trust settings appropriate to your SQL Server installation. The checked-in strings are placeholders. Read needs access to the production tables; Write needs read/insert/update access to the results table and permission to use transactions. Always specify Write explicitly: the current connection factory falls back to Read if Write is absent.

### Start the API

```powershell
dotnet run --project qualityControl.API/qualityControl.API.csproj --launch-profile https
```

The `https` profile in `qualityControl.API/Properties/launchSettings.json` listens on `https://localhost:7110` and `http://localhost:5177`. Swagger is available at `https://localhost:7110/swagger` in Development. The `http` profile uses `http://localhost:5177`; IIS Express is configured for HTTP port `7349` and HTTPS port `44340`. The API also uses HTTPS redirection.

For local HTTPS development, configure and trust the ASP.NET Core development certificate on the host, for example with `dotnet dev-certs https --trust`. Trust on the host does not automatically establish trust on an emulator or physical device.

### Connect and launch the mobile app

Select the required mobile target and emulator/device in Visual Studio. HTTP clients are configured in `qualityControl.MOBILE/MauiProgram.cs`, not in a separate mobile configuration file:

| Build/environment | Current base address | Meaning |
|---|---|---|
| Android Debug and Android Release | `https://10.0.2.2:1234` | Host computer through the standard Android emulator's special host address |
| Non-Android, including local Windows desktop | `https://localhost:1234` | The machine/device running the app |

These mobile addresses use port `1234`, which differs from all checked-in API launch profile ports. They are retained as configured. Before running locally, match the scheme and port at both ends: either edit the appropriate `baseUri` branch in `MauiProgram.cs` to the API's selected port (for example Android emulator `https://10.0.2.2:7110`, or local desktop `https://localhost:7110`), or configure the API launch profile/binding to serve HTTPS on port `1234`. Restart the API and rebuild the mobile app after changing their configuration.

- **Android emulator:** `10.0.2.2` refers to the host of the standard Android emulator. Ensure the API is running on the matching port and reachable from the emulator. Other emulator products can use different host mappings. A certificate issued only for `localhost` can also fail hostname validation for `10.0.2.2`.
- **Local desktop:** `localhost` works when the API runs on that same computer. Match the selected API port and trust its HTTPS certificate for that user/machine. On a separate desktop, use the API host's reachable network name or IP instead.
- **Physical device:** neither `localhost` nor the emulator's `10.0.2.2` points to your development computer. Set the relevant platform's `baseUri` to the API host's LAN name/IP and matching port. Use the same reachable network or a suitable routed connection; check Wi-Fi client isolation. Configure the API to bind to that network interface rather than only loopback, using its launch profile or ASP.NET Core/Kestrel endpoint configuration. Allow inbound access to the chosen port in the host firewall and configure the HTTPS endpoint with a certificate valid for the host name/IP that the device trusts. An HTTPS URL binding alone does not supply a suitable certificate. For iOS/Mac Catalyst, `localhost` also means the machine/device running that app, so use a reachable API host when it is elsewhere.

**Existing certificate behavior:** Android Debug disables server certificate validation in `MauiProgram.cs` by returning `true` from `ServerCertificateCustomValidationCallback`. Android Release and non-Android builds use normal certificate validation. This behavior is unchanged; Debug bypasses certificate checks but still requires network access, a listening API endpoint and a working TLS connection. Configure valid, trusted certificates for builds that validate certificates.

### Required SQL objects

The repository does not include production SQL setup scripts, migrations or sample data. The application does not create the required tables. An empty database alone is insufficient. The following objects and columns are inferred from the existing queries and must be supplied for your environment:

| Connection | Table | Referenced columns |
|---|---|---|
| Read | `LG_112_DISPLINE` | `LOGICALREF`, `LINENO_`, `OPBEGDATE`, `PRODORDREF`, `ITEMREF` |
| Read | `LG_112_ITEMS` | `LOGICALREF`, `CODE`, `NAME`, `QCCSETREF` |
| Read | `LG_112_PRODORD` | `LOGICALREF`, `PLNAMOUNT`, `ACTAMOUNT` |
| Read | `LG_112_QCSLINE` | `LOGICALREF`, `NAME`, `SETREF`, `MINVAL`, `MAXVAL` |
| Write | `dbo.QualityControlResult` | `LogicalRef`, `WorkOrderRef`, `QcRef`, `SetRef`, `Name`, `Result`, `CreatedAt` |

Work orders link to products through `ITEMREF` and production orders through `PRODORDREF`. A product's `QCCSETREF` identifies the test definitions with that `SETREF`. Results are matched by `(WorkOrderRef, QcRef)` and should have one row per pair. Inserts omit `LogicalRef`, so the results database must generate it. `CreatedAt` is set by `SYSUTCDATETIME()` on insert and preserved on update; `Result` represents a Boolean outcome.

Exact SQL types, string lengths, identity/default definitions, constraints, indexes and source-table nullability are not supplied by a schema in this repository; verify them against the actual databases. In particular, the old `GetDouble`/`GetFloat` difference does not establish whether quantity columns are SQL `float`, `real` or another numeric type. Quantity reads now check `DBNull` and convert the provider's numeric value to the nullable `double` used by the models. Missing joined product/production rows remain readable as null display fields and quantities. No additional database columns are required by these changes.

## Author

**Doğu Erbaş**  
doguerbass@gmail.com  
Developed as a professional internship project.
