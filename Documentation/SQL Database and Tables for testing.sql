Create database SafetyToolBox

drop table Attendance
drop table Employees
drop table Certifications

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
	EmployeeID int not null primary key, 
	CertType varchar(50),
	TrainedOnDate Date,
	ExpiryDate Date
)

INSERT INTO Employees Values(1, 'Bob', 'Bobington');
INSERT INTO Employees Values(2, 'Joe', 'Jones');
INSERT INTO Employees Values(3, 'Sue', 'Snow');

INSERT INTO Attendance Values(1, '2023-11-04', 1, 0, 0);
INSERT INTO Attendance Values(2, '2023-11-04', 0, 1, 0);
INSERT INTO Attendance Values(3, '2023-11-04', 0, 0, 1);

INSERT INTO Certifications Values(1, 'Crane Operation', '2019-12-20', '2024-04-30');
INSERT INTO Certifications Values(2, 'Kleen Press', '2021-04-30', '2024-02-15');
INSERT INTO Certifications Values(3, 'First Aid', '2020-07-15', '2024-07-20');

SELECT * FROM Employees
SELECT * FROM Attendance
SELECT * FROM Certifications