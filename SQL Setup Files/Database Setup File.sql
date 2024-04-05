-- SQL Setup File
-- This file needs to be run first to set up the database and all of the tables.

CREATE DATABASE SafetyToolBox

CREATE TABLE Attendance (
	EmployeeID INT NOT NULL,
	AttendanceDate Date NOT NULL,
	Present BIT, 
	Excused BIT,
	Absent BIT,
	CONSTRAINT PersonDate Primary KEY (EmployeeID, AttendanceDate)
)

CREATE TABLE Positions(
	PositionID INT IDENTITY(1,1) PRIMARY KEY,
	PositionName VARCHAR(250)
)

CREATE TABLE Employees (
	EmployeeID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EmployeeFirstName VARCHAR(100),
	EmployeeLastName VARCHAR(100),
	PositionID INT,
	CONSTRAINT FK_Employee_Position FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
)

CREATE TABLE Certifications(
	EmployeeID INT NOT NULL, 
	CertificationID INT NOT NULL,
	TrainedOnDate DATE,
	ExpiryDate DATE 
	CONSTRAINT PersonCert PRIMARY KEY (EmployeeID, CertificationID)
)

CREATE TABLE CertificationTypes(
	CertificationID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	CertificationName VARCHAR(70)
)

CREATE TABLE Roles(
	RoleID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	RoleName VARCHAR(250)
)

CREATE TABLE Users(
	UserID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Email VARCHAR(250),
	Username VARCHAR(100),
	Salt VARBINARY(200),
	HashedPassword VARBINARY(200),
	RoleID INT,
	CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
)

CREATE TABLE CertificationPositionMap(
	CertificationPositionMapID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	CertificationID INT,
	PositionID INT,
	CONSTRAINT FK_CertPosition_CertType FOREIGN KEY (CertificationID) REFERENCES CertificationTypes(CertificationID),
	CONSTRAINT FK_CertPosition_Positions FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
)

CREATE TABLE Topics(
	TopicIdea VARCHAR(200)
)

CREATE TABLE Notes(
	NoteDate DATE,
	NoteContent VARCHAR(500)
)