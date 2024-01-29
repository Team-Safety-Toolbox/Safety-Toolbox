using Microsoft.Maui;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Safety_Toolbox.Pages;

public partial class GetCertFile : ContentPage
{
    
    String FileName { get; set; }
	public GetCertFile(String employee, String certType, DateTime? trained, DateTime? expires)
	{
        InitializeComponent();
        
        String separator = "-";
        String fileType = ".pdf";
        String trainedDisplay = "";
        String expireDisplay = "";
        if (!trained.HasValue) {
            trainedDisplay = "noTrainDate";
        }
        else
        {
            trainedDisplay = trained.Value.ToShortDateString();
        }
        if (!expires.HasValue)
        {
            expireDisplay = "noExpireDate";
        }
        else
        {
            expireDisplay = expires.Value.ToShortDateString();
        }

        String filename = employee + separator + certType + separator + trainedDisplay + separator + expireDisplay + fileType;

        FileName = filename;
        filenameLabel.Text = filename;
    }

    private async void OnFilePickerBtnClicked(object sender, EventArgs e)
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Certification PDF",
            FileTypes = FilePickerFileType.Pdf
        });

        if (file != null)
        {
            var fileName = file.FileName;
            var fullPath = file.FullPath;
            var fileStream = await file.OpenReadAsync();

            //delete existing file
            File.Copy(fullPath, Path.Combine(Constants.certificationFilePath, FileName));

            //push a file saved screen? and then a back to cert page (but actually remove the pages)
            //await Navigation.PopAsync();
        }
    }

    void OnSubmitBtnClicked(object sender, EventArgs e)
    {
        //save the file
        //update sql table
    }
}