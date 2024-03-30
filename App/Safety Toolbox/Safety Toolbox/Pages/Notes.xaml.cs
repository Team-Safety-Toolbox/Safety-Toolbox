namespace Safety_Toolbox;
using Safety_Toolbox.Types;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Pages;
using CommunityToolkit.Maui.Core.Primitives;
using Microsoft.Maui.Storage;

public partial class Notes : ContentPage
{
    private string newFileFullPath;
    public Notes()
	{
		InitializeComponent();

        getTextNotes();
        getFileNotes();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        ScrollNotes.HeightRequest = 0.7 * height;
        TextNotesArea.WidthRequest = 0.4 * width;
        FileNotesArea.WidthRequest = 0.4 * width;
    }

    private void getTextNotes()
    {
        List<TextNoteData> textNotes = new List<TextNoteData>();

        string query = "SELECT * FROM Notes";

        using (SqlConnection connection = new SqlConnection(Constants.connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime noteDate = reader.GetDateTime(0);
                        string noteContent = reader.GetString(1);

                        textNotes.Add(new TextNoteData(noteDate, noteContent));
                    }
                }
            }
        }

        List<TextNoteData> sortedtextNotes = textNotes.OrderByDescending(o => o.NoteDate).ToList();
        ViewTextNotes.ItemsSource = sortedtextNotes;
    }

    private void getFileNotes()
    {
        if (Preferences.Default.Get("NotesFilePath", "Not Found") != "Not Found")
        {
            string[] notesFilePaths = Directory.GetFiles(Path.Combine(Preferences.Default.Get("NotesFilePath", "Not Found")));

            List<FileNoteData> fileNotes = new List<FileNoteData>();

            foreach (string noteFilePath in notesFilePaths)
            {
                fileNotes.Add(new FileNoteData(File.GetLastWriteTime(noteFilePath), Path.GetFileName(noteFilePath)));
            }
            List<FileNoteData> sortedFileNotes = fileNotes.OrderByDescending(o => o.NoteDate).ToList();
            ViewFileNotes.ItemsSource = sortedFileNotes;
        }
        else
        {
            List<string> notesFiles = new List<string>();
            ViewFileNotes.ItemsSource = notesFiles;
            FilePathWarning.IsVisible = true;
        }
    }

    private void NewNoteTextChanged(object sender, TextChangedEventArgs e)
    {
        if (NewNote.Text.ToCharArray().Count() > 500)
        {
            CharLimitWarning.IsVisible = true;
            AddNoteButton.IsEnabled = false;
        }
        else
        {
            CharLimitWarning.IsVisible = false;
            AddNoteButton.IsEnabled = true;
        }
    }

    private void OnAddNoteButtonClicked(object sender, EventArgs e)
    {
        if (NewNote.Text != "" && NewNote.Text != null)
        {
            string query = "Insert into Notes Values (@NoteDate, @NoteContent);";

            using (SqlConnection connection = new SqlConnection(Constants.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var noteDateParam = new SqlParameter("NoteDate", DateTime.Now);
                    var noteContentParam = new SqlParameter("NoteContent", NewNote.Text);
                    command.Parameters.Add(noteDateParam);
                    command.Parameters.Add(noteContentParam);

                    var results = command.ExecuteReader();

                }
            }

            //refresh text list
            getTextNotes();
        }
    }

    async private void AddFileButtonClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Notes PDF",
            FileTypes = FilePickerFileType.Pdf
        });

        if (file != null)
        {
            newFileFullPath = file.FullPath;
            var fileName = file.FileName;

            ConfirmFileButton.CommandParameter = fileName;

            FileNameDisplay.Text = fileName;
            SaveFileLabel.IsVisible = true;
            FileNameDisplay.IsVisible = true;
            ConfirmFileButton.IsVisible = true;
            CancelFileButton.IsVisible = true;
            NewFileLabel.IsVisible = false;
            AddFileButton.IsVisible = false;
        }
    }
    private void ConfirmFileButtonClicked(object sender, EventArgs e)
    {

        if(File.Exists(Path.Combine(Preferences.Default.Get("NotesFilePath", "Not Found"), FileNameDisplay.Text)))
        {
            FileFeedback.Text = "File with this name already exists.";
            FileFeedback.TextColor = Color.Parse("Red");
            FileFeedback.IsVisible = true;
        }
        else
        {
            //TODO: what do if file exists? give user a warning and chance to rename?
            //this is a problem elsewhere too, but not on certs page for sure
            //**fixed here, check other places with File.Copy
            File.Copy(newFileFullPath, Path.Combine(Preferences.Default.Get("NotesFilePath", "Not Found"), FileNameDisplay.Text));
            getFileNotes();
            FileFeedback.Text = "File has been added!";
            FileFeedback.TextColor = Color.Parse("Green");
            FileFeedback.IsVisible = true;

            newFileFullPath = "";
            FileNameDisplay.Text = "";
            SaveFileLabel.IsVisible = false;
            FileNameDisplay.IsVisible = false;
            ConfirmFileButton.IsVisible = false;
            CancelFileButton.IsVisible = false;
            NewFileLabel.IsVisible = true;
            AddFileButton.IsVisible = true;
        }
       
    }

    private void CancelFileButtonClicked(object sender, EventArgs e)
    {
        newFileFullPath = "";
        FileNameDisplay.Text = "";
        SaveFileLabel.IsVisible = false;
        FileNameDisplay.IsVisible = false;
        ConfirmFileButton.IsVisible = false;
        CancelFileButton.IsVisible = false;
        NewFileLabel.IsVisible = true;
        AddFileButton.IsVisible = true;
    }

    async private void OnViewButtonClicked (object sender, EventArgs e)
    {
        var button = (Button)sender;
        string filename = button.CommandParameter.ToString();
        var fullFilePath = Path.Combine(Preferences.Default.Get("NotesFilePath", "Not Found"), filename);

        if (File.Exists(fullFilePath))
        {
            await Navigation.PushAsync(new FileViewer(fullFilePath, filename));
        }
    }
}