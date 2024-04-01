using Microsoft.Data.SqlClient;
using System.Xml;

namespace Safety_Toolbox;

public partial class TrainingMatricies : ContentPage
{
	List <string> Employees { get; set; }
	List <string> Positions { get; set; }
    List<string> EmployeeTraining { get; set; }
    List<string> PositionTraining { get; set; }
    public TrainingMatricies()
	{
		InitializeComponent();

		Employees = new List<string>();
		Positions = new List<string>();

		getEmployees();
		getPositions();

        BindingContext = this;
        employeePicker.ItemsSource = Employees;
        positionPicker.ItemsSource = Positions;
	}

    private void getEmployees()
    {
        string query = "SELECT EmployeeID, EmployeeFirstName, EmployeeLastName FROM Employees ORDER BY EmployeeFirstName ASC";
        List<String> employees = new List<String>();
        
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
                            string empFirstName = reader.GetString(1);
                            string empLastName = reader.GetString(2);
                            employees.Add(empFirstName + " " + empLastName);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }

        Employees = employees;
    }

    private void getPositions()
    {
        string query = "SELECT PositionName FROM Positions ORDER BY PositionName ASC";
        List<string> positions = new List<string>();

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
                            string pos = reader.GetString(0);
                            positions.Add(pos);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }
        Positions = positions;
    }

    private void getEmployeeTraining(string employeeName)
    {
        string query = "SELECT CertificationName FROM Certifications " +
            "LEFT JOIN CertificationTypes ON Certifications.CertificationID = CertificationTypes.CertificationId " +
            "LEFT JOIN Employees on Certifications.EmployeeID = Employees.EmployeeID " +
            "WHERE (Employees.EmployeeFirstName + ' ' + Employees.EmployeeLastName) = '" + employeeName + "';";
        List<string> employeeTraining = new List<string>();

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
                            string certName = reader.GetString(0);
                            employeeTraining.Add(certName);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }

        EmployeeTraining = employeeTraining;
    }

    private void getPositionTraining(string positionName)
    {
        string query = "SELECT CertificationName FROM CertificationPositionMap " +
            "LEFT JOIN CertificationTypes ON CertificationPositionMap.CertificationID = CertificationTypes.CertificationID " +
            "LEFT JOIN Positions ON CertificationPositionMap.PositionID = Positions.PositionID " +
            "WHERE Positions.PositionName = '" + positionName + "';";
        List<string> positionTraining = new List<string>();

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
                            string certName = reader.GetString(0);
                            positionTraining.Add(certName);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }

        PositionTraining = positionTraining;
    }

    private void employeeDeselectBtn_Clicked(object sender, EventArgs e)
    {
        employeePicker.SelectedItem = null;
    }

    private void positionDeselectBtn_Clicked(object sender, EventArgs e)
    {
        positionPicker.SelectedItem = null;
    }

    private void generateBtn_Clicked(object sender, EventArgs e)
    {
        // generate the training matrix
        // position training across the top
        // empl training across the side

        if (employeePicker.SelectedItem != null && positionPicker.SelectedItem != null)
        {
            // show what training the employee has compared to the position
            getEmployeeTraining(employeePicker.SelectedItem.ToString());
            getPositionTraining(positionPicker.SelectedItem.ToString());
        }
        else if (employeePicker.SelectedItem != null)
        {
            // show vs the position they have
            // this means position needs to be added to the employee table
        }
        else if (positionPicker.SelectedItem != null)
        {
            // show the required training for the position
            getPositionTraining(positionPicker.SelectedItem.ToString());
        }
        

        addCertLabels(PositionTraining.Count());
    }

    private void addCertLabels(int totalCerts)
    {
        headerStackLayout.Children.Clear();

        for (int i = 0; i < totalCerts; i++)
        {
            headerStackLayout.Children.Add(new Label
            {
                Text = PositionTraining[i].ToString(),
                HorizontalOptions = LayoutOptions.StartAndExpand
            });
        }
    }
}