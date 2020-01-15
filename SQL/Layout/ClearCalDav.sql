/*
DROP TABLE ASPIdentity.__EFMigrationsHistory;
DROP TABLE ASPIdentity.AspNetRoleClaims;
DROP TABLE ASPIdentity.AspNetUserClaims;
DROP TABLE ASPIdentity.AspNetUserLogins;
DROP TABLE ASPIdentity.AspNetUserTokens;
DROP TABLE ASPIdentity.AspNetUserRoles;
DROP TABLE ASPIdentity.AspNetRoles;
DROP TABLE ASPIdentity.AspNetUsers;
*/
/* The first part of the script wipes the database clean of any tables... 
   it's huge because it needs to delete stored procedures, foreign key relationships..
*/

/* Drop all non-system stored procs */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'

SELECT @name = (SELECT TOP 1 sp.name FROM sys.procedures as sp INNER JOIN sys.schemas as ss ON sp.schema_id = ss.schema_id WHERE sp.[type] = 'P' AND ss.name = @SCHEMA ORDER BY sp.[name]);

WHILE @name is not null
BEGIN
    SELECT @SQL = 'DROP PROCEDURE [' + @SCHEMA + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Procedure: ' + @name
    SELECT @name = (SELECT TOP 1 sp.name FROM sys.procedures as sp INNER JOIN sys.schemas as ss ON sp.schema_id = ss.schema_id WHERE sp.[type] = 'P' AND ss.name = @SCHEMA ORDER BY sp.[name]);
END
GO

DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'

SELECT @name = (SELECT TOP 1 sp.name FROM sys.sequences as sp INNER JOIN sys.schemas as ss ON sp.schema_id = ss.schema_id WHERE sp.[type] = 'SO' AND ss.name = @SCHEMA ORDER BY sp.[name]);

WHILE @name is not null
BEGIN
    SELECT @SQL = 'DROP SEQUENCE [' + @SCHEMA + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Sequence: ' + @name
    SELECT @name = (SELECT TOP 1 sp.name FROM sys.sequences as sp INNER JOIN sys.schemas as ss ON sp.schema_id = ss.schema_id WHERE sp.[type] = 'SO' AND ss.name = @SCHEMA ORDER BY sp.[name]);
END
GO

/* Drop all Foreign Key constraints */
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'

SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_SCHEMA = @SCHEMA ORDER BY TABLE_NAME)

WHILE @name is not null
BEGIN
    SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name AND TABLE_SCHEMA = @SCHEMA ORDER BY CONSTRAINT_NAME)
    WHILE @constraint IS NOT NULL
    BEGIN
        SELECT @SQL = 'ALTER TABLE [' + @SCHEMA + '].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
        EXEC (@SQL)
        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name AND TABLE_SCHEMA = @SCHEMA ORDER BY CONSTRAINT_NAME)
    END
SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_SCHEMA = @SCHEMA ORDER BY TABLE_NAME)
END
GO

/* Drop all Primary Key constraints */
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'

SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_SCHEMA = @SCHEMA ORDER BY TABLE_NAME)

WHILE @name IS NOT NULL
BEGIN
    SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = @name AND TABLE_SCHEMA = @SCHEMA ORDER BY CONSTRAINT_NAME)
    WHILE @constraint is not null
    BEGIN
        SELECT @SQL = 'ALTER TABLE [' + @SCHEMA + '].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint)+']'
        EXEC (@SQL)
        PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name AND TABLE_SCHEMA = @SCHEMA ORDER BY CONSTRAINT_NAME)
    END
SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_SCHEMA = @SCHEMA ORDER BY TABLE_NAME)
END
GO

/* Drop all Views */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'



SELECT @name = (SELECT TOP 1 sv.name FROM sys.views as sv INNER JOIN sys.schemas as ss ON sv.schema_id = ss.schema_id WHERE sv.[type] = 'V' AND ss.name = @SCHEMA ORDER BY sv.[name]);
WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP VIEW [' + @SCHEMA + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
    SELECT @name = (SELECT TOP 1 sv.name FROM sys.views as sv INNER JOIN sys.schemas as ss ON sv.schema_id = ss.schema_id WHERE sv.[type] = 'V' AND ss.name = @SCHEMA ORDER BY sv.[name]);
END
GO

/* Drop all tables */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)
DECLARE @SCHEMA VARCHAR(254) = 'CALDAV'

SELECT @name = (SELECT TOP 1 st.name FROM sys.tables as st INNER JOIN sys.schemas as ss ON st.schema_id = ss.schema_id WHERE st.[type] = 'U' AND ss.name = @SCHEMA ORDER BY st.[name]);
WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP TABLE [' + @SCHEMA + '].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
    SELECT @name = (SELECT TOP 1 st.name FROM sys.tables as st INNER JOIN sys.schemas as ss ON st.schema_id = ss.schema_id WHERE st.[type] = 'U' AND ss.name = @SCHEMA ORDER BY st.[name])
END
GO