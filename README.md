# Meeting Info Database

Meeting Info Database is a class library project, which provides API for connecting to Microsoft SQL Server, precisely the meeting database (*MeetingDatabase*). *MeetingDatabase* is the database consisting of two tables: *Meeting* and *Client* table. This project provides an API for several other projects, specifically:

- [MeetingInfoWinForms](https://github.com/SanjinKurelic/MeetingInfoWinForms)
- [MeetingInfoWPF](https://github.com/SanjinKurelic/MeetingInfoWPF)
- MeetingInfoWebForms
- MeetingInfoMVC

## Getting started

If you are not interested in building this project yourself, you can use prebuilt NuPackage in **packed** direcotry.

### Prerequisites

For running the project you need to have the following items:

- Visual Studio 2017 or newer
- SQL Server 2017 or newer
- .NET Framework 4.6.1 or newer
- NuGet CLI (for building the project)

**Notice:** Project might run on the older .NET Framework or using older Visual Studio/SQL Server version, with or without additional tweaks.

### Installing

Install Visual Studio (include .NET Framework) and NuGet CLI with default options. While installing SQL Server create **sa** user with the following values:

database username: **sa**<br>
database password: **SQL**

**Note:** It is discouraged to use **sa** account for application-database connection, so it is preferable to create the new user for this project. If you create different user, you need to **change connection strings** in the projects mentioned above and in *MeetingInfoDatabaseTest* project:

 ```
 MeetingInfoDatabase > MeetingInfoDatabaseTest > TestConfiguration [class] > ConnectionString [field]
 ```

Clone this solution and open it with Visual Studio. This project uses several frameworks which are available trough NuGet package manager. Using a built-in NuGet download and install these packages:

- Microsoft.ApplicationBlocks.Data version 2.0 or newer
- EnterpriseLibrary.Common version 6.0 or newer
- EnterpriseLibrary.Data 6.0 or newer

Visual Stduio may install those packages upon opening the project, so check their existence before proceeding with instalation.

### Running

This project is a class library project so you can't directly run it without one of the project mentioned above. You could only run Test classes provided by this repository.

### Building project

Build project by running it as a release from Visual Studio, which will generate dll-s in bin/Release directory of the project. You could copy them to other project and use them accordingly, but with that approach you loose flexibility of upgrading the referenced NuGet packages. Better approach is to build those dll-s as NuGet package with the following command:

```
nuget.exe pack MeetingInfoDatabase.csproj -Prop Configuration=Release
```

**Use generated MeetingInfoDatabase.1.0.0.nupkg file as a reference in other projects with the following command:**

```
Install-Package MeetingInfoDatabase -Source <path_to_package>
```

## Project structure

The project uses several technologies for connecting to the database:

- ADO .NET connected enviroment
- ADO .NET disconnected enviroment
- Enterprise DAAB
- SqlHelper class

**Note**: The project doesn't have an example of using Entity Framework, because Entity Framework is less coding, and more choosing required UI options - so it's not programming challenge ;)

**Repository** class is a main project endpoint. Repository constructor accepts two parameters: *connectrionString* for connecting to the database and *databaseType* which define the technology used for fetching the data. *DatabaseType* has the following values:

- Ado (connected enviroment)
- DataSet (disconnected enviroment)
- EnterpriseDAAB
- SqlHelper

## Licence

See the LICENSE file. For every question write to kurelic@sanjin.eu;