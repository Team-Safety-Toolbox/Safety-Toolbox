namespace Safety_Toolbox;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using System.Threading;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();

        setEntryFields();
    }

    private void setEntryFields()
    {
        DBConnStr.Text = Preferences.Default.Get("DBConn", "Not Found");
        ReportServerURL.Text = Preferences.Default.Get("ReportServerURL", "Not Found");
        CertFilePath.Text = Preferences.Default.Get("CertFilePath", "Not Found");
        LibFilePath.Text = Preferences.Default.Get("LibFilePath", "Not Found");
        NotesFilePath.Text = Preferences.Default.Get("NotesFilePath", "NotFound");
        SignUpToggleSwitch.IsToggled = Preferences.Default.Get("SignUpEnabled", false);
    }

    private void OnSaveBtnClicked(object sender, EventArgs e)
    {
        Saved.IsVisible = false;

        Preferences.Default.Set("DBConn", DBConnStr.Text);
        Preferences.Default.Set("ReportServerURL", ReportServerURL.Text);
        Preferences.Default.Set("CertFilePath", CertFilePath.Text);
        Preferences.Default.Set("LibFilePath", LibFilePath.Text);
        Preferences.Default.Set("NotesFilePath", NotesFilePath.Text);
        Preferences.Default.Set("SignUpEnabled", SignUpToggleSwitch.IsToggled);

        setEntryFields();
        Saved.IsVisible = true;
        dbInfo.IsVisible = true;
    }

    private void OnCertPathBtnClicked(object sender, EventArgs e)
    {
        launchFolderPicker("certPath");
    }
    private void OnLibPathBtnClicked(object sender, EventArgs e)
    {
        launchFolderPicker("libPath");
    }

    private void OnNotesPathBtnClicked(object sender, EventArgs e)
    {
        launchFolderPicker("notesPath");
    }

    private async void launchFolderPicker(string path)
    {
        CancellationToken cancellationToken = new CancellationToken();
        var result = await FolderPicker.Default.PickAsync(cancellationToken);
        if (result.IsSuccessful)
        {
            if (path == "certPath")
            {
                Preferences.Default.Set("CertFilePath", result.Folder.Path);
            }
            else if (path == "libPath")
            {
                Preferences.Default.Set("LibFilePath", result.Folder.Path);
            }
            else //notesPath
            {
                Preferences.Default.Set("NotesFilePath", result.Folder.Path);
            }

            CertFilePath.Text = Preferences.Default.Get("CertFilePath", "Not Found");
            LibFilePath.Text = Preferences.Default.Get("LibFilePath", "Not Found");
            NotesFilePath.Text = Preferences.Default.Get("NotesFilePath", "NotFound");
        }

    }
}