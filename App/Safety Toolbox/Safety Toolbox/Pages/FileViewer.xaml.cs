namespace Safety_Toolbox.Pages;

public partial class FileViewer : ContentPage
{
	public FileViewer(string fullFilePath, string fileName)
	{
		InitializeComponent();

        FileName.Text = fileName;
        PdfWebView.Source = new UrlWebViewSource { Url = fullFilePath };
	}

	protected override void OnSizeAllocated(double width, double height)
	{
        PdfWebView.HeightRequest = 0.8 * height;
    }
}