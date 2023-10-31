namespace Safety_Toolbox;

public partial class Toolbox_Talk : ContentPage
{
	public Toolbox_Talk()
	{
		InitializeComponent();
	}
    private async void OnHomeBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}