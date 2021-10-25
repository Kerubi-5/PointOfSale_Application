CREATE TABLE [dbo].[staff_tbl]
(
	[staff_id] INT NOT NULL PRIMARY KEY,
	[staff_name] NVARCHAR(50),
	[phone] NCHAR(11),
	[username] NVARCHAR(50),
	[password] NVARCHAR(50)
)

CREATE TABLE [dbo].[product_tbl]
(
	[prod_id] INT NOT NULL PRIMARY KEY,
	[name] NVARCHAR(50),
	[brand] NVARCHAR(50),
	[msrp] DECIMAL(18,2),
)

CREATE TABLE [dbo].[product_status_tbl]
(
	[status_id] INT NOT NULL, PRIMARY KEY,
	[prod_id] INT NOT NULL,
	[qty] INT,
	[status] NVARCHAR(50)
)

CREATE TABLE [dbo].[product_report]
(
	[report_id] INT NOT NULL PRIMARY KEY,
	[staff_id] INT NOT NULL,
	[sales_type] NVARCHAR(50),
	[address] NVARCHAR(50),
	[paid_amount] DECIMAL(18,2),
	[date_issued] DATE
)

CREATE TABLE [dbo].[product_in_transaction]
(
	[report_id] INT NOT NULL,
	[prod_id] INT NOT NULL,
	[name] NVARCHAR(50),
	[qty_transacted] INT,
	[total_amount] DECIMAL(18,2)
)




