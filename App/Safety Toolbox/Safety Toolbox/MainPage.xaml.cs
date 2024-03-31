﻿using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Types;
using System.Text;

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
                byte[] saltDB = null;
                byte[] hashedPasswordDB = null;
                String query = "SELECT TOP 1 Username, Salt, HashedPassword, RoleName FROM Users LEFT JOIN Roles ON Users.RoleID = Roles.RoleID WHERE Username = '" + Username + "'";

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
                                    saltDB = (byte[])reader["Salt"];
                                    hashedPasswordDB = (byte[])reader["HashedPassword"];
                                    Role = (String)reader["RoleName"];
                                }
                            }
                        }
                        catch{
                            //alert gets displayed later
                        }
                    }
                }

                Password processPassword = new Password(Password, saltDB);
                var hashPassword = processPassword.Hash();

                if (usernameDB.Equals(Username) && hashedPasswordDB.SequenceEqual(hashPassword))
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
                    //clear these so they'll be empty when user logs out
                    UsernameEntry.Text = null;
                    PasswordEntry.Text = null;
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