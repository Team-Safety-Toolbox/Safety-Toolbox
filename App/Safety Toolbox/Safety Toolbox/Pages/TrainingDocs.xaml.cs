using Safety_Toolbox.Pages;

namespace Safety_Toolbox;

public partial class TrainingDocs : ContentPage
{
    string folder = "Training Documents";
    List<string> docs = new List<string>();
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
        var fullFilePath = Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), folder, filename);

        if (File.Exists(fullFilePath))
        {
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    void displayFiles()
    {
        if(Preferences.Default.Get("LibFilePath", "Not Found") != "Not Found")
        {
            string[] libFilePaths = Directory.GetFiles(Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), folder));
            List<string> libFiles = new List<string>();
            foreach (string libFilePath in libFilePaths)
            {
                libFiles.Add(Path.GetFileName(libFilePath));
            }
            collectionView.ItemsSource = libFiles;
            docs = libFiles;
        }
        else
        {
            List<string> libFiles = new List<string>();
            collectionView.ItemsSource = libFiles;
            docs = libFiles;
            FilePathWarning.IsVisible = true;
        }
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        if (searchBar != null)
        {
            var matches = docs.Where(docs => docs.ToLower().Contains(searchBar.Text.ToLower()));

            collectionView.ItemsSource = matches;
        }
    }
}