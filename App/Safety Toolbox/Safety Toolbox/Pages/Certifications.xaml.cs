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
        sortByList.Add("Train Date");
        sortByList.Add("Employee First Name");
        sortByList.Add("Employee Last Name");
        sortByList.Add("Certification");

        BindingContext = this;
        sortPicker.ItemsSource = sortByList;
        sortPicker.SelectedIndex = 0;

        updateData();

        // configure the readonly controls
        if (MainPage.IsReadOnly)
        {
            AddEditCertBtn.IsEnabled = false;
        }
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
            List<CertificationData> sortedByTrainDate = certs.OrderBy(o => o.TrainedOnDate.HasValue ? o.TrainedOnDate : DateTime.MaxValue).ToList();
            collectionView.ItemsSource = sortedByTrainDate;
        }
        else if (selectedIndex == 2)
        {
            List<CertificationData> sortedByEmpFirstName = certs.OrderBy(o => o.EmployeeFirstName).ToList();
            collectionView.ItemsSource = sortedByEmpFirstName;
        }
        else if (selectedIndex == 3)
        {
            List<CertificationData> sortedByEmpLastName = certs.OrderBy(o => o.EmployeeLastName).ToList();
            collectionView.ItemsSource = sortedByEmpLastName;
        }
        else if (selectedIndex == 4)
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

    async void OnExportBtnClicked(object sender, EventArgs e)
    {
        try
        {
            // might need to adjust what this report is called and/or be able to run a bunch of different reports
            //Uri uri = new Uri(Preferences.Default.Get("ReportServerURL", "Not Found") + "/ExpiredCertifications&CutoffDate=val&CertificationName=val");
            Uri uri = new Uri(Preferences.Default.Get("ReportServerURL", "Not Found") + "/ExpiredCertifications");
            //^^now using preferences instead of constants, need some error catching in the case of "Not Found"
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch
        {
            await DisplayAlert("Unexpected Error Occured", "An unexpected error occured.", "OK");
        }
    }

    async void OnViewBtnClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Preferences.Default.Get("CertFilePath", "Not Found"), filename);

        if (File.Exists(fullFilePath)){
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    private async void OnAddEditCertBtnClicked(object sender, EventArgs e)
    {
        //Add or edit a certification
        await Navigation.PushAsync(new AddEditCert());
    }
    private List<CertificationData> getCertificationData()
    {
        string query = "SELECT Certifications.EmployeeID, Employees.EmployeeFirstName, Employees.EmployeeLastName, CertificationTypes.CertificationName, Certifications.TrainedOnDate, Certifications.ExpiryDate FROM Certifications JOIN Employees on Certifications.EmployeeID = Employees.EmployeeID JOIN CertificationTypes on Certifications.CertificationID = CertificationTypes.CertificationID";
        List<CertificationData> certificationList = new List<CertificationData>();
        
        try
        {
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
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
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }
        return certificationList;
    }

    private async void OnEditBtnClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var selectedItem = (CertificationData)button.CommandParameter;

        await Navigation.PushAsync(new AddEditCert(selectedItem));
    }
}