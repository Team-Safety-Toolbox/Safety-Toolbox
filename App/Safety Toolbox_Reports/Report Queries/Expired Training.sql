DECLARE @CutoffDate AS DATETIME ='4/15/2024'
DECLARE @CertificationName AS VARCHAR(250) = 'Disc Lathe Operation'

SELECT TOP (1000) 
  CertType, TrainedOnDate, ExpiryDate, EmployeeFirstName, EmployeeLastName
  FROM [SafetyToolBox].[dbo].[Certifications]
  LEFT JOIN Employees ON Certifications.EmployeeID = Employees.EmployeeID

  WHERE ExpiryDate <= @CutoffDate AND CertType = @CertificationName -- should have an option for all as well