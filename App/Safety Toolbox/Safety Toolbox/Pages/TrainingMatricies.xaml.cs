using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using System.Xml;

namespace Safety_Toolbox;

public partial class TrainingMatricies : ContentPage
{
	List <string> Employees { get; set; }
	List <string> Positions { get; set; }
    List<string> EmployeeTraining { get; set; }
    List<DateTime?> EmployeeTrainingDates { get; set; }
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

    private void getEmployeeTraining(string employeeName, string positionName)
    {
        string query = "SELECT CertificationName, TrainedOnDate FROM Certifications " +
            "LEFT JOIN CertificationTypes ON Certifications.CertificationID = CertificationTypes.CertificationId " +
            "LEFT JOIN Employees ON Certifications.EmployeeID = Employees.EmployeeID " +
            "WHERE (Employees.EmployeeFirstName + ' ' + Employees.EmployeeLastName) = '" + employeeName + "' " +
            "ORDER BY CertificationName ASC;";
        List<string> employeeTraining = new List<string>();
        List<DateTime?> trainDates = new List<DateTime?>();

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
                            DateTime? trainDate = !reader.IsDBNull(1) ? reader.GetDateTime(1) : null;
                            employeeTraining.Add(certName);
                            trainDates.Add(trainDate);
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
        EmployeeTrainingDates = trainDates;
    }

    private void getPositionTraining(string positionName)
    {
        string query = "SELECT CertificationName FROM CertificationPositionMap " +
            "LEFT JOIN CertificationTypes ON CertificationPositionMap.CertificationID = CertificationTypes.CertificationID " +
            "LEFT JOIN Positions ON CertificationPositionMap.PositionID = Positions.PositionID " +
            "WHERE Positions.PositionName = '" + positionName + "' ORDER BY CertificationName ASC;";
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

    private String getEmployeePositionTitle(string employeeName)
    {
        string query = "SELECT TOP 1 PositionName FROM Employees " +
            "LEFT JOIN Positions ON Employees.PositionID = Positions.PositionID " +
            "WHERE (EmployeeFirstName + ' ' + EmployeeLastName) = '" + employeeName + "';";
        string selectedEmployeePosition = "";

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
                            selectedEmployeePosition = reader.GetString(0);
                        }
                    }
                }
            }
        }
        catch
        {
            ConnectionFail.IsVisible = true;
        }

        return selectedEmployeePosition;
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
        headerStackLayout.Children.Clear();
        employeeStackLayout.Children.Clear();
        EmployeeTraining = null;
        PositionTraining = null;

        if (employeePicker.SelectedItem != null && positionPicker.SelectedItem != null)
        {
            // show what training the employee has compared to the position that was manually selected
            getEmployeeTraining(employeePicker.SelectedItem.ToString(), positionPicker.SelectedItem.ToString());
            getPositionTraining(positionPicker.SelectedItem.ToString());
        }
        else if (employeePicker.SelectedItem != null)
        {
            // show the training they have vs the training they need for the position they have
            getEmployeeTraining(employeePicker.SelectedItem.ToString(), getEmployeePositionTitle(employeePicker.SelectedItem.ToString()));
            getPositionTraining(getEmployeePositionTitle(employeePicker.SelectedItem.ToString()));
        }
        else if (positionPicker.SelectedItem != null)
        {
            // show the required training for the position
            getPositionTraining(positionPicker.SelectedItem.ToString());
        }
        
        if (PositionTraining != null)
            addCertLabels(PositionTraining.Count());
        if (EmployeeTraining != null)
            addCompletedTrainingLabels();
    }

    private void addCertLabels(int totalCerts)
    {
        for (int i = 0; i < totalCerts; i++)
        {
            headerStackLayout.Children.Add(new Label
            {
                Text = PositionTraining[i].ToString(),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });
        }
    }

    private void addCompletedTrainingLabels()
    {
        foreach (String trainingName in PositionTraining)
        {
            int trainIndex = EmployeeTraining.IndexOf(trainingName);
            if (trainIndex != -1)
            {
                string dateTrained = EmployeeTrainingDates[trainIndex] != null ? EmployeeTrainingDates[trainIndex].Value.ToShortDateString() : "No date available";
                employeeStackLayout.Children.Add(new Label
                {
                    Text = "Trained On: " + dateTrained,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                });
            }
            else
            {
                employeeStackLayout.Children.Add(new Label
                {
                    Text = "Training Record Not Found",
                    TextColor = Color.FromArgb("FF0000"),
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                });
            }
        }
    }
}