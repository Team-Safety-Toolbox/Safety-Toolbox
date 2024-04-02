using Microsoft.IdentityModel.Tokens;

namespace Safety_Toolbox.Pages;

public partial class AddLibraryFile : ContentPage
{
    private string NewFileCurrentPath { get; set; }
    private string Folder {  get; set; }

	public AddLibraryFile(string saveToFolder)
	{
		InitializeComponent();
        Folder = saveToFolder;

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

    private async void OnPickFileBtnClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Library PDF",
            FileTypes = FilePickerFileType.Pdf
        });

        if (file != null)
        {
            NewFileCurrentPath = file.FullPath;
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
            fileName = FileNameEntry.Text + Path.GetExtension(NewFileCurrentPath);
        }
        else
        {
            fileName = FileNameEntry.Text;
        }
        if (File.Exists(Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), Folder, fileName)))
        {
            FileFeedback.Text = "File with this name already exists.";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1) { //index of any returns -1 if none are found
            FileFeedback.Text = "File name cannot contain invalid chars";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else if (FileNameEntry.Text.IsNullOrEmpty())
        {
            FileFeedback.Text = "File needs a name.";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else
        {

            File.Copy(NewFileCurrentPath, Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), Folder, fileName));

            FileFeedback.Text = "Success!";
            FileFeedback.TextColor = Color.Parse("Green");
            FileFeedback.IsVisible = true;

            FileConfirmArea.IsVisible = false;
            FileNameEntry.Text = "";
            NewFileCurrentPath = "";
        }
    }

    private void CancelButtonClicked(object sender, EventArgs e)
    {
        FileConfirmArea.IsVisible = false;
        FileNameEntry.Text = "";
        NewFileCurrentPath = "";
    }
}