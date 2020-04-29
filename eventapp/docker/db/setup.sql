-- Create the database
CREATE DATABASE IF NOT EXISTS `eventapp` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;

CREATE USER IF NOT EXISTS 'eventapp'@'%' IDENTIFIED BY 'eventapp';
GRANT ALL PRIVILEGES ON * . * TO 'eventapp'@'%';

-- Create EF migrations table
CREATE TABLE IF NOT EXISTS `eventapp`.`__EFMigrationsHistory` (
    `MigrationId` NVARCHAR (150) NOT NULL,
    `ProductVersion` NVARCHAR (32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

CREATE TABLE IF NOT EXISTS `eventapp`.`AspNetRoles` (
  `Id` varchar(85) CHARACTER SET utf8mb4 NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4  DEFAULT NULL,
  `NormalizedName` varchar(85) CHARACTER SET utf8mb4  DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 ,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE IF NOT EXISTS  `eventapp`.`AspNetRoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 ,
  `ClaimValue` longtext CHARACTER SET utf8mb4 ,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;


CREATE TABLE IF NOT EXISTS `eventapp`.`AspNetUsers` (
  `Id` varchar(255) CHARACTER SET utf8mb4  NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4  DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4  DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4  DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4  DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 ,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 ,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 ,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 ,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;


CREATE TABLE IF NOT EXISTS `eventapp`.`AspNetUserTokens` (
  `UserId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `LoginProvider` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `Name` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 ,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE  IF NOT EXISTS `eventapp`.`AspNetUserRoles` (
  `UserId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `RoleId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE  IF NOT EXISTS `eventapp`.`AspNetUserLogins` (
  `LoginProvider` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `ProviderKey` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 ,
  `UserId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE  IF NOT EXISTS `eventapp`.`AspNetUserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 ,
  `ClaimValue` longtext CHARACTER SET utf8mb4 ,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE IF NOT EXISTS  `eventapp`.`PersistedGrants` (
  `Key` varchar(200) CHARACTER SET utf8mb4  NOT NULL,
  `Type` varchar(50) CHARACTER SET utf8mb4  NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8mb4  DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4  NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `Data` longtext CHARACTER SET utf8mb4  NOT NULL,
  PRIMARY KEY (`Key`),
  KEY `IX_PersistedGrants_Expiration` (`Expiration`),
  KEY `IX_PersistedGrants_SubjectId_ClientId_Type` (`SubjectId`,`ClientId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 ;

CREATE TABLE  IF NOT EXISTS  `eventapp`.`IdentityUser` (
  `Id` varchar(85) CHARACTER SET utf8mb4  NOT NULL,
  `UserName` longtext CHARACTER SET utf8mb4 ,
  `NormalizedUserName` varchar(85) CHARACTER SET utf8mb4  DEFAULT NULL,
  `Email` longtext CHARACTER SET utf8mb4 ,
  `NormalizedEmail` varchar(85) CHARACTER SET utf8mb4  DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 ,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 ,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 ,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 ,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `eventapp`.`DeviceCodes` (
  `UserCode` varchar(200) CHARACTER SET utf8mb4  NOT NULL,
  `DeviceCode` varchar(200) CHARACTER SET utf8mb4  NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8mb4  DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4  NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4  NOT NULL,
  PRIMARY KEY (`UserCode`),
  UNIQUE KEY `IX_DeviceCodes_DeviceCode` (`DeviceCode`),
  KEY `IX_DeviceCodes_Expiration` (`Expiration`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create priority table
CREATE TABLE IF NOT EXISTS `eventapp`.`Priority` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Code` VARCHAR(30) NOT NULL,
  `Title` VARCHAR(60) NOT NULL,
  `Colour` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

-- Create tasks table
CREATE TABLE IF NOT EXISTS `eventapp`.`Tasks` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(100) NOT NULL,
  `Description` TEXT NULL,
  `Done` TINYINT NOT NULL,
  `PriorityId` INT DEFAULT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DueDate` DATETIME NULL,
  `CreatedBy`  VARCHAR(255) NULL,
  `AssignedTo` VARCHAR(255) NULL,
  `Reminder` TINYINT NOT NULL,
  `ReminderSent` TINYINT NOT NULL DEFAULT 0,
  FOREIGN KEY (PriorityId)
  REFERENCES Priority(Id),
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `eventapp`.`UserFriend` (
`Id` INT NOT NULL AUTO_INCREMENT,
`UserId` VARCHAR(255) NOT NULL,
`UserFriendId` VARCHAR(255) NOT NULL,
`Accepted` TINYINT NOT NULL DEFAULT 0,
PRIMARY KEY (`Id`),
UNIQUE KEY (`UserId`, `UserFriendId`));

-- insert records into the priority table
INSERT INTO eventapp.Priority
(Code, Title, Colour)
VALUES
('low', 'Low', '#008000'),
('medium', 'Medium', '#FFA500'),
('high', 'High', '#ff0000');

-- Create event table
CREATE TABLE IF NOT EXISTS `eventapp`.`Events` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(100) NOT NULL,
  `Description` TEXT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `CreatedBy`  VARCHAR(255) NULL,
  `UpdatedAt` DATETIME NULL,
  `StartDate` DATETIME NULL,
  `EndDate` DATETIME NULL,
  `Reminder` TINYINT NOT NULL,
  `PublicEvent` TINYINT NOT NULL,
  `ReminderSent` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;
