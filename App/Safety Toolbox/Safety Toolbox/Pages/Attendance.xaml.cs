using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;

namespace Safety_Toolbox;

public partial class Attendance : ContentPage
{
	public Attendance()
	{
        InitializeComponent();

        if (MainPage.isReadOnly)
        {
            EditorView.IsVisible = false;
            NonEditorView.IsVisible = true;
        }
        else
        {
            EditorView.IsVisible = true;
            NonEditorView.IsVisible = false;
        }

        AttendanceDate.Date = DateTime.Now.Date;
        AttendanceDate.MaximumDate = DateTime.Now.Date;
        getAttendanceData(DateTime.Now.Date);
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        ScrollAttendance.HeightRequest = 0.7 * height;
    }

    async void RadioChanged(object sender, CheckedChangedEventArgs e)
    {
        //radio button changed
        //this event will fire twice, once for the new button checked, once for old button unchecked
		//We will just handle the new one being set to true
        if (e.Value) 
		{
            int empID = Int32.Parse(((Microsoft.Maui.Controls.RadioButton)sender).GroupName);
            DateTime attendanceDate = AttendanceDate.Date.Date;
            bool present = false;
            bool excused = false;
            bool absent = false;
            if (((Microsoft.Maui.Controls.RadioButton)sender).Content.ToString() == "Present") {
                present = true;
            }
            else if (((Microsoft.Maui.Controls.RadioButton)sender).Content.ToString() == "Excused") {
                excused = true;
            }
            else if (((Microsoft.Maui.Controls.RadioButton)sender).Content.ToString() == "Absent") {
                absent = true;
            }

            string connectionString = Constants.connectionString; //TODO: preferences instead of constants

            string queryInsert = "INSERT INTO Attendance (EmployeeID, AttendanceDate, Present, Excused, Absent) VALUES (@EmpId, @AttendanceDate, @Present, @Excused, @Absent);";
            string queryUpdate = "UPDATE Attendance SET Present = @Present, Excused = @Excused, Absent = @Absent WHERE EmployeeID = @EmpId AND AttendanceDate = @AttendanceDate;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    try
                    {
                        using (SqlCommand command = new SqlCommand(queryInsert, connection))
                        {
                            var empIdParam = new SqlParameter("EmpId", empID);
                            var attendanceDateParam = new SqlParameter("AttendanceDate", attendanceDate);
                            var presentParam = new SqlParameter("Present", present);
                            var excusedParam = new SqlParameter("Excused", excused);
                            var absentParam = new SqlParameter("Absent", absent);

                            command.Parameters.Add(empIdParam);
                            command.Parameters.Add(attendanceDateParam);
                            command.Parameters.Add(presentParam);
                            command.Parameters.Add(excusedParam);
                            command.Parameters.Add(absentParam);

                            var results = command.ExecuteReader();
                        }
                    }
                    catch
                    {
                        using (SqlCommand command = new SqlCommand(queryUpdate, connection))
                        {
                            var empIdParam = new SqlParameter("EmpId", empID);
                            var attendanceDateParam = new SqlParameter("AttendanceDate", attendanceDate);
                            var presentParam = new SqlParameter("Present", present);
                            var excusedParam = new SqlParameter("Excused", excused);
                            var absentParam = new SqlParameter("Absent", absent);

                            command.Parameters.Add(empIdParam);
                            command.Parameters.Add(attendanceDateParam);
                            command.Parameters.Add(presentParam);
                            command.Parameters.Add(excusedParam);
                            command.Parameters.Add(absentParam);

                            var results = command.ExecuteReader();
                        }
                    }
                }
                catch
                {
                    await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
                }

            }
        }

    }

    private void AttendanceDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime datetime = AttendanceDate.Date.Date;
        getAttendanceData(datetime);
    }

    private void getAttendanceData(DateTime datetime)
    {
        string connectionString = Constants.connectionString; //TODO: preferences instead of constants
        string query = "SELECT Employees.EmployeeID, Employees.EmployeeFirstName, Employees.EmployeeLastName, Attendance.AttendanceDate, Attendance.Present, Attendance.Excused, Attendance.Absent FROM Employees LEFT JOIN Attendance on Employees.EmployeeID = Attendance.EmployeeID AND Attendance.AttendanceDate = @AttendanceDate";
        List<AttendanceData> attendanceItems = new List<AttendanceData>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
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
                catch
                {
                    ConnectionFail.IsVisible = true;
                }
            }
        }

        collectionView.ItemsSource = attendanceItems;
    }
}