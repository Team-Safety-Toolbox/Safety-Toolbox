using Microsoft.Maui.Controls;
using Safety_Toolbox.Pages;
namespace Safety_Toolbox;

public partial class WorkProcedures : ContentPage
{
    string folder = "Work Procedures";
    public WorkProcedures()
	{
		InitializeComponent();


        // configure the readonly controls
        if (MainPage.isReadOnly)
        {
            AddFileBtn.IsEnabled = false;
        }

        displayFiles();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        MainFileDisplay.Height = 0.7 * height;
    }

    private async void OnAddFileBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddLibraryFile(folder));
    }
    void OnRefreshBtnClicked(object sender, EventArgs e)
    {
        displayFiles();
    }

    async void OnViewBtnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Constants.libraryFilePath, folder, filename);

        if (File.Exists(fullFilePath))
        {
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    void displayFiles()
    {
        string[] libFilePaths = Directory.GetFiles(Path.Combine(Constants.libraryFilePath, folder));
        List<string> libFiles = new List<string>();
        foreach (string libFilePath in libFilePaths)
        {
            libFiles.Add(Path.GetFileName(libFilePath));
        }

        collectionView.ItemsSource = libFiles;
    }
}