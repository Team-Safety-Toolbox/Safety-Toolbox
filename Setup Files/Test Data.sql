-- TEST DATA
-- This is a bunch of test data that is used for demo purposes

-- POSITIONS
INSERT INTO Positions Values('Level 1 Operator');
INSERT INTO Positions Values('Supervisor');
INSERT INTO Positions Values('Level 2 Operator');

-- EMPLOYEES
INSERT INTO Employees Values('Bob', 'Bobington', 1);
INSERT INTO Employees Values('Joe', 'Jones', 2);
INSERT INTO Employees Values('Sue', 'Snow', 2);
INSERT INTO Employees values('Bill', 'Nye', 1);

--ATTENDANCE
INSERT INTO Attendance Values(1, '2023-11-04', 1, 0, 0);
INSERT INTO Attendance Values(2, '2023-11-04', 0, 1, 0);
INSERT INTO Attendance Values(3, '2023-11-04', 0, 0, 1);

-- CERTIFICATIONS
INSERT INTO Certifications Values(1, 1, '2019-12-20', '2024-04-30');
INSERT INTO Certifications Values(2, 2, '2021-04-30', '2024-02-15');
INSERT INTO Certifications Values(3, 3, '2020-07-15', '2024-07-20');
INSERT INTO Certifications Values(2, 4, '2021-08-17', '2024-01-20');
INSERT INTO Certifications Values(1, 5, '2020-06-01', null);
INSERT INTO Certifications Values(3, 6, null, '2024-08-30');
INSERT INTO Certifications Values(1, 10, '2020-12-20', '2024-08-30');
INSERT INTO Certifications Values(2, 10, '2021-12-20', '2024-08-30');
INSERT INTO Certifications Values(3, 10, '2022-12-20', '2024-08-30');

-- CERTIFICATION TYPES
INSERT INTO CertificationTypes Values('Tower Motor Safety');
INSERT INTO CertificationTypes Values('Lock Out Tag Out');
INSERT INTO CertificationTypes Values('Bio Hazard');
INSERT INTO CertificationTypes Values('Cranes/Slings');
INSERT INTO CertificationTypes Values('Lifting');
INSERT INTO CertificationTypes Values('Fire/Emergency');
INSERT INTO CertificationTypes Values('Work Order System Training');
INSERT INTO CertificationTypes Values('Operator Trainee Orientation Per OTP-001 Checklist');
INSERT INTO CertificationTypes Values('First Aid Training');
INSERT INTO CertificationTypes Values('WHIMIS Training');
INSERT INTO CertificationTypes Values('O.H.& S. Training');
INSERT INTO CertificationTypes Values('Supervision Training');
INSERT INTO CertificationTypes Values('Heat Treating Qualification');
INSERT INTO CertificationTypes Values('MPI Training');
INSERT INTO CertificationTypes Values('Feeding & Cold Blanking Press Operation');
INSERT INTO CertificationTypes Values('Press Tool Setting');
INSERT INTO CertificationTypes Values('B/Berg Friction Screw Press Controls-Old Relay & New  PLC Panels');
INSERT INTO CertificationTypes Values('Rail Clip Scragger and Form Machine');
INSERT INTO CertificationTypes Values('Roll Straightener Set Up');
INSERT INTO CertificationTypes Values('Disc Lathe Operation');
INSERT INTO CertificationTypes Values('Kleen Press Operation');
INSERT INTO CertificationTypes Values('Kleen Press #3 (New Shut Height control)');
INSERT INTO CertificationTypes Values('Drill Press Set Up/Bit Change/Counter Sink Hole Depth');
INSERT INTO CertificationTypes Values('Others Per OTP-002 to OTP-018');
INSERT INTO CertificationTypes Values('Process Trouble Shooting and Corrective Action Techniques Training');
INSERT INTO CertificationTypes Values('Forklift Operations Training OPT-015');
INSERT INTO CertificationTypes Values('Weekend Shutdown & Lockup Checklist Training');
INSERT INTO CertificationTypes Values('Building Alarm System Trainee (Security System)');
INSERT INTO CertificationTypes Values('Shipper/Receiver Training');
INSERT INTO CertificationTypes Values('Use of Quality Fixtures/Gauges for Measuring Product Specs');
INSERT INTO CertificationTypes Values('Paint Dip Tank Mixture Prep. & Viscosity Measurement & Coat Appearance');
INSERT INTO CertificationTypes Values('Crane Operation');
INSERT INTO CertificationTypes Values('Computer Usage');

-- CERTIFICATION POSITION MAPPING
INSERT INTO CertificationPositionMap Values(1, 1);
INSERT INTO CertificationPositionMap Values(4, 1);
INSERT INTO CertificationPositionMap Values(2, 2);
INSERT INTO CertificationPositionMap Values(5, 2);
INSERT INTO CertificationPositionMap Values(10, 3);
INSERT INTO CertificationPositionMap Values(4, 3);
INSERT INTO CertificationPositionMap Values(6, 3);

-- USER ROLES
INSERT INTO Roles Values('IT');
INSERT INTO Roles Values('Management');
INSERT INTO Roles Values('readonly');