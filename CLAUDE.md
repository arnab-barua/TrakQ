# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TrakQ is a cross-platform personal finance tracking app built with **.NET 9 MAUI**, targeting Android (`net9.0-android`) and Windows (`net9.0-windows10.0.19041.0`). App ID: `com.parametrix.trakq`.

## Build Commands

```bash
# Build the solution
dotnet build TrakQ.sln

# Build and publish for Android
dotnet publish -f net9.0-android -c Release

# Build and publish for Windows
dotnet publish -f net9.0-windows10.0.19041.0 -c Release

# Add a new EF Core migration (run from TrakQ.Db project)
dotnet ef migrations add <MigrationName> --project TrakQ.Db --startup-project TrakQ

# Apply migrations manually
dotnet ef database update --project TrakQ.Db --startup-project TrakQ
```

There is no automated test suite in this project.

## Architecture

Two projects in the solution:

- **TrakQ/** — Main MAUI application (Views, ViewModels, Services, DTOs)
- **TrakQ.Db/** — Database layer (EF Core DbContext, Entities, Migrations)

### Layered Architecture

```
View (XAML) → ViewModel → Service → AppDbContext (EF Core) → SQLite
```

- **Views** in `TrakQ/View/` bind directly to ViewModels via MVVM
- **ViewModels** in `TrakQ/ViewModel/` hold UI state and call services
- **Services** in `TrakQ/Service/` contain all business logic; interact directly with EF Core
- **Entities** in `TrakQ.Db/Data/Entities/` define the database schema
- **DTOs** in `TrakQ/Dto/` are used for passing data between service and ViewModel layers

### Key Data Model

```
FiscalMonth (Year + Month reference)
  └── AccountSheet (per-account monthly opening/closing balance)
        └── Account

ExpenditureHead (expense categories, hierarchical via ParentHeadId)
  └── Expenditure (individual expense records, soft-delete)

IncomeHead (income categories)
  └── Income (individual income records, soft-delete)
```

- All currency fields use `[Precision(10,2)]`
- `Expenditure` and `Income` have `IsDeleted` for soft deletes
- DB file path: `%APPDATA%\trakq\TrakQ.db3` (Windows); configured in `TrakQ.Db/Constants.cs`
- Migrations run automatically at startup via `App.xaml.cs`

### Dependency Injection

All registrations are Singleton. Registration helpers:
- `TrakQ/View/InjectViews.cs` — registers all Views
- `TrakQ/ViewModel/InjectViewModels.cs` — registers all ViewModels
- `TrakQ/Service/InjectDomainServices.cs` — registers all Services
- `MauiProgram.cs` calls these three extension methods

### Navigation

Shell-based navigation defined in `AppShell.xaml`. Pages are navigated via `Shell.Current.GoToAsync()` with route names. Parameters are passed as route dictionaries.

### MVVM Conventions

Uses **CommunityToolkit.Mvvm** source generators:
- `[ObservableProperty]` on private fields → generates public property + `INotifyPropertyChanged`
- `[RelayCommand]` on `async Task` methods → generates `IAsyncRelayCommand` property
- ViewModels inherit from `BaseViewModel` (has `IsBusy`, `Title`)

### UI Framework

- **UraniumUI Material** for Material Design components
- **FontAwesome 6** icons (via UraniumUI.Icons.FontAwesome)
- Lists rendered with `CollectionView`; month/year selection with `Picker`

### Import/Export

`ImportExportService` handles:
- **Export**: copies `TrakQ.db3` to the Downloads folder after WAL checkpoint
- **Import**: executes raw SQL via `ExecuteSqlAsync` to restore data
