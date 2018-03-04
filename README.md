# exin server

The _exin_ project aims to make monitoring incomes and expenses easy and intuitive. This is it's backend component.

## How does it work?

The server stores the data in an SQLite database and allows access to it and managing it through an HTTP REST API.

## Building

Install .NET Core if you haven't got it already. Then execute `dotnet build` from the `src` directory.

You can also run the application directly by issuing the following commands.

    dotnet restore
    dotnet run --project ExinServer.Web/ExinServer.Web.csproj

## Installation and usage

After building the solution, copy the DLL files from the `build/bin/netcoreapp2.0` directory together with `src/ExinServer.Web/appsettings.json` to the location of your choice and execute the application using the command below. (You may want to configure the path of the database file in `appsettings.json` first.)

    dotnet ExinServer.Web.dll

You can also use _Visual Studio Code_ to debug the application.

## Development Environment

  * Ubuntu 17.10
  * .NET Core 2.1.4
  * Visual Studio Code 1.19.3
    * Extension: C# 1.14.0
