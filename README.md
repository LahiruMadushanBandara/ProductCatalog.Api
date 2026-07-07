# ProductCatalog.Api

## How to build and run

Run these from the repo root folder.

### Step 1 - Create the database

I have put the database script "Script_ProductCatalogDb.sql". Execute it in your SQL server.

```
sqlcmd -S localhost -E -i db\schema.sql
```

If you are on SQL Express use `-S .\SQLEXPRESS` instead.
If your server needs a username and password:

```
sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -i db\schema.sql
```

No SQL Server installed? You can start one with docker:

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Running the script twice is fine, it only creates things if they
are not there already.

### Step 2 - Connection string

The app reads the connection string from appsettings.json. By default
it points to a local SQL Server with Windows login. If your setup is
different, either edit that file or set it from the command line:

```
set ConnectionStrings__Default=Server=localhost;Database=ProductCatalogDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
```

(on Linux/Mac use export instead of set)

### Step 3 - Build

```
dotnet build
```

### Step 4 - Run

```
dotnet run --project ProductCatalog.Api
```

The console shows the URL. Open it in a browser and add /swagger at
the end, for example http://localhost:5009/swagger. From there you can
try the endpoints.

If you want to make a release build and run that instead:

```
dotnet publish ProductCatalog.Api -c Release -o publish
dotnet publish\ProductCatalog.ApiJson.dll
```