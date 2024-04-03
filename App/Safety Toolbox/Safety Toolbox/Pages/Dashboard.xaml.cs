namespace Safety_Toolbox;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();

        if (!MainPage.IsITAccount) //if it isn't an IT account, no access to settings
        {
            SettingsBtn.IsEnabled = false;
        }

        if (MainPage.IsSetupAccount) //setup account ONLY has access to settings
        {
            TBTBtn.IsEnabled = false;
            CertsBtn.IsEnabled = false;
            LibBtn.IsEnabled = false;
            SetupAccountWarning.IsVisible = true;
            SettingsBtn.IsEnabled = true;
        }

        checkConnection();
    }

    private async void checkConnection()
    {
        if (Preferences.Default.Get("DBConn", "Not Found") == "Not Found")
        {
            TBTBtn.IsEnabled = false;
            CertsBtn.IsEnabled = false;
            LibBtn.IsEnabled = false;
            await DisplayAlert("Database Connection", "You are not yet connected to a database. Change this in settings.", "OK");
        }
    }

    private async void OnSettingsBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Settings());
    }
    private async void OnTBTBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Toolbox_Talk());
    }
    private async void OnCertBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Certifications());
    }
    private async void OnLibBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Library());
    }
}