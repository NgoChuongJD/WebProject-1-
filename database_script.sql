-- =============================================
-- Create database template
-- =============================================
USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'WebDienThoai'
)
DROP DATABASE WebDienThoai
GO

CREATE DATABASE WebDienThoai
GO

USE WebDienThoai
GO

CREATE TABLE Supplier
(
	SupplierId			INT PRIMARY KEY IDENTITY,
	SupplierName		NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Category
(
	CateId				INT PRIMARY KEY IDENTITY,
	CateName			NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Product
(
	ProductId			INT PRIMARY KEY IDENTITY,
	ProductName			NVARCHAR(50),
	SupplierId			INT NOT NULL REFERENCES Supplier(SupplierId),
	CateId				INT NOT NULL REFERENCES Category(CateId),
	ProductImage		NVARCHAR(250),
	Price				MONEY,
	Quantity			INT,
	TechnicalInfo		NTEXT,
	InformationDetail	NTEXT,
	[Status]			BIT,
	DateCreated			DATETIME,
	DateModified		DATETIME
	
)



CREATE TABLE Customer
(
	CustId				INT PRIMARY KEY IDENTITY,
	CustomerName		NVARCHAR(50),
	Address				NTEXT,
	Email				VARCHAR(250) NOT NULL,
	Password			VARCHAR(50),
	PhoneNumber			VARCHAR(250),
	Gender				BIT,
	[Status]			BIT
)

GO
ALTER TABLE Customer ADD CONSTRAINT uq1 UNIQUE (Email)
GO

--Tao bang don hang
CREATE TABLE [Order]
(
	OrderId				INT PRIMARY KEY IDENTITY,
	OrderDate			DATETIME,
	CustId				INT NOT NULL REFERENCES Customer(CustId),
	[Status]			INT,
	ShipAddress			NTEXT,
	ShipPhone			VARCHAR(250)
)

CREATE TABLE OrderDetail
(
	OrderId				INT NOT NULL REFERENCES [Order](OrderId),
	ProductId			INT NOT NULL REFERENCES Product(ProductId)
	PRIMARY KEY (OrderId,ProductId),
	Price				MONEY,
	Quantity			INT
)

CREATE TABLE tblUsers
(
	UserName VARCHAR(50) PRIMARY KEY,
	Password VARCHAR(50) NOT NULL,
	FullName NVARCHAR(250)
)

INSERT tblUsers VALUES ('admin','12345678','administrator')

SELECT * FROM tblUsers