using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;

namespace Safety_Toolbox;

public partial class Attendance : ContentPage
{
	public Attendance()
	{
        InitializeComponent();
        AttendanceDate.Date = DateTime.Now.Date;
        List<AttendanceData> attendance = getAttendanceData(DateTime.Now);
        collectionView.ItemsSource = attendance;
	}

    void RadioChanged(object sender, CheckedChangedEventArgs e)
    {
        //radio button changed
        //this event will fire twice, once for the new button checked, once for old button unchecked
		//We will just handle the new one being set to true
        if (e.Value) 
		{
			//update this group in SQL (groupname = employeeID)
			//ex) if this button is present, set excused and absent for today's date to 0 and present to 1
		}

    }

    private void AttendanceDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime datetime = AttendanceDate.Date.Date;
        List<AttendanceData> attendance = getAttendanceData(datetime);
        collectionView.ItemsSource = attendance;
    }

    private List<AttendanceData> getAttendanceData(DateTime datetime)
    {
        string connectionString = Constants.connectionString; //TODO: preferences instead of constants
        string query = "SELECT Employees.EmployeeID, Employees.EmployeeFirstName, Employees.EmployeeLastName, Attendance.AttendanceDate, Attendance.Present, Attendance.Excused, Attendance.Absent FROM Employees LEFT JOIN Attendance on Employees.EmployeeID = Attendance.EmployeeID AND Attendance.AttendanceDate = @AttendanceDate";
        List<AttendanceData> attendanceItems = new List<AttendanceData>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                var attendanceDateParam = new SqlParameter("AttendanceDate", datetime);
                command.Parameters.Add(attendanceDateParam);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int empID = reader.GetInt32(0);
                        string empFirstName = reader.GetString(1);
                        string empLastName = reader.GetString(2);
                        DateTime? day = null;
                        if (!reader.IsDBNull(4))
                        {
                            day = reader.GetDateTime(3);
                        }
                        bool present = false;
                        if (!reader.IsDBNull(4)) 
                        {
                            present = reader.GetBoolean(4);
                        }
                        bool excused = false;
                        if (!reader.IsDBNull(5))
                        {
                            excused = reader.GetBoolean(5);
                        }
                        bool absent = false;
                        if (!reader.IsDBNull(6))
                        {
                            absent = reader.GetBoolean(6);
                        }
                        attendanceItems.Add(new AttendanceData() { EmployeeID = empID, EmployeeFirstName = empFirstName, EmployeeLastName = empLastName, AttendanceDate = day, Present = present, Excused = excused, Absent = absent });
                    }
                }
            }
        }
        return attendanceItems;
    }
}