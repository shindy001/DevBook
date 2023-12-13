# DevBook

DevBook is a simple application for app and group of apps management (e.g. apps you need to run before you can start to work like VS, VSCode, Browser, TimeManagementApp, Switching between projects, etc.). You can setup common apps, group them to a profile and then start them together. Starting apps via shell name like "notepad" or "spotify" is also supported.

Purpose of this repo is also to brush up on my rusty skills and to try new languages/technologies/approaches like Blazor, Vertical slices, dart, dart bloc, grpc-dotnet, etc.

![DevBook1](https://github.com/shindy001/DevBook/assets/23438364/a67afc1b-7c9b-4c37-9a07-d1c0f77684a1)
![DevBook2](https://github.com/shindy001/DevBook/assets/23438364/144b4faf-b281-4d43-9aa6-9b2203988470)
![DevBook3](https://github.com/shindy001/DevBook/assets/23438364/f5e2d815-c491-4440-ad6c-8fd21d4ea451)

## Current state of Features (more to come)
- [x] App management - name, path, args
- [x] Startup Profile management - name, apps to start
- [x] Hacker News integration - lists 10 newest hacker news in a side drawer for quick access
- [x] Server Integration Tests
- [ ] Time/Task/Work management
- [ ] Client Integration Tests
- [ ] Work statistics, work evidence export - for week/month
- [ ] Spotify api integration ?
- [ ] Weather api integration ? 

## Dev Requirements
- `Visual Studio` or `VSCode with C# Dev Kit, .NET MAUI extensions`
- .net 8
- MAUI Blazor Hybrid app Prerequisites - https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/maui?view=aspnetcore-8.0

## Techstack
- Client - Maui Blazor Hybrid (Windows only), MudBlazor, tailwind (cli win-x64 in repo), GRPC, .net8
- Server - ASP.NET Core, GRPC, EFCore, SQLite, .net8

## How to run
1. Open DevBook.sln
1. Select DevBook.Server configuration and start "https" configuration without debugging (VS will probably prompts for approve of untrusted https or to create a dev cert)
1. Select MauiBlazorClient configuration and start "Windows Machine" configuration (if run as debug, this will also start tailwind watch in terminal to hot reload tailwind css)
