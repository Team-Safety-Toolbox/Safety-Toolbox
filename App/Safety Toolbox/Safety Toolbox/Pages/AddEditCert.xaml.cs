namespace Safety_Toolbox.Pages;

public partial class AddEditCert : ContentPage
{
	public AddEditCert()
	{
		InitializeComponent();
	}

    private async void OnFilePickerBtnClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions {
            PickerTitle="Pick Certification PDF", 
            FileTypes = FilePickerFileType.Pdf 
        });
        if(file != null)
        {
            //var stream = file.OpenReadAc
        }
    }

    void OnTrainedCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        trainedDatePicker.IsEnabled = !trainedDatePicker.IsEnabled;
    }

    void OnExpiryCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        expiryDatePicker.IsEnabled = !expiryDatePicker.IsEnabled;
    }
}