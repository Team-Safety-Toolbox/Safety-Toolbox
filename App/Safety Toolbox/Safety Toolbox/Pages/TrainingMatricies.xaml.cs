using Microsoft.Data.SqlClient;

namespace Safety_Toolbox;

public partial class TrainingMatricies : ContentPage
{
	List <string> Employees { get; set; }
	List <string> Positions { get; set; }
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

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
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
                catch
                {
                    ConnectionFail.IsVisible = true;
                }
            }
        }
        Employees = employees;
    }

    private void getPositions()
    {
        string query = "SELECT PositionName FROM Positions ORDER BY PositionName ASC";
        List<string> positions = new List<string>();

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try { 
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
                catch
                {
                    ConnectionFail.IsVisible = true;
                }
            }
        }
        Positions = positions;
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
    }
}