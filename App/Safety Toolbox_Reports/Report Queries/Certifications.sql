DECLARE @CutoffDate as datetime = '4/30/2024'
SELECT TOP (1000)
  EmployeeLastName, EmployeeFirstName, CertificationName, TrainedOnDate, ExpiryDate
  FROM [SafetyToolBox].[dbo].[Certifications]
  LEFT JOIN Employees ON Certifications.EmployeeID = Employees.EmployeeID
  LEFT JOIN CertificationTypes ON CertificationTypes.CertificationId = Certifications.CertificationID

  WHERE ExpiryDate <= @CutoffDate