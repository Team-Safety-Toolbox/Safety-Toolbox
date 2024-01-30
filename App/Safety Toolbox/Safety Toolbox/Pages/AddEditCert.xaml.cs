namespace Safety_Toolbox.Pages;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

public partial class AddEditCert : ContentPage
{
    List<string> EmployeeNames { get; set; }
    List<int> EmployeeIds { get; set; }
    List<string> CertificationTypes { get; set; }

    public AddEditCert()
    {
        InitializeComponent();

        EmployeeNames = new List<string>();
        EmployeeIds = new List<int>();
        CertificationTypes = new List<string>();
        getEmployees();

        //getCertifications();
        CertificationTypes.Add("test");

        BindingContext = this;
        employeePicker.ItemsSource = EmployeeNames;
        certificationPicker.ItemsSource = CertificationTypes;
    }


    void OnTrainedCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        trainedDatePicker.IsEnabled = !trainedDatePicker.IsEnabled;
    }

    void OnExpiryCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        expiryDatePicker.IsEnabled = !expiryDatePicker.IsEnabled;
    }
    private async void OnNextStepBtnClicked(object sender, EventArgs e)
    {
        string employee = EmployeeNames[employeePicker.SelectedIndex];
        int empId = EmployeeIds[employeePicker.SelectedIndex];
        string certType = CertificationTypes[certificationPicker.SelectedIndex];
        DateTime? trained = NoTrainDateCheckBox.IsChecked ? null : trainedDatePicker.Date;
        DateTime? expires = NoExpiryDateCheckBox.IsChecked ? null : expiryDatePicker.Date;

        await Navigation.PushAsync(new GetCertFile(empId, employee, certType, trained, expires));
    }

    private void getEmployees()
    {
        string query = "SELECT EmployeeID, EmployeeFirstName, EmployeeLastName FROM Employees";
        List<String> employees = new List<String>();
        List<Int32> employeeIds = new List<Int32>();

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
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
                    }
                }
            }
        }
        EmployeeNames = employees;
        EmployeeIds = employeeIds;
    }

    private void getCertificationTypes()
    {
        string query = "SELECT x FROM x";
        List<String> certs = new List<String>();

        //we need a cert type table

        //using (SqlConnection connection = new SqlConnection(Constants.connectionString))
        //{
        //    using (SqlCommand command = new SqlCommand(query, connection))
        //    {
        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                certs.Add(reader.GetString(0));
        //            }
        //        }
        //    }
        //}

        CertificationTypes = certs;
    }
}