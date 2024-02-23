namespace Safety_Toolbox;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();
        if (MainPage.isReadOnly)
        {
            SettingsBtn.IsEnabled = false;
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