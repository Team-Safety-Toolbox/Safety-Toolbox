namespace Safety_Toolbox;
using Safety_Toolbox.Types;
using Microsoft.Data.SqlClient;
using Safety_Toolbox.Pages;

public partial class Notes : ContentPage
{ 
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

    private void AddFileButtonClicked(object sender, EventArgs e)
    {

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