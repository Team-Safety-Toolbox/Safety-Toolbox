Create database SafetyToolBox

Create table Attendance (
	EmployeeID int not null,
	AttendanceDate Date not null,
	Present bit, 
	Excused bit,
	Absent bit,
	Constraint PersonDate Primary KEY (EmployeeID, AttendanceDate))

Create table Employees (
	EmployeeID int not null primary key,
	EmployeeName varchar(100))

Create table Certifications(
	EmployeeID int not null primary key, 
	CertType varchar(50),
	ExpiryDate Date
)

INSERT INTO Employees Values(1, 'Bob');
INSERT INTO Employees Values(2, 'Joe');
INSERT INTO Employees Values(3, 'Sue');

INSERT INTO Attendance Values(1, '2023-11-04', 1, 0, 0);
INSERT INTO Attendance Values(2, '2023-11-04', 0, 1, 0);
INSERT INTO Attendance Values(3, '2023-11-04', 0, 0, 1);

INSERT INTO Certifications Values(1, 'Forklift', '2023-12-10');
INSERT INTO Certifications Values(2, 'Forklift', '2023-11-15');
INSERT INTO Certifications Values(3, 'Forklift', '2024-07-20');

SELECT * FROM Employees
SELECT * FROM Attendance
SELECT * FROM Certifications