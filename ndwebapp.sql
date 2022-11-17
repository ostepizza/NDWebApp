-- Database setup
CREATE DATABASE IF NOT EXISTS webappdatabase;
USE webappdatabase;
ALTER DATABASE CHARACTER SET utf8mb4;

-- Adding the tables for the Identity framework
CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    PRIMARY KEY (`Id`)
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
    PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;


CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
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
    PRIMARY KEY (`TeamId`),
    CONSTRAINT `FK_LeaderUserId` FOREIGN KEY (`LeaderUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

ALTER TABLE `AspNetUsers` ADD `TeamId` int NULL;

ALTER TABLE `AspNetUsers` ADD CONSTRAINT `FK_TeamId_AspNetUsers_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE SET NULL;

-- Adding tables for status, suggestions and repairs
CREATE TABLE `Status` (
    `StatusId` int NOT NULL AUTO_INCREMENT,
    `StatusTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    PRIMARY KEY (`StatusId`)
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
    `StatusId` int NULL DEFAULT 0,
    PRIMARY KEY (`SuggestionId`),
    CONSTRAINT `FK_SuggestedUserId` FOREIGN KEY (`SuggestedUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_ResponsibleUserId` FOREIGN KEY (`ResponsibleUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_SuggestionStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE SET NULL,
    CONSTRAINT `FK_SuggestionTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE TABLE `Repairs` (
    `RepairsId` int NOT NULL AUTO_INCREMENT,
    `RepairsTitle` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `RepairsDescription` text CHARACTER SET utf8mb4 NULL,
    `RepairsDeadline` date NULL,
    `RepairsEnddate` date NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `TeamId` int NULL,
    `StatusId` int NULL DEFAULT 0,
    PRIMARY KEY (`RepairsId`),
    CONSTRAINT `FK_RepairsUserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_RepairStatus` FOREIGN KEY (`StatusId`) REFERENCES `Status` (`StatusId`) ON DELETE SET NULL,
    CONSTRAINT `FK_RepairTeam` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

-- Test data
-- Users are created using a model in the Identity framework. These are just for testing purposes for the other tables:
INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "88c215e4-0410-4fba-ac81-b94343ec2010",
    "admin@admin.net",
    "ADMIN@ADMIN.NET",
    "admin@admin.net",
    "ADMIN@ADMIN.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Admin",
    ""
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "af04f53d-7a94-409e-b72e-8d65fb6f9774",
    "user1@users.net",
    "USER1@USERS.NET",
    "user1@users.net",
    "USER1@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "User",
    "One"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "163f3a9a-5566-4737-b5f0-105a8f62fa7d",
    "user2@users.net",
    "USER2@USERS.NET",
    "user2@users.net",
    "USER2@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Duser",
    "Two"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "60bbe9f7-d3ab-444d-9f65-a36a411773b4",
    "user3@users.net",
    "USER3@USERS.NET",
    "user3@users.net",
    "USER3@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Fuser",
    "Three"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "de1d72ab-8c3f-484c-b060-f2e8345464a8",
    "user4@users.net",
    "USER4@USERS.NET",
    "user4@users.net",
    "USER4@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Kuser",
    "Four"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "a6c61e62-cf37-4421-bbec-f496b1724ab6",
    "user5@users.net",
    "USER5@USERS.NET",
    "user5@users.net",
    "USER5@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Muser",
    "Five"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "6e0db95f-a9d8-4b4d-808c-aaebe769b56c",
    "user6@users.net",
    "USER6@USERS.NET",
    "user6@users.net",
    "USER6@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Suser",
    "Six"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "e31400a0-f278-4603-87ed-f10dc1f60fb8",
    "user7@users.net",
    "USER7@USERS.NET",
    "user7@users.net",
    "USER7@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Buser",
    "Seven"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "6b1d879f-1e12-491e-9a80-488a04d1c959",
    "user8@users.net",
    "USER8@USERS.NET",
    "user8@users.net",
    "USER8@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Pusur",
    "Eight"
);

INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, empFname, empLname)
VALUES(
    "cdfc0707-97ae-465a-9a8b-b65b3678d41b",
    "user9@users.net",
    "USER9@USERS.NET",
    "user9@users.net",
    "USER9@USERS.NET",
    1,
    "AQAAAAEAACcQAAAAEEw7yvYYvDvjn7CyvoDpnP1Dsr/p+61upoGSdjJE2jwbYiTXPL81iSzehKAdt6a5wg==",
    "randomstamp",
    0,
    0,
    0,
    0,
    "Vuser",
    "Nine"
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

-- Updating users to be part of teams
UPDATE `AspNetUsers` SET `TeamId` = 1 WHERE `UserName` = "admin@admin.net";
UPDATE `AspNetUsers` SET `TeamId` = 2 WHERE `UserName` = "user1@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 3 WHERE `UserName` = "user2@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 4 WHERE `UserName` = "user3@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 5 WHERE `UserName` = "user4@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 6 WHERE `UserName` = "user5@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 7 WHERE `UserName` = "user6@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 8 WHERE `UserName` = "user7@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 9 WHERE `UserName` = "user8@users.net";
UPDATE `AspNetUsers` SET `TeamId` = 10 WHERE `UserName` = "user9@users.net";

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
VALUES (1, "Tittel på forslag nr. 1", "Dette er en beskrivelse", '2023-01-15', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 4, 6);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (2, "Tittel på forslag nr. 2", "Også en beskrivelse", '2023-01-17', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "de1d72ab-8c3f-484c-b060-f2e8345464a8", 5, 1);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (3, "Tittel på forslag nr. 3", "En bra beskrivelse", '2023-02-05', '2022-11-10', "af04f53d-7a94-409e-b72e-8d65fb6f9774", "6b1d879f-1e12-491e-9a80-488a04d1c959", 6, 10);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (4, "Tittel på forslag nr. 4", "En dårlig beskrivelse", '2023-03-25', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 6, 5);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (5, "Tittel på forslag nr. 5", "Den beste beskrivelsen", '2023-01-05', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", "de1d72ab-8c3f-484c-b060-f2e8345464a8", 8, 6);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (6, "Tittel på forslag nr. 6", "En kreativ beskrivelse", '2023-02-13', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", "6b1d879f-1e12-491e-9a80-488a04d1c959", 9, 6);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (7, "Tittel på forslag nr. 7", "En usunn beskrivelse av et sunt forslag", '2023-03-30', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", "88c215e4-0410-4fba-ac81-b94343ec2010", 1, 7);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (8, "Tittel på forslag nr. 8", "En helt ok beskrivelse", '2023-04-27', NULL, "6e0db95f-a9d8-4b4d-808c-aaebe769b56c", "de1d72ab-8c3f-484c-b060-f2e8345464a8", 2, 8);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (9, "Tittel på forslag nr. 9", "En god beskrivelse", '2023-04-12', NULL, "6e0db95f-a9d8-4b4d-808c-aaebe769b56c", "6b1d879f-1e12-491e-9a80-488a04d1c959", 6, 6);

INSERT INTO `Suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`)
VALUES (10, "Tittel på forslag nr. 10", "En kjedelig beskrivelse", '2023-01-10', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", "88c215e4-0410-4fba-ac81-b94343ec2010", 4, 1);

-- Adding repairs
INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (1, "Noe er ødelagt", "Beskrivelse på reparasjon", '2023-01-15', '2022-11-10', "af04f53d-7a94-409e-b72e-8d65fb6f9774", 4, 10);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (2, "En tittel", "En annen beskrivelse", '2023-01-15', '2022-11-10', "60bbe9f7-d3ab-444d-9f65-a36a411773b4", 5, 10);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (3, "Noe annet er ødelagt", "Mange ord som blir til en setning", '2023-01-15', NULL, "6e0db95f-a9d8-4b4d-808c-aaebe769b56c", 6, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (4, "Enda en tittel", "Få ord", '2023-01-15', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", 7, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (5, "Brann i søppelkassa", "Heldigvis ikke en hel bok", '2023-01-15', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 8, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (6, "Atter en tittel", "Atter en beskrivelse", '2023-01-15', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", 9, 7);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (7, "Brann i fuglekassa", "Ingen fugler skadd", '2023-01-15', NULL, "6e0db95f-a9d8-4b4d-808c-aaebe769b56c", 10, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (8, "Nok en tittel", "Jada!", '2023-01-15', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", 1, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (9, "Brusmaskin ødelagt", "Trenger brus", '2023-01-15', NULL, "af04f53d-7a94-409e-b72e-8d65fb6f9774", 2, 1);

INSERT INTO `Repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`)
VALUES (10, "Kan det fikses?", "Klart det kan!", '2023-01-15', NULL, "60bbe9f7-d3ab-444d-9f65-a36a411773b4", 3, 1);

-- Testing the data
-- Listing the 5 first rows of the 5 most important tables, sorted:
SELECT * FROM `AspNetUsers` ORDER BY `Id` ASC LIMIT 5;
SELECT * FROM `Team` ORDER BY `TeamId` ASC LIMIT 5;
SELECT * FROM `Suggestion` ORDER BY `SuggestionId` ASC LIMIT 5;
SELECT * FROM `Repairs` ORDER BY `RepairsId` ASC LIMIT 5;
SELECT * FROM `Status` ORDER BY `StatusId` ASC LIMIT 5;

-- List all suggestions with their suggesting employee and responsible employee
SELECT s.`SuggestedUserId` AS IDofSuggester, u.`empFname` AS SuggestedFirstName, u.`empLname` AS SuggestedLastName, s.`SuggestionTitle`, s.`SuggestionDescription`, s.`ResponsibleUserId` AS IDofResponsible
FROM `AspNetUsers` AS u JOIN `Suggestion` AS s WHERE u.`Id`=s.`SuggestedUserId`;

-- List all employees that have not submitted any suggestions
SELECT `Id`, `empFname`, `empLname` FROM `AspNetUsers` WHERE `Id` NOT IN (SELECT `SuggestedUserId` FROM `Suggestion`);

-- List the names and number of suggestions of the three users with most suggestions submitted, sorted by number of submissions
SELECT u.`Id`, u.`EmpFname`, u.`empLname`, (SELECT COUNT(*) FROM `Suggestion` WHERE `SuggestedUserId` = u.`Id`) SuggestionsAdded FROM `AspNetUsers` u ORDER BY SuggestionsAdded DESC LIMIT 3;

-- List all the suggestions by the employee with the highest number of suggestions submitted, sorted by date/time
SELECT u.`empFname`, u.`empLname`, s.`SuggestionTitle`, s.`SuggestionDescription`, s.`SuggestionDeadline`
FROM `AspNetUsers` AS u INNER JOIN `Suggestion` AS s ON s.`SuggestedUserId`=u.`Id`
WHERE u.`Id`=(SELECT `SuggestedUserId` FROM `Suggestion` GROUP BY `SuggestedUserId` ORDER BY COUNT(`SuggestedUserId`) DESC LIMIT 1)
ORDER BY s.SuggestionDeadline ASC;

-- List all suggestions that are in the "ACT" state at the moment
SELECT su.`SuggestionTitle`, st.`StatusTitle`
FROM `Suggestion` AS su INNER JOIN `Status` AS st
WHERE st.`StatusId` = 6 AND st.`StatusId` = su.`StatusId`;

-- List all teams with total number of suggestions handled per team, ordered by number of suggestions, the team with most suggestions first
SELECT t.`TeamID`, t.`TeamName`, count(s.`TeamID`) TotalSuggestionsAdded
FROM `Team` t
LEFT JOIN `Suggestion` s ON t.`TeamId` = s.`TeamId`
GROUP BY t.`TeamID`, t.`TeamName`
ORDER BY TotalSuggestionsAdded DESC;
