using Microsoft.IdentityModel.Tokens;

namespace Safety_Toolbox.Pages;

public partial class AddLibraryFile : ContentPage
{
    public bool validEntryFileName {  get; set; }
    public bool takenEntryFileName { get; set; }
    public string folder {  get; set; }

	public AddLibraryFile(string saveToFolder)
	{
		InitializeComponent();
        folder = saveToFolder;
        validEntryFileName = true;
        takenEntryFileName = false;

        FileNameErrorText.IsVisible = false;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        FileNameEntry.WidthRequest = 0.9 * width;
    }

    private void EvaluateFileName(object sender, EventArgs e)
    {
        validEntryFileName = true;
        takenEntryFileName = false;
        FileNameErrorText.IsVisible = false;

        if (FileNameEntry.Text.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) { //index of any returns -1 if none are found
            validEntryFileName = true;
            if (File.Exists(Path.Combine(Constants.libraryFilePath, folder, FileNameEntry.Text))) { 
                takenEntryFileName = true;
                FileNameErrorText.Text = "This filename is taken.";
                FileNameErrorText.IsVisible = true;
            }
        }
        else
        {
            validEntryFileName = false;
            FileNameErrorText.Text = "This filename is invalid.";
            FileNameErrorText.IsVisible = true;
        }
    }

    private async void OnPickFileBtnClicked(object sender, EventArgs e)
    {
        if(validEntryFileName && !takenEntryFileName)
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
                var fileExt = Path.GetExtension(file.FileName);

                if (!FileNameEntry.Text.IsNullOrEmpty())
                {
                    if (Path.GetExtension(FileNameEntry.Text) == "")
                    {
                        fileName = FileNameEntry.Text + fileExt;
                    }
                    else
                    {
                        fileName = FileNameEntry.Text;
                    }
                }

                File.Copy(fullPath, Path.Combine(Constants.libraryFilePath, folder, fileName));

                await Navigation.PushAsync(new DataSaved(2));
            }
        }
    }
}