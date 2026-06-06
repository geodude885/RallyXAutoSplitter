# RallyXAutoSplitter

A [MelonLoader](https://github.com/LavaGang/MelonLoader) mod for RUMBLE that provides automatic splitting integration with [LiveSplit](https://livesplit.org/).

## Features

- **Automatic Timer Control**: Automatically starts, splits, and resets the LiveSplit timer based on race events
- **Track-based Splitting**: Splits at the start of tracks 1-4, plus a final split when track 4 completes
- **Smart Reset Logic**: Resets when returning to track selection or when a race ends without a winner
- **TCP Integration**: Communicates with LiveSplit via TCP server component

## Requirements

- RUMBLE (the game)
- [MelonLoader](https://github.com/LavaGang/MelonLoader) installed
- [LiveSplit](https://livesplit.org/) with the [LiveSplit Server](https://github.com/LiveSplit/LiveSplit.Server) component enabled

## Installation

1. Ensure MelonLoader is installed in your RUMBLE directory
2. Download the latest release of RallyXAutoSplitter
3. Place `RallyXAutoSplitter.dll` in your `RUMBLE/Mods` folder
4. Configure LiveSplit Server component to listen on `127.0.0.1:16834`

## LiveSplit Setup

1. Right-click on LiveSplit → Edit Layout
2. Add "LiveSplit Server" component (under Control)
3. Configure server settings:
   - Host: `127.0.0.1`
   - Port: `16834`
4. Create 5 splits for your run (tracks 1-4 start + track 4 end)

## How It Works

The mod hooks into RUMBLE's race system and:
- **Starts** the timer when track 0 begins (no split)
- **Splits** when tracks 1, 2, 3, and 4 start
- **Splits** again when track 4 completes (final split)
- **Resets** when a race ends without completion or when returning to track selection

## Building

### Prerequisites
- .NET 6 SDK
- Visual Studio 2022 or later (optional)

### Build Steps

```bash
dotnet build RallyXAutoSplitter/RallyXAutoSplitter.csproj -c Release
```

The compiled DLL will be in `RallyXAutoSplitter/bin/Release/net6.0/`

## Development

The project includes a comprehensive unit test suite:

```bash
dotnet test RallyXAutoSplitter.Tests/RallyXAutoSplitter.Tests.csproj
```

### Architecture

- **Interfaces**: Core logic depends on abstractions (`ILiveSplitService`, `ILogger`, `IRaceData`, `ITcpClientFactory`)
- **Adapters**: MelonLoader and Il2Cpp types are isolated behind adapters for testability
- **Services**: 
  - `AutoSplitterService`: Contains the splitting logic
  - `LiveSplitService`: Handles TCP communication with LiveSplit

## License

This project is open source. Feel free to modify and distribute.

## Credits

Created by ACutiePi
