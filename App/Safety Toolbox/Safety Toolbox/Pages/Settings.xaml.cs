namespace Safety_Toolbox;

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
}