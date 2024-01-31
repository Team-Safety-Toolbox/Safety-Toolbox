--Create database SafetyToolBox

drop table Attendance
drop table Employees
drop table Certifications
drop table Users
drop table Roles
drop table CertificationTypes

Create table Attendance (
	EmployeeID int not null,
	AttendanceDate Date not null,
	Present bit, 
	Excused bit,
	Absent bit,
	Constraint PersonDate Primary KEY (EmployeeID, AttendanceDate))

Create table Employees (
	EmployeeID int not null primary key,
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
	Password varchar(100),
	RoleID int
)

Create table Roles(
	RoleID int not null primary key,
	RoleName varchar(250)
)

INSERT INTO Employees Values(1, 'Bob', 'Bobington');
INSERT INTO Employees Values(2, 'Joe', 'Jones');
INSERT INTO Employees Values(3, 'Sue', 'Snow');
INSERT INTO Employees values(4, 'Bill', 'Nye');

INSERT INTO Attendance Values(1, '2023-11-04', 1, 0, 0);
INSERT INTO Attendance Values(2, '2023-11-04', 0, 1, 0);
INSERT INTO Attendance Values(3, '2023-11-04', 0, 0, 1);

INSERT INTO Certifications Values(1, 1, '2019-12-20', '2024-04-30');
INSERT INTO Certifications Values(2, 2, '2021-04-30', '2024-02-15');
INSERT INTO Certifications Values(3, 3, '2020-07-15', '2024-07-20');
INSERT INTO Certifications Values(2, 4, '2021-08-17', '2024-01-20');
INSERT INTO Certifications Values(3, 5, '2020-06-01', null);
INSERT INTO Certifications Values(1, 6, '2018-12-20', '2024-08-30');

INSERT INTO Roles Values(1, 'IT');
INSERT INTO Roles Values(2, 'Management');
INSERT INTO Roles Values(3, 'readonly');

INSERT INTO Users Values('mikayla@email.com', 'mik', 'Abc', 1); 
INSERT INTO Users Values('test', 'test', '1', 3); 

SELECT * FROM Employees
SELECT * FROM Attendance
SELECT * FROM Certifications
SELECT * FROM CertificationTypes
SELECT * FROM Roles
SELECT * FROM Users