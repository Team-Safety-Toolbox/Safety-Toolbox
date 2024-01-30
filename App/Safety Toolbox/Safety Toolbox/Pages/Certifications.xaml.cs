using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Pages;
using Safety_Toolbox.Types;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;

namespace Safety_Toolbox;

public partial class Certifications : ContentPage
{
    bool Refresh {  get; set; }
	public Certifications()
	{
        InitializeComponent();
        
        var sortByList = new List<string>();
        sortByList.Add("Expiry Date");
        sortByList.Add("Employee First Name");
        sortByList.Add("Employee Last Name");
        sortByList.Add("Certification");

        BindingContext = this;
        sortPicker.ItemsSource = sortByList;
        sortPicker.SelectedIndex = 0;

        updateData();
    }

    private void updateData()
    {
        List<CertificationData> certs = getCertificationData();
        updateSort(sortPicker.SelectedIndex, certs);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        MainDataRow.Height = 0.7 * height;
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        List<CertificationData> certs = getCertificationData();

        updateSort(selectedIndex, certs);
    }

    void updateSort(int selectedIndex, List<CertificationData> certs)
    {
        if (selectedIndex == 0)
        {
            List<CertificationData> sortedByExpDate = certs.OrderBy(o => o.ExpiryDate.HasValue ? o.ExpiryDate : DateTime.MaxValue).ToList();
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

    void OnRefreshBtnClicked(object sender, EventArgs e)
    {
        updateData();
    }

    void OnExportBtnClicked(object sender, EventArgs e)
    {
        //Generate report here!
    }

    async void OnViewBtnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Constants.certificationFilePath, filename);

        if (File.Exists(fullFilePath)){
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    private async void OnReportSettingsBtnClicked(object sender, EventArgs e)
    {
        //Report settings
        await Navigation.PushAsync(new ReportSettings());
    }

    private async void OnAddEditCertBtnClicked(object sender, EventArgs e)
    {
        //Add or edit a certification
        await Navigation.PushAsync(new AddEditCert());
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
                        DateTime? trainedOnDate = null;
                        if (!reader.IsDBNull(4))
                        {
                            trainedOnDate = reader.GetDateTime(4);
                        }
                        DateTime? expDate = null;
                        if (!reader.IsDBNull(5))
                        { 
                            expDate = reader.GetDateTime(5);
                        }
                        
                        certificationList.Add(new CertificationData(empID, empFirstName, empLastName, certType, trainedOnDate, expDate));
                    }
                }
            }
        }
        return certificationList;
    }
}