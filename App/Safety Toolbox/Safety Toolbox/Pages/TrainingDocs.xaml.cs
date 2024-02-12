using Safety_Toolbox.Pages;

namespace Safety_Toolbox;

public partial class TrainingDocs : ContentPage
{
    public TrainingDocs()
	{
		InitializeComponent();


        // configure the readonly controls
        if (MainPage.isReadOnly)
        {
            AddFileBtn.IsEnabled = false;
        }

        displayFiles();
    }

    
    private async void OnAddFileBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddLibraryFile());
    }
    void OnRefreshBtnClicked(object sender, EventArgs e)
    {
        displayFiles();
    }

    async void OnViewBtnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Constants.libraryFilePath, filename);

        if (File.Exists(fullFilePath))
        {
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    void displayFiles()
    {
        string[] libFilePaths = Directory.GetFiles(Constants.libraryFilePath);
        List<string> libFiles = new List<string>();
        foreach (string libFilePath in libFilePaths)
        {
            libFiles.Add(Path.GetFileName(libFilePath));
        }
        collectionView.ItemsSource = libFiles;
    }

}