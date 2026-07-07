# ProductCatalog.Api

## Clone repository
1. repository : https://github.com/LahiruMadushanBandara/ProductCatalog.Api.git
2. Checkout master or dev Branch.

## How to build and run


### Step 1 - Create the database

I have put the database script "Script_ProductCatalogDb.sql" under DB folder. Execute it in your SQL server OR use following command.

sqlcmd -S localhost -E -i DB\Script_ProductCatalogDb.sql


Running the script twice is fine, it only creates things if they
are not there already.

### Step 2 - Connection string

The app reads the connection string from appsettings.json. By default
it points to a local SQL Server with Windows login. If your setup is
different, either edit that file or set it from the command line:


Command :
set ConnectionStrings__Default=Server=localhost;Database=ProductCatalogDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;


### Step 3 - Build

Command :
dotnet build

### Step 4 - Run

Click run btn on Visual studio OR Use following command
Command :
dotnet run --project ProductCatalog.Api


The console shows the URL. Open it in a browser and add /swagger at
the end.
