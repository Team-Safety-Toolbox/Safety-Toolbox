namespace Safety_Toolbox.Pages;

public partial class DataSaved : ContentPage
{
	int GoBack { get; set; }
    public DataSaved(int goBack)
	{
		InitializeComponent();
        GoBack = goBack;
	}

    private async void OnBackBtnClicked(object sender, EventArgs e)
    {
        int goBack = GoBack; 
        for (var i = 1; i < goBack; i++)
        {
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
        }
        await Navigation.PopAsync();
    }
}