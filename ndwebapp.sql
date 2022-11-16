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
    `TeamId` int NOT NULL AUTO_INCREMENT,
    `TeamName` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `LeaderUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Team` PRIMARY KEY (`TeamId`),
    CONSTRAINT `FK_LeaderUserId` FOREIGN KEY (`LeaderUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

ALTER TABLE `AspNetUsers` ADD `TeamId` int NULL;

-- Adding tables for status, suggestions and repairs
CREATE TABLE `Status` (
    `StatusId` int NOT NULL AUTO_INCREMENT,
    `StatusTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Status` PRIMARY KEY (`StatusId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Suggestion` (
    `SuggestionId` int NOT NULL AUTO_INCREMENT,
    `SuggestionTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `SuggestionDescription` text CHARACTER SET utf8mb4 NULL,
    `SuggestionDeadline` date NULL,
    `SuggestionEnddate` date NULL,
    `SuggestedUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `ResponsibleUserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `TeamId` int NULL,
    `StatusId` int NULL,
    CONSTRAINT `PK_Suggestion` PRIMARY KEY (`SuggestionId`),
    CONSTRAINT `FK_SuggestedUserId` FOREIGN KEY (`SuggestedUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ResponsibleUserId` FOREIGN KEY (`ResponsibleUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_SuggestionStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE CASCADE,
    CONSTRAINT `FK_SuggestionTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Repairs` (
    `RepairsId` int NOT NULL AUTO_INCREMENT,
    `RepairsTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `RepairsDescription` text CHARACTER SET utf8mb4 NULL,
    `RepairsDeadline` date NULL,
    `RepairsEnddate` date NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `TeamId` int NULL,
    `StatusId` int NULL,
    CONSTRAINT `PK_Repairs` PRIMARY KEY (`RepairsId`),
    CONSTRAINT `FK_RepairsUserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RepairStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE CASCADE,
    CONSTRAINT `FK_RepairTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

-- Test data
-- Users are created using a model in the Identity framework. These are just for testing purposes for the other tables:
INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"88c215e4-0410-4fba-ac81-b94343ec2010" -- userid
, "admin@admin.net" -- username
, "ADMIN@ADMIN:NET" -- normalized Username
, "admin@admin.net" -- email
, "ADMIN@ADMIN:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "Admin" -- first name
, "" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"af04f53d-7a94-409e-b72e-8d65fb6f9774" -- userid
, "user1@users.net" -- username
, "USER1@USERS:NET" -- normalized Username
, "user1@users.net" -- email
, "USER1@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "One" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"163f3a9a-5566-4737-b5f0-105a8f62fa7d" -- userid
, "user2@users.net" -- username
, "USER2@USERS:NET" -- normalized Username
, "user2@users.net" -- email
, "USER2@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "Two" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"60bbe9f7-d3ab-444d-9f65-a36a411773b4" -- userid
, "user3@users.net" -- username
, "USER3@USERS:NET" -- normalized Username
, "user3@users.net" -- email
, "USER3@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "Three" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"de1d72ab-8c3f-484c-b060-f2e8345464a8" -- userid
, "user4@users.net" -- username
, "USER4@USERS:NET" -- normalized Username
, "user4@users.net" -- email
, "USER4@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "Four" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"a6c61e62-cf37-4421-bbec-f496b1724ab6`" -- userid
, "user5@users.net" -- username
, "USER5@USERS:NET" -- normalized Username
, "user5@users.net" -- email
, "USER5@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "Five" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"6e0db95f-a9d8-4b4d-808c-aaebe769b56c" -- userid
, "user6@users.net" -- username
, "USER6@USERS:NET" -- normalized Username
, "user6@users.net" -- email
, "USER6@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "`User`" -- first name
, "`Six`" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"e31400a0-f278-4603-87ed-f10dc1f60fb8" -- userid
, "user7@users.net" -- username
, "USER7@USERS:NET" -- normalized Username
, "user7@users.net" -- email
, "USER7@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User" -- first name
, "Seven" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"6b1d879f-1e12-491e-9a80-488a04d1c959" -- userid
, "user8@users.net" -- username
, "USER8@USERS:NET" -- normalized Username
, "user8@users.net" -- email
, "USER8@USERS:NET" -- normalized Email
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "`User`" -- first name
, "`Eight`" -- last name
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
"cdfc0707-97ae-465a-9a8b-b65b3678d41b"
, "user9@users.net"
, "USER9@USERS:NET"
, "user9@users.net"
, "USER9@USERS:NET"
, 1 -- email confirmed
, "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==" -- password Hash
, "randomstamp" -- security stamp
, 0 -- phone confirmed
, 0 -- 2fa enabled
, 0 -- lockout enabled
, 0 -- access failed count
, "User"
, "Nine"
);

-- Adding teams
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (1, "IT", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (2, "Administrasjon", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (3, "Økonomi", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (4, "HR", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (5, "Produksjon avd. A", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (6, "Produksjon avd. B", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (7, "Produksjon avd. C", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (8, "Produksjon avd. D", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (9, "Lager avd. A", "88c215e4-0410-4fba-ac81-b94343ec2010");
INSERT INTO `Team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES (10, "Lager avd. B", "88c215e4-0410-4fba-ac81-b94343ec2010");

-- Adding statuses
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (1, "Ikke vurdert");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (2, "Under vurdering");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (3, "Godtatt");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (4, "Avslått");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (5, "Revurderes");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (6, "Pågår");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (7, "Venter på deler");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (8, "Repareres");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (9, "På pause");
INSERT INTO `Status` (`StatusId`, `StatusTitle`) VALUES (10, "Ferdig");

-- Adding suggestions
INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (1, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 4, 2);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (2, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 5, 1);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (3, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, 2022-11-10, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 6, 10);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (4, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 6, 5);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (5, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 8, 5);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (6, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 9, 6);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (7, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 1, 7);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (8, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 2, 8);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (9, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 3, 1);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (10, "Tittel på forslag nr. 1", "Dette er en beskrivelse", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 4, 1);

-- Adding repairs
INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (1, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, 2022-11-10, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 4, 10);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (2, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, 2022-11-10, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 5, 10);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (3, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 6, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (4, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 7, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (5, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 8, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (6, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 9, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (7, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 10, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (8, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 1, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (9, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 2, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (10, "Tittel på reparasjon", "Beskrivelse på reparasjon", 2023-01-15, NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 3, 1);