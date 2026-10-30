# CD Tray

A tiny Windows utility that opens or closes the CD/DVD/BD tray from the **Run** dialog (`Win+R`) or a command line. Handy when the physical eject button is broken.

It has no visible window and exits as soon as the tray command is sent.

## Usage

```
CD_Tray [command] [drive:]
```

| Command   | Effect                                              |
|-----------|-----------------------------------------------------|
| *(none)*  | Open the tray, wait 1 second, then close it         |
| `<n>`     | Open the tray, wait *n* seconds, then close it      |
| `o`       | Open the tray                                       |
| `c`       | Close the tray                                      |
| `-h`, `/?`| Show help                                           |

An optional drive letter (`E:` or `E`) selects a specific drive; otherwise the first optical drive is used.

### Examples

```
CD_Tray          :: open, then close after 1 second
CD_Tray 5        :: open, close after 5 seconds
CD_Tray o        :: open
CD_Tray c E:     :: close the tray of drive E:
```

### Run from `Win+R`

Copy `CD_Tray.exe` to a folder on your `PATH` (for example `C:\Windows`, or add your own folder to `PATH`) and type `cd_tray o` in the Run dialog. To get a shorter command, rename the file (e.g. `cd.exe`).

Exit codes: `0` success, `1` bad arguments, `2` no optical drive, `3` drive/MCI error.

## Build

Requires the .NET SDK and the .NET Framework 4.8 targeting pack (included with Visual Studio / Windows).

```
dotnet build CD_Tray.sln -c Release
```

The executable is written to `src/CD_Tray/bin/Release/net48/CD_Tray.exe`.

## How it works

The tray is controlled through the Windows Multimedia (MCI) API in `winmm.dll`: the drive is opened as a `CDAudio` device and sent `set <alias> door open` / `door closed`.
