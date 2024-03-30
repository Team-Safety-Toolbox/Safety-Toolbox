using Microsoft.IdentityModel.Tokens;

namespace Safety_Toolbox.Pages;

public partial class AddLibraryFile : ContentPage
{
    private bool validEntryFileName {  get; set; }
    private bool takenEntryFileName { get; set; }
    private string newFileCurrentPath { get; set; }
    private string folder {  get; set; }

	public AddLibraryFile(string saveToFolder)
	{
		InitializeComponent();
        folder = saveToFolder;
        validEntryFileName = true;
        takenEntryFileName = false;

        FileFeedback.IsVisible = false;

        if (Preferences.Default.Get("LibFilePath", "Not Found") == "Not Found")
        {
            FilePathWarning.IsVisible = true;
            FilePickerBtn.IsEnabled = false;
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        FileNameEntry.WidthRequest = 0.7 * width;
    }

    //private void EvaluateFileName()
    //{
    //    validEntryFileName = true;
    //    takenEntryFileName = false;
    //    FileFeedback.IsVisible = false;

    //    if(Preferences.Default.Get("LibFilePath", "Not Found") == "Not Found"){
    //        FilePathWarning.IsVisible = true;
    //    }

    //    if (FileNameEntry.Text.IndexOfAny(Path.GetInvalidFileNameChars()) == -1) { //index of any returns -1 if none are found
    //        validEntryFileName = true;
    //        if (File.Exists(Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), folder, FileNameEntry.Text))) { 
    //            takenEntryFileName = true;
    //            FileFeedback.Text = "This filename is taken.";
    //            FileFeedback.IsVisible = true;
    //        }
    //    }
    //    else
    //    {
    //        validEntryFileName = false;
    //        FileFeedback.Text = "This filename is invalid.";
    //        FileFeedback.IsVisible = true;
    //    }
    //}

    private async void OnPickFileBtnClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Library PDF",
            FileTypes = FilePickerFileType.Pdf
        });

        if (file != null)
        {
            newFileCurrentPath = file.FullPath;
            var fileName = file.FileName;

            FileNameEntry.Text = fileName;
            FileConfirmArea.IsVisible = true;
        }
        
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        string fileName = "";
        if (Path.GetExtension(FileNameEntry.Text) == "")
        {
            fileName = FileNameEntry.Text + Path.GetExtension(newFileCurrentPath);
        }
        else
        {
            fileName = FileNameEntry.Text;
        }
        if (File.Exists(Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), folder, fileName)))
        {
            FileFeedback.Text = "File with this name already exists.";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else if(FileNameEntry.Text.IsNullOrEmpty())
        {
            FileFeedback.Text = "File needs a name.";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else
        {
           
            File.Copy(newFileCurrentPath, Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), folder, fileName));

            FileFeedback.Text = "Success!";
            FileFeedback.TextColor = Color.Parse("Green");
            FileFeedback.IsVisible = true;

            FileConfirmArea.IsVisible = false;
            FileNameEntry.Text = "";
            newFileCurrentPath = "";
        }
    }

    private void CancelButtonClicked(object sender, EventArgs e)
    {
        FileConfirmArea.IsVisible = false;
        FileNameEntry.Text = "";
        newFileCurrentPath = "";
    }
}