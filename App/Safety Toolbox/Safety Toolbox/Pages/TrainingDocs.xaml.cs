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