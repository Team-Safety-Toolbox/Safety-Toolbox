using Microsoft.Maui.Controls;
using Safety_Toolbox.Pages;
namespace Safety_Toolbox;

public partial class WorkProcedures : ContentPage
{
    string Folder = "Work Procedures";
    List<string> Procedures = new List<string>();
    public WorkProcedures()
	{
		InitializeComponent();


        // configure the readonly controls
        if (MainPage.IsReadOnly)
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
        await Navigation.PushAsync(new AddLibraryFile(Folder));
    }
    void OnRefreshBtnClicked(object sender, EventArgs e)
    {
        displayFiles();
    }

    async void OnViewBtnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), Folder, filename);

        if (File.Exists(fullFilePath))
        {
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }

    void displayFiles()
    {
        if (Preferences.Default.Get("LibFilePath", "Not Found") != "Not Found")
        {
            string[] libFilePaths = Directory.GetFiles(Path.Combine(Preferences.Default.Get("LibFilePath", "Not Found"), Folder));
            List<string> libFiles = new List<string>();
            foreach (string libFilePath in libFilePaths)
            {
                libFiles.Add(Path.GetFileName(libFilePath));
            }

            collectionView.ItemsSource = libFiles;
            Procedures = libFiles;
        }
        else {
            List<string> libFiles = new List<string>();
            collectionView.ItemsSource = libFiles;
            Procedures = libFiles;
            FilePathWarning.IsVisible = true;
        }
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        if (searchBar != null)
        {
            var matches = Procedures.Where(procedures => procedures.ToLower().Contains(searchBar.Text.ToLower()));

            collectionView.ItemsSource = matches;
        }
    }
}