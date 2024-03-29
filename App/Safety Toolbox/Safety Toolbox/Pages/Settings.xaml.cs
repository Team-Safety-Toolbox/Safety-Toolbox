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
        //TODO: error checking for cert and lib when there is not yet a path saved here
        DBConnStr.Text = Preferences.Default.Get("DBConn", "Not Found");
        ReportServerURL.Text = Preferences.Default.Get("ReportServerURL", "Not Found");
        CertFilePath.Text = Preferences.Default.Get("CertFilePath", "Not Found");
        LibFilePath.Text = Preferences.Default.Get("LibFilePath", "Not Found");

    }

    private void OnSaveBtnClicked(object sender, EventArgs e)
    {
        Saved.IsVisible = false;

        Preferences.Default.Set("DBConn", DBConnStr.Text);
        Preferences.Default.Set("ReportServerURL", ReportServerURL.Text);
        Preferences.Default.Set("CertFilePath", CertFilePath.Text);
        Preferences.Default.Set("LibFilePath", LibFilePath.Text);

        setEntryFields();
        Saved.IsVisible = true;
    }

    private void OnCertPathBtnClicked(object sender, EventArgs e)
    {
        launchFolderPicker(true);
    }
    private void OnLibPathBtnClicked(object sender, EventArgs e)
    {
        launchFolderPicker(false);
    }

    private async void launchFolderPicker(bool certPath)
    {
        CancellationToken cancellationToken = new CancellationToken();
        var result = await FolderPicker.Default.PickAsync(cancellationToken);
        if (result.IsSuccessful)
        {
            if (certPath)
            {
                Preferences.Default.Set("CertFilePath", result.Folder.Path);
                
            }
            else //libPath
            {
                Preferences.Default.Set("LibFilePath", result.Folder.Path);
            }

            CertFilePath.Text = Preferences.Default.Get("CertFilePath", "Not Found");
            LibFilePath.Text = Preferences.Default.Get("LibFilePath", "Not Found");
        }

    }
}