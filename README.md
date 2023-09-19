# GT4 Progress Tracker for Twitch

This is a launcher for PCSX2 on Windows which allows you to track progress made in Gran Turismo 4 and automatically update the title of a Twitch stream via its API.

The update happens each time the game saves its game.

## Requirements

- .NET 6.0 Desktop Runtime - [from here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) look for .NET Desktop Runtime, under installers pick x64 (alternatively, [click here](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.22-windows-x64-installer) to get 6.0.22)

## Usage

1. Launch GT4Twitch.exe

2. Browse for PCSX2's executable

3. Browse for your memory card which has the save data

4. Set up the title format

5. Log in to Twitch (you only need this once in a while)

6. Launch PCSX2

7. Save the game and observe the title. It should update accordingly.

## Compatibility

Currently it is only compatible with:

- SCUS-97328 (NTSC-U)

- SCUS-97436 (NTSC-U Online Beta)

## Security concerns

Please keep in mind that the access token is saved directly in the `GT4Twitch.dll.config` file of this application in plaintext!

Do **NOT** share that file with anyone whom you do not trust.

If you're sharing this app, always point them to this repository's releases page! Don't just pack up your tool and send it out!

This tool hadn't been written yet with high security in mind, so please be mindful of this while you're using the app!

## Building

The build process isn't fully automated yet.

1. Build GT4Twitch

2. Copy `Resources` and `myMC` to the output folder (`bin/(Release/Debug)/net6.0-windows`)

## NuGet packages

- Microsoft.Web.WebView2 & WebView2.Runtime.X64 - for the auth flow

- Newtonsoft.Json - for Twitch API

- System.Data.SQLite - for GT4SaveEditor

## TODOs

- Error handling

- Periodic validation and re-login

- Enhance the security

- Compatibility with other GT4 versions

- Shrink the size - WebView2 is HUGE and has binaries for other platforms which we don't need!

- Housekeeping around the code - this tool was written with functionality first and not much in the way of cleanliness.

## Credits

[Nenkai](https://github.com/Nenkai) for PDTools and GT4SaveEditor


