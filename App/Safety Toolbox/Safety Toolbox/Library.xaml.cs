namespace Safety_Toolbox;

public partial class Library : ContentPage
{
	public Library()
	{
		InitializeComponent();
    }
    private async void OnHomeBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}