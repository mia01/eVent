﻿-- Create the database
CREATE DATABASE IF NOT EXISTS `eventapp` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;

CREATE USER IF NOT EXISTS 'eventapp'@'%' IDENTIFIED BY 'eventapp';
GRANT ALL PRIVILEGES ON * . * TO 'eventapp'@'%';

-- Create EF migrations table
CREATE TABLE IF NOT EXISTS `eventapp`.`__EFMigrationsHistory` (
    `MigrationId` NVARCHAR (150) NOT NULL,
    `ProductVersion` NVARCHAR (32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

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
  `CreatedAt` DATE NOT NULL,
  `UpdatedAt` DATE NULL,
  `DueDate` DATE NULL,
  FOREIGN KEY (PriorityId)
  REFERENCES Priority(Id),
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- insert records into the priority table
INSERT INTO eventapp.Priority
(Code, Title, Colour)
VALUES
('low', 'Low', '#008000'),
('medium', 'Medium', '#FFA500'),
('high', 'High', '#ff0000');
