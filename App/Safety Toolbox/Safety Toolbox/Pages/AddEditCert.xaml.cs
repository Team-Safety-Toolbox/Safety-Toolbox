namespace Safety_Toolbox.Pages;

using CommunityToolkit.Maui.Converters;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;

public partial class AddEditCert : ContentPage
{
    List<string> EmployeeFullNames { get; set; } //nessecary to use as a source for dropdown
    List<string> EmployeeFirstNames { get; set; } //separate first names are needed
    List<string> EmployeeLastNames { get; set; } //separate last names are needed
    List<int> EmployeeIds { get; set; }
    List<string> CertificationTypes { get; set; }

    public AddEditCert(CertificationData optionalDataToEdit = null)
    {
        InitializeComponent();

        trainedDatePicker.MaximumDate = DateTime.Now.Date;
        expiryDatePicker.MinimumDate = trainedDatePicker.Date;

        EmployeeFullNames = new List<string>();
        EmployeeFirstNames = new List<string>();
        EmployeeLastNames = new List<string>();
        EmployeeIds = new List<int>();
        CertificationTypes = new List<string>();
        getEmployees();
        getCertificationTypes();

        BindingContext = this;
        employeePicker.ItemsSource = EmployeeFullNames;
        certificationPicker.ItemsSource = CertificationTypes;

        if (optionalDataToEdit != null)
        {
            employeePicker.IsEnabled = false;
            certificationPicker.IsEnabled = false;

            employeePicker.SelectedIndex = EmployeeFullNames.IndexOf(optionalDataToEdit.EmployeeFirstName + " " + optionalDataToEdit.EmployeeLastName);
            certificationPicker.SelectedIndex = CertificationTypes.IndexOf(optionalDataToEdit.CertType);

            if (optionalDataToEdit.TrainedOnDate != null)
                trainedDatePicker.Date = (DateTime)optionalDataToEdit.TrainedOnDate;
            else
                NoTrainDateCheckBox.IsChecked = true;

            if (optionalDataToEdit.ExpiryDate != null)
                expiryDatePicker.Date = (DateTime)optionalDataToEdit.ExpiryDate;
            else
                NoExpiryDateCheckBox.IsChecked = true;
        }
    }

    private void OnTrainedDatePickerDateSelected(object sender, DateChangedEventArgs e)
    {
        expiryDatePicker.MinimumDate = trainedDatePicker.Date;
    }

    void OnTrainedCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        trainedDatePicker.IsEnabled = !trainedDatePicker.IsEnabled;

        if(trainedDatePicker.IsEnabled)
        {
            expiryDatePicker.MinimumDate = trainedDatePicker.Date;
        }
        else
        {
            expiryDatePicker.MinimumDate = DateTime.MinValue;
        }
    }

    void OnExpiryCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        expiryDatePicker.IsEnabled = !expiryDatePicker.IsEnabled;
    }
    private async void OnNextStepBtnClicked(object sender, EventArgs e)
    {
        EmployeeErrorText.IsVisible = false;
        CertTypeErrorText.IsVisible = false;

        if (employeePicker.SelectedIndex == -1)
        {
            EmployeeErrorText.IsVisible = true;
            EmployeeErrorText.Text = "You must pick an employee.";
        }

        if (certificationPicker.SelectedIndex == -1)
        {
            CertTypeErrorText.IsVisible = true;
            CertTypeErrorText.Text = "You must pick a certification type.";
        }

        if(employeePicker.SelectedIndex != -1 && certificationPicker.SelectedIndex != -1)
        {
            string employeeFirstName = EmployeeFirstNames[employeePicker.SelectedIndex];
            string employeeLastName = EmployeeLastNames[employeePicker.SelectedIndex];
            int empId = EmployeeIds[employeePicker.SelectedIndex];
            string certType = CertificationTypes[certificationPicker.SelectedIndex];
            DateTime? trained = NoTrainDateCheckBox.IsChecked ? null : trainedDatePicker.Date;
            DateTime? expires = NoExpiryDateCheckBox.IsChecked ? null : expiryDatePicker.Date;

            await Navigation.PushAsync(new GetCertFile(new CertificationData(empId, employeeFirstName, employeeLastName, certType, trained, expires)));
        }
    }

    async private void getEmployees()
    {
        string query = "SELECT EmployeeID, EmployeeFirstName, EmployeeLastName FROM Employees ORDER BY EmployeeFirstName ASC";
        List<String> employees = new List<String>();
        List<String> firstNames = new List<String>();
        List<String> lastNames = new List<String>();
        List<Int32> employeeIds = new List<Int32>();

        try {
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeIds.Add(reader.GetInt32(0));

                            string empFirstName = reader.GetString(1);
                            string empLastName = reader.GetString(2);
                            employees.Add(empFirstName+" "+empLastName);
                            firstNames.Add(empFirstName);
                            lastNames.Add(empLastName);
                        }
                    }
                }
            }
        }
        catch
        {
            await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
        }
        EmployeeFullNames = employees;
        EmployeeFirstNames = firstNames;
        EmployeeLastNames = lastNames;
        EmployeeIds = employeeIds;
    }

    async private void getCertificationTypes()
    {
        string query = "SELECT CertificationName FROM CertificationTypes ORDER BY CertificationName ASC";
        List<String> certs = new List<String>();

        try { 
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            certs.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }
        catch
        {
            await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
        }

        CertificationTypes = certs;
    }
}