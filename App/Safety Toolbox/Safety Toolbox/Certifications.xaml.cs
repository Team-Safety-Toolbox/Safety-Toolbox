namespace Safety_Toolbox;

public partial class Certifications : ContentPage
{
	public Certifications()
	{
		InitializeComponent();
    }
    private async void OnHomeBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}