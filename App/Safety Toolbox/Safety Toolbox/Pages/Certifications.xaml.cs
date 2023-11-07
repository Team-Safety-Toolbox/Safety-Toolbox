using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;
using System.ComponentModel;

namespace Safety_Toolbox;

public partial class Certifications : ContentPage
{
	public Certifications()
	{
        InitializeComponent();
        List<CertificationData> certs = getCertificationData();
        collectionView.ItemsSource = certs;
    }

    private List<CertificationData> getCertificationData()
    {
        string connectionString = "Server=DESKTOP-0LUMUS9;Database=SafetyToolBox;Persist Security Info=False;Integrated Security=true;Encrypt=False;";
        string query = "SELECT Certifications.EmployeeID, Employees.EmployeeName, Certifications.CertType, Certifications.ExpiryDate FROM Certifications JOIN Employees on Certifications.EmployeeID = Employees.EmployeeID";
        List<CertificationData> certificationList = new List<CertificationData>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int empID = reader.GetInt32(0);
                        string empName = reader.GetString(1);
                        string certType = reader.GetString(2);
                        DateTime expDate = reader.GetDateTime(3);
                        certificationList.Add(new CertificationData(empID, empName, certType, expDate));
                    }
                }
            }
        }
        return certificationList;
    }
}