IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Statistics] (
    [Id] int NOT NULL IDENTITY,
    [GamesPlayed] int NOT NULL,
    [GamesWon] int NOT NULL,
    [AvgScore] float NOT NULL,
    [HighScore] float NOT NULL,
    [LastPlayedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Statistics] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [Guid] nvarchar(max) NULL,
    [IsLoggedIn] bit NOT NULL,
    [RegistrationDate] datetime2 NOT NULL,
    [StatisticId] int NULL,
    [UserId] nvarchar(450) NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Statistics_StatisticId] FOREIGN KEY ([StatisticId]) REFERENCES [Statistics] ([Id]),
    CONSTRAINT [FK_Users_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [Games] (
    [Uid] nvarchar(450) NOT NULL,
    [Player1RefUid] nvarchar(max) NOT NULL,
    [Player1Id] nvarchar(450) NULL,
    [Player2RefUid] nvarchar(max) NOT NULL,
    [Player2Id] nvarchar(450) NULL,
    CONSTRAINT [PK_Games] PRIMARY KEY ([Uid]),
    CONSTRAINT [FK_Games_Users_Player1Id] FOREIGN KEY ([Player1Id]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_Games_Users_Player2Id] FOREIGN KEY ([Player2Id]) REFERENCES [Users] ([Id])
);
GO

CREATE INDEX [IX_Games_Player1Id] ON [Games] ([Player1Id]);
GO

CREATE INDEX [IX_Games_Player2Id] ON [Games] ([Player2Id]);
GO

CREATE INDEX [IX_Users_StatisticId] ON [Users] ([StatisticId]);
GO

CREATE INDEX [IX_Users_UserId] ON [Users] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240601130543_init', N'8.0.6');
GO

COMMIT;
GO

