IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HalmaDb')
BEGIN
    CREATE DATABASE HalmaDb;
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'IdentityServerDb')
BEGIN
    CREATE DATABASE IdentityServerDb;
END
GO

USE HalmaDb;
CREATE LOGIN HalmaDbUser WITH PASSWORD = 'P@ssw0rdHalma';
CREATE USER HalmaDbUser FOR LOGIN HalmaDbUser;

-- Grant permissions to HalmaDbUser
ALTER ROLE db_owner ADD MEMBER HalmaDbUser;

USE IdentityServerDb;
CREATE LOGIN IdentityServerDbUser WITH PASSWORD = 'P@ssw0rdIdentity';
CREATE USER IdentityServerDbUser FOR LOGIN IdentityServerDbUser;

-- Grant permissions to IdentityServerDbUser
ALTER ROLE db_owner ADD MEMBER IdentityServerDbUser;