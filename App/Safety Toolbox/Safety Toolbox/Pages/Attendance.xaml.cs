using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;

namespace Safety_Toolbox;

public partial class Attendance : ContentPage
{
	public Attendance()
	{
        //List<AttendanceData> attendance = getAttendanceData();

        InitializeComponent();
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
    private List<AttendanceData> getAttendanceData()
    {
        //TODO - save connectionString somewhere else
        string connectionString = "Server=DESKTOP-0LUMUS9;Database=SafetyToolBox;Persist Security Info=False;Integrated Security=true;Encrypt=False;";
        string query = "SELECT * FROM Attendance";
        List<AttendanceData> attendanceItems = new List<AttendanceData>();

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
                        DateTime day = reader.GetDateTime(2);
                        bool present = reader.GetBoolean(3);
                        bool excused = reader.GetBoolean(4);
                        bool absent = reader.GetBoolean(5);
                        attendanceItems.Add(new AttendanceData() { EmployeeID = empID, EmployeeName = empName, AttendanceDate = day, Present = present, Excused = excused, Absent = absent });
                    }
                }
            }
        }
        return attendanceItems;
    }
}