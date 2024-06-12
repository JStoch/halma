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

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [IsLoggedIn] bit NOT NULL,
    [RegistrationDate] datetime2 NOT NULL,
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
    CONSTRAINT [FK_Users_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [PlayerModels] (
    [Guid] nvarchar(450) NOT NULL,
    [ConnectionId] nvarchar(max) NOT NULL,
    [UserGuid] nvarchar(450) NULL,
    CONSTRAINT [PK_PlayerModels] PRIMARY KEY ([Guid]),
    CONSTRAINT [FK_PlayerModels_Users_UserGuid] FOREIGN KEY ([UserGuid]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [Games] (
    [Guid] nvarchar(450) NOT NULL,
    [Player1Guid] nvarchar(450) NOT NULL,
    [Player2Guid] nvarchar(450) NOT NULL,
    [IsGameActive] bit NOT NULL,
    CONSTRAINT [PK_Games] PRIMARY KEY ([Guid]),
    CONSTRAINT [FK_Games_PlayerModels_Player1Guid] FOREIGN KEY ([Player1Guid]) REFERENCES [PlayerModels] ([Guid]),
    CONSTRAINT [FK_Games_PlayerModels_Player2Guid] FOREIGN KEY ([Player2Guid]) REFERENCES [PlayerModels] ([Guid])
);
GO

CREATE TABLE [Statistics] (
    [Guid] nvarchar(450) NOT NULL,
    [PlayerGuid] nvarchar(450) NOT NULL,
    [GamesPlayed] int NOT NULL,
    [GamesWon] int NOT NULL,
    [AvgWinRate] float NOT NULL,
    [LastPlayedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Statistics] PRIMARY KEY ([Guid]),
    CONSTRAINT [FK_Statistics_PlayerModels_PlayerGuid] FOREIGN KEY ([PlayerGuid]) REFERENCES [PlayerModels] ([Guid]) ON DELETE CASCADE
);
GO

CREATE TABLE [GamesHistory] (
    [Guid] nvarchar(450) NOT NULL,
    [GameModelGuid] nvarchar(450) NULL,
    CONSTRAINT [PK_GamesHistory] PRIMARY KEY ([Guid]),
    CONSTRAINT [FK_GamesHistory_Games_GameModelGuid] FOREIGN KEY ([GameModelGuid]) REFERENCES [Games] ([Guid])
);
GO

CREATE TABLE [PiecePositionModels] (
    [Guid] nvarchar(450) NOT NULL,
    [X] int NOT NULL,
    [Y] int NOT NULL,
    [GameGuid] nvarchar(450) NOT NULL,
    [OwnerGuid] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PiecePositionModels] PRIMARY KEY ([Guid]),
    CONSTRAINT [FK_PiecePositionModels_Games_GameGuid] FOREIGN KEY ([GameGuid]) REFERENCES [Games] ([Guid]) ON DELETE CASCADE,
    CONSTRAINT [FK_PiecePositionModels_PlayerModels_OwnerGuid] FOREIGN KEY ([OwnerGuid]) REFERENCES [PlayerModels] ([Guid]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Games_Player1Guid] ON [Games] ([Player1Guid]);
GO

CREATE INDEX [IX_Games_Player2Guid] ON [Games] ([Player2Guid]);
GO

CREATE INDEX [IX_GamesHistory_GameModelGuid] ON [GamesHistory] ([GameModelGuid]);
GO

CREATE INDEX [IX_PiecePositionModels_GameGuid] ON [PiecePositionModels] ([GameGuid]);
GO

CREATE INDEX [IX_PiecePositionModels_OwnerGuid] ON [PiecePositionModels] ([OwnerGuid]);
GO

CREATE INDEX [IX_PlayerModels_UserGuid] ON [PlayerModels] ([UserGuid]);
GO

CREATE INDEX [IX_Statistics_PlayerGuid] ON [Statistics] ([PlayerGuid]);
GO

CREATE INDEX [IX_Users_UserId] ON [Users] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240612184811_init', N'8.0.6');
GO

COMMIT;
GO

