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
        public static Boolean signUpEnabled { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private Boolean validateUser()
        {
            try
            {
                String usernameDB = "";
                String passwordDB = "";
                String query = "SELECT TOP 1 Username, Password, RoleName FROM Users LEFT JOIN Roles ON Users.RoleID = Roles.RoleID WHERE Username = '" + Username + "' AND Password = '" + Password + "'";

                using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
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
                                    usernameDB = (String)reader["Username"];
                                    passwordDB = (String)reader["Password"];
                                    Role = (String)reader["RoleName"];
                                }
                            }
                        }
                        catch{
                            //alert gets displayed later
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
            catch
            {
                return false;
            }
        }

        public static void setReadOnlyStatus(Boolean status)
        {
            isReadOnly = status;
        }

        public static void setSignUpEnabled(Boolean status)
        {
            // this is being annoying and not working
            signUpEnabled = status;
            //setSignUpVisibility(status);
        }

        public void setSignUpVisibility(Boolean status)
        {
            SignupBtn.IsVisible = status;
        }

        private async void OnLoginBtnClicked(object sender, EventArgs e)
        {
            Username = UsernameEntry.Text;
            Password = PasswordEntry.Text;

            //TODO: decide if admin account is always accessible, or only when no database connection
            if (Username == "admin" && Password == "Adm1nU$er")
            {
                await Navigation.PushAsync(new Dashboard());
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && validateUser())
                {
                    await Navigation.PushAsync(new Dashboard());
                }
                else
                {
                    await DisplayAlert("Invalid Login", "Please enter valid credentials.", "OK");
                }
            }

            
        }

        private async void OnSignupBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Signup());
        }
    }
}