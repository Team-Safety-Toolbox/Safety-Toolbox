using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Safety_Toolbox.Types;
using System.Security.Cryptography;

namespace Safety_Toolbox;

public partial class Signup : ContentPage
{
    private String Email;
    private String Username;
	private byte[] Salt;
	private byte[] HashedPassword;
	
	public Signup()
	{
		InitializeComponent();
	}

	async private void saveSignupInformation()
	{
        int matchCount = -1;
        string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email OR Username = @Username;";

        try
        {
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("Email", Email);
                    command.Parameters.AddWithValue("Username", Username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            matchCount = reader.GetInt32(0);
                        }
                    }
                }
            }
        }
        catch
        {
            await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
        }

        if(matchCount == 0) //email and username will both be unique
        {
            int readOnlyID = -1;
		    query = "SELECT RoleID FROM Roles WHERE RoleName = 'readonly';";

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
							    readOnlyID = reader.GetInt32(0);
						    }
					    }
                    }
                }
            }
            catch
            {
                await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
            }

            query = "INSERT INTO Users (Email, Username, Salt, HashedPassword, RoleID) Values (@Email, @Username, @Salt, @HashedPassword, @ReadOnlyID);";
            
            try {
                using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
		        {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
					    connection.Open();

					    command.Parameters.AddWithValue("Email", Email);
					    command.Parameters.AddWithValue("Username", Username);
                        command.Parameters.AddWithValue("Salt", Salt);
                        command.Parameters.AddWithValue("HashedPassword", HashedPassword);
					    command.Parameters.AddWithValue("ReadOnlyID", readOnlyID);

					    var results = command.ExecuteReader();
                    }
                }
            }
            catch
            {
                await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Email/Username", "This email or username is already taken.", "OK");
        }
    }

    private async void OnSignUpBtnClicked(object sender, EventArgs e)
    {
		Email = EmailEntry.Text;
		Username = UsernameEntry.Text;
		var password = PasswordEntry.Text;
		var confirmPassword = ConfirmPasswordEntry.Text;

		if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Username)
			&& !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword) && password.Equals(confirmPassword))
		{
			Password processPassword = new Password(password);
			HashedPassword = processPassword.Hash();
			Salt = processPassword.Salt;

			saveSignupInformation();
            await Navigation.PushAsync(new MainPage());
        }
		else
            await DisplayAlert("Invalid Information", "Please ensure all fields are filled in and the passwords match.", "OK");
    }
}