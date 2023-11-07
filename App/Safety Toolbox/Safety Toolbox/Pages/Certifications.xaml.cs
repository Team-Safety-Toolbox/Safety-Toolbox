using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;

namespace Safety_Toolbox;

public partial class Certifications : ContentPage
{
	public Certifications()
	{
        InitializeComponent();
        List<CertificationData> certs = getCertificationData();
        List<CertificationData> sortedByExpDate = certs.OrderBy(o=>o.ExpiryDate).ToList();
        collectionView.ItemsSource = sortedByExpDate;

        var sortByList = new List<string>();
        sortByList.Add("Expiry Date");
        sortByList.Add("Employee First Name");
        sortByList.Add("Employee Last Name");
        sortByList.Add("Certification");

        BindingContext = this;
        sortPicker.ItemsSource = sortByList;
        sortPicker.SelectedIndex = 0;

    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        List<CertificationData> certs = getCertificationData();

        if (selectedIndex == 0)
        {
            List<CertificationData> sortedByExpDate = certs.OrderBy(o => o.ExpiryDate).ToList();
            collectionView.ItemsSource = sortedByExpDate;
        }
        else if (selectedIndex == 1)
        {
            List<CertificationData> sortedByEmpFirstName = certs.OrderBy(o => o.EmployeeName).ToList();
            collectionView.ItemsSource = sortedByEmpFirstName;
        }
        else if (selectedIndex == 2)
        {
            //List<CertificationData> sortedByEmpLastName = certs.OrderBy(o=>o.EmployeeLastName).ToList();
            //collectionView.ItemsSource = sortedByEmpLastName;
        }
        else if (selectedIndex == 3)
        {
            List<CertificationData> sortedByCertType = certs.OrderBy(o => o.CertType).ToList();
            collectionView.ItemsSource = sortedByCertType;
        }
    }

    void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        //do a new query searching for searchBar.Text
    }

    void OnExportBtnClicked(object sender, EventArgs e)
    {
        //Generate report here!
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