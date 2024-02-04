namespace Safety_Toolbox.Pages;

public partial class AddLibraryFile : ContentPage
{
	public AddLibraryFile()
	{
		InitializeComponent();

    }

    protected override void OnSizeAllocated(double width, double height)
    {
        FileNameEntry.WidthRequest = 0.9 * width;
    }

    private async void OnPickFileBtnClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Library PDF",
            FileTypes = FilePickerFileType.Pdf
        });

        if (file != null)
        {
            var fullPath = file.FullPath;
            var fileName = file.FileName;
            //UNLESS USER INPUT INTO ENTRY FIELD


            //todo: decide what to do for overwrite
            //File.Copy(fullPath, Path.Combine(Constants.libraryFilePath, fileName), true); //overwrites file if it exists

            //saveCertificationDetails();

            //await Navigation.PushAsync(new DataSaved(2));
        }
    }
}