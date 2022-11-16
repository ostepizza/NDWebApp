-- Adding the tables for the Identity framework
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;


CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);


-- Updating the AspNetUsers with Employee fields
ALTER TABLE `AspNetUsers` ADD `empFname` varchar(250) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `AspNetUsers` ADD `empLname` varchar(250) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `AspNetUsers` ADD `empNr` int NOT NULL DEFAULT 0;

-- Adding a table for the teams
CREATE TABLE `Team` (
    `TeamId` int NOT NULL,
    `TeamName` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `LeaderUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Team` PRIMARY KEY (`TeamId`),
    CONSTRAINT `FK_LeaderUserId` FOREIGN KEY (`LeaderUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

ALTER TABLE `AspNetUsers` ADD `TeamId` int NULL;

-- Adding tables for status, suggestions and repairs
CREATE TABLE `Status` (
    `StatusId` int NOT NULL,
    `StatusTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Status` PRIMARY KEY (`StatusId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Suggestion` (
    `SuggestionId` int NOT NULL,
    `SuggestionTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `SuggestionDescription` text CHARACTER SET utf8mb4 NULL,
    `SuggestionDeadline` datetime NULL,
    `SuggestionEnddate` datetime NULL,
    `SuggestedUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `ResponibleUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `TeamId` int NULL,
    `StatusId` int NULL,
    CONSTRAINT `PK_Suggestion` PRIMARY KEY (`SuggestionId`),
    CONSTRAINT `FK_SuggestedUserId` FOREIGN KEY (`SuggestedUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ResponsibleUserId` FOREIGN KEY (`ResponibleUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_SuggestionStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE CASCADE,
    CONSTRAINT `FK_SuggestionTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Repairs` (
    `RepairsId` int NOT NULL,
    `RepairsTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `RepairsDescription` text CHARACTER SET utf8mb4 NULL,
    `RepairsDeadline` datetime NULL,
    `RepairsEnddate` datetime NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `TeamId` int NULL,
    `StatusId` int NULL,
    CONSTRAINT `PK_Repairs` PRIMARY KEY (`RepairsId`),
    CONSTRAINT `FK_RepairsUserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RepairStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE CASCADE,
    CONSTRAINT `FK_RepairTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;