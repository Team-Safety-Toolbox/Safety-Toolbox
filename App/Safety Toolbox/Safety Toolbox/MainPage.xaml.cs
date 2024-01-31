using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;

namespace Safety_Toolbox
{

    public partial class MainPage : ContentPage
    {
        private String Username;
        private String Password;
        private String Role;
        public static Boolean isReadOnly { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private Boolean validateUser()
        {
            String usernameDB = "";
            String passwordDB = "";
            String query = "SELECT TOP 1 Username, Password, RoleName FROM Users LEFT JOIN Roles ON Users.RoleID = Roles.RoleID WHERE Username = '" + Username + "' AND Password = '" + Password + "'";

            using (SqlConnection connection = new SqlConnection(Constants.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernameDB = (String)reader["Username"];
                            passwordDB = (String)reader["Password"];
                            Role = (String)reader["RoleName"];
                        }
                    }
                }
            }


            if (usernameDB.Equals(Username) && passwordDB.Equals(Password))
            {
                setReadOnlyStatus(Role.Equals("readonly"));
                return true;
            }
            else
                return false;
        }

        public static void setReadOnlyStatus(Boolean status)
        {
            isReadOnly = status;
        }

        private async void OnLoginBtnClicked(object sender, EventArgs e)
        {
            Username = UsernameEntry.Text;
            Password = PasswordEntry.Text;

            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && validateUser())
            {
                await Navigation.PushAsync(new Dashboard());
            }
            else
            {
                await DisplayAlert("Invalid Login", "Please enter valid credentials.", "OK");
            }
        }

        private async void OnSignupBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Signup());
        }
    }
}