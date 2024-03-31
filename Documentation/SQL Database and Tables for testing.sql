--Create database SafetyToolBox

drop table Attendance
drop table Employees
drop table Certifications
drop table Users
drop table Roles
drop table CertificationTypes
drop table Positions
drop table CertificationPositionMap
drop table Topics

Create table Attendance (
	EmployeeID int not null,
	AttendanceDate Date not null,
	Present bit, 
	Excused bit,
	Absent bit,
	Constraint PersonDate Primary KEY (EmployeeID, AttendanceDate))

Create table Employees (
	EmployeeID int not null identity(1,1) primary key,
	EmployeeFirstName varchar(100),
	EmployeeLastName varchar(100))

Create table Certifications(
	EmployeeID int not null, 
	CertificationID int not null,
	TrainedOnDate Date,
	ExpiryDate Date 
	Constraint PersonCert Primary KEY (EmployeeID, CertificationID)
)

Create Table CertificationTypes(
	CertificationId int not null primary key IDENTITY(1,1),
	CertificationName varchar(70)
)

Create table Users(
	UserID int IDENTITY,
	Email varchar(250),
	Username varchar(100),
	Salt varbinary(200),
	HashedPassword varbinary(200),
	RoleID int
)

Create table Roles(
	RoleID int not null primary key,
	RoleName varchar(250)
)

Create Table Positions(
	PositionID int not null primary key,
	PositionName varchar(250)
)

Create Table CertificationPositionMap(
	CertificationPositionMapID int not null primary key,
	CertificationID int, /*needs to be an FK*/
	PositionID int /*needs to be an FK*/
)

Create table Topics(TopicIdea varchar(200))

Create table Notes (NoteDate Date, NoteContent varchar(500))

INSERT INTO Employees Values('Bob', 'Bobington');
INSERT INTO Employees Values('Joe', 'Jones');
INSERT INTO Employees Values('Sue', 'Snow');
INSERT INTO Employees values('Bill', 'Nye');

INSERT INTO Attendance Values(1, '2023-11-04', 1, 0, 0);
INSERT INTO Attendance Values(2, '2023-11-04', 0, 1, 0);
INSERT INTO Attendance Values(3, '2023-11-04', 0, 0, 1);

INSERT INTO Certifications Values(1, 1, '2019-12-20', '2024-04-30');
INSERT INTO Certifications Values(2, 2, '2021-04-30', '2024-02-15');
INSERT INTO Certifications Values(3, 3, '2020-07-15', '2024-07-20');
INSERT INTO Certifications Values(2, 4, '2021-08-17', '2024-01-20');
INSERT INTO Certifications Values(3, 5, '2020-06-01', null);
INSERT INTO Certifications Values(1, 6, '2018-12-20', '2024-08-30');

/* I somehow lost the SQL file with the proper ones... i hate myself*/
INSERT INTO CertificationTypes Values('Test for Level 1 Operator');
INSERT INTO CertificationTypes Values('Test1 for Supervisor');
INSERT INTO CertificationTypes Values('Test3');
INSERT INTO CertificationTypes Values('Test2 for Level 1 Operator');
INSERT INTO CertificationTypes Values('Test2 for Supervisor');
INSERT INTO CertificationTypes Values('Test6');

INSERT INTO Positions Values(1, 'Level 1 Operator');
INSERT INTO Positions Values(2, 'Supervisor');
INSERT INTO Positions Values(3, 'Level 2 Operator');

INSERT INTO CertificationPositionMap Values(1, 1, 1);
INSERT INTO CertificationPositionMap Values(2, 4, 1);
INSERT INTO CertificationPositionMap Values(3, 2, 2);
INSERT INTO CertificationPositionMap Values(4, 5, 2);
INSERT INTO CertificationPositionMap Values(5, 1, 3);
INSERT INTO CertificationPositionMap Values(6, 4, 3);
INSERT INTO CertificationPositionMap Values(7, 6, 3);

INSERT INTO Roles Values(1, 'IT');
INSERT INTO Roles Values(2, 'Management');
INSERT INTO Roles Values(3, 'readonly');

SELECT * FROM Employees
SELECT * FROM Attendance
SELECT * FROM Certifications
SELECT * FROM CertificationTypes
SELECT * FROM Roles
SELECT * FROM Users
SELECT * FROM Positions
SELECT * FROM CertificationPositionMap
SELECT * FROM Topics
SELECT * FROM Notes