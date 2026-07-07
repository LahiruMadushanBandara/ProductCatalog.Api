IF DB_ID('ProductCatalogDb') IS NULL
    CREATE DATABASE ProductCatalogDb;
GO
USE ProductCatalogDb;
GO
IF OBJECT_ID('dbo.Products') IS NULL
CREATE TABLE dbo.Products (
    Id              INT            NOT NULL PRIMARY KEY,
    Title           NVARCHAR(300)  NOT NULL,
    Category        NVARCHAR(100)  NULL,
    Brand           NVARCHAR(150)  NULL,
    Price           DECIMAL(10,2)  NOT NULL,
    Stock           INT            NOT NULL,
);
GO