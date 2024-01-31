namespace Safety_Toolbox.Pages;

public partial class DataSaved : ContentPage
{
	public DataSaved()
	{
		InitializeComponent();
	}

    private async void OnBackBtnClicked(object sender, EventArgs e)
    {
        //go back to certifications page
        int goBack = 3; //filesaved, getcertfile, addeditcert
        for (var i = 1; i < goBack; i++)
        {
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }
        await Navigation.PopAsync();
    }
}