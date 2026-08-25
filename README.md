# Pilot Kneeboard (WinUI 3)

A simple digital kneeboard for flight simmers. One card holds:

- **Callsign**
- **Departure / Arrival airport (ICAO)**
- **SID** — departure procedure
- **STAR** — arrival procedure
- **Squawk code** — validated as 4 octal digits (0–7)
- **SELCAL code** — validated as 4 letters (e.g. `AB-CD`)
- A free-form **notes** pad for ATIS, clearance, runway, frequencies, etc.

Cards can be saved to and loaded from a `.json` file, so you can prep a card
before you fly and reload it mid-session if the app restarts.

## Requirements

This is a native Windows app — it must be built and run on Windows, not in
this chat/sandbox.

- Windows 10 (1809+) or Windows 11
- Visual Studio 2022 (17.9+) with the **".NET Desktop Development"** and
  **"Windows App SDK C# Templates"** workloads
  (installable via Visual Studio Installer → Individual Components →
  search "Windows App SDK")
- .NET 8 SDK

## Running it

1. Open `KneeboardApp.csproj` in Visual Studio (File → Open → Project/Solution).
2. Set the target platform (x64 is easiest) in the toolbar.
3. Press **F5** to build and run.

Or from the command line, on Windows with the .NET 8 SDK installed:

```powershell
dotnet build -r win-x64
dotnet run -r win-x64
```

## Project layout

```
KneeboardApp/
├── KneeboardApp.csproj      Project file (WinUI 3 / Windows App SDK, unpackaged)
├── app.manifest              Standard Windows app manifest (DPI awareness etc.)
├── App.xaml / App.xaml.cs    App startup, creates MainWindow
├── MainWindow.xaml           The kneeboard UI layout
├── MainWindow.xaml.cs        Save/Load logic + squawk/SELCAL validation
└── Models/
    └── FlightData.cs         Plain data model for one card
```

## Extending it

Ideas if you want to build on this:

- Add a `ListView` of saved cards for quick switching between legs.
- Add a cruise altitude / route field.
- Pin the window "always on top" via `AppWindow.Presenter` so it stays
  visible over your sim.
- Add a second page/tab for airport charts (image viewer) alongside the
  data fields.
