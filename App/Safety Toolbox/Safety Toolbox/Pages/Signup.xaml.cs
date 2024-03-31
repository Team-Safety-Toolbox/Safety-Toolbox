using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;

namespace Safety_Toolbox;

public partial class Signup : ContentPage
{
    private String Email;
    private String Username;
	private String Password;
	private String ConfirmPassword;
	
	public Signup()
	{
		InitializeComponent();
	}

	async private void saveSignupInformation()
	{
		int readOnlyID = -1;
		string query = "SELECT RoleID FROM Roles WHERE RoleName = 'readonly';";

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
							readOnlyID = reader.GetInt32(0);
						}
					}
                }
                catch
                {
                    await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
                }
            }
        }

        query = "INSERT INTO Users (Email, Username, Password, RoleID) Values (@Email, @Username, @Password, @ReadOnlyID);";

		// there's an issue here where these aren't saving
        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
		{
            using (SqlCommand command = new SqlCommand(query, connection))
            {
				try {
					connection.Open();

					command.Parameters.AddWithValue("Email", Email);
					command.Parameters.AddWithValue("Username", Username);
					command.Parameters.AddWithValue("Password", Password);
					command.Parameters.AddWithValue("ConfirmPassword", ConfirmPassword);
					command.Parameters.AddWithValue("ReadOnlyID", readOnlyID);

					var results = command.ExecuteReader();
                }
                catch
                {
                    await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
                }
            }
        }
    }

    private async void OnSignUpBtnClicked(object sender, EventArgs e)
    {
		Email = EmailEntry.Text;
		Username = UsernameEntry.Text;
		Password = PasswordEntry.Text;
		ConfirmPassword = ConfirmPasswordEntry.Text;

		if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Username)
			&& !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword) && Password.Equals(ConfirmPassword))
		{
			// should also make sure the email and the username will not be duplicates
			saveSignupInformation();
            await Navigation.PushAsync(new MainPage());
        }
		else
            await DisplayAlert("Invalid Information", "Please ensure all fields are filled in and the passwords match.", "OK");
    }
}