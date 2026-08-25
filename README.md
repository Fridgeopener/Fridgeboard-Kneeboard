# Fridgeboard (Pilot Kneeboard)

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

## Credit(s)

This code is mostly written using Claude AI. 
