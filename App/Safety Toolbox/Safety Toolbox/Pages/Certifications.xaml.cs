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
            List<CertificationData> sortedByEmpFirstName = certs.OrderBy(o => o.EmployeeFirstName).ToList();
            collectionView.ItemsSource = sortedByEmpFirstName;
        }
        else if (selectedIndex == 2)
        {
            List<CertificationData> sortedByEmpLastName = certs.OrderBy(o => o.EmployeeLastName).ToList();
            collectionView.ItemsSource = sortedByEmpLastName;
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
        if (searchBar != null){
            List<CertificationData> certs = getCertificationData();
            var matchingFirstNames = certs.Where(certs => certs.EmployeeFirstName.ToLower().Contains(searchBar.Text.ToLower()));
            var matchingLastNames = certs.Where(certs => certs.EmployeeLastName.ToLower().Contains(searchBar.Text.ToLower()));
            var matchingCertifications = certs.Where(certs => certs.CertType.ToLower().Contains(searchBar.Text.ToLower()));

            var allMatches = matchingFirstNames.Union(matchingLastNames);
            allMatches = allMatches.Union(matchingCertifications);

            collectionView.ItemsSource = allMatches;
        }
    }

    void OnExportBtnClicked(object sender, EventArgs e)
    {
        //Generate report here!
    }

    void OnReportSettingsBtnClicked(object sender, EventArgs e)
    {
        //Report settings
    }
    private List<CertificationData> getCertificationData()
    {
        string query = "SELECT Certifications.EmployeeID, Employees.EmployeeFirstName, Employees.EmployeeLastName, Certifications.CertType, Certifications.TrainedOnDate, Certifications.ExpiryDate FROM Certifications JOIN Employees on Certifications.EmployeeID = Employees.EmployeeID";
        List<CertificationData> certificationList = new List<CertificationData>();

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int empID = reader.GetInt32(0);
                        string empFirstName = reader.GetString(1);
                        string empLastName = reader.GetString(2);
                        string certType = reader.GetString(3);
                        DateTime trainedOnDate = reader.GetDateTime(4);
                        DateTime expDate = reader.GetDateTime(5);
                        certificationList.Add(new CertificationData(empID, empFirstName, empLastName, certType, trainedOnDate, expDate));
                    }
                }
            }
        }
        return certificationList;
    }
}