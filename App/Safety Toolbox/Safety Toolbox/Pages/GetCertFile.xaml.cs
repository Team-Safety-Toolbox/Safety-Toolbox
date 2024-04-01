using Microsoft.Data.SqlClient;
using Microsoft.Maui;
using Safety_Toolbox.Types;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Safety_Toolbox.Pages;

public partial class GetCertFile : ContentPage
{
    CertificationData CertificationData { get; set; }

    public GetCertFile(CertificationData certificationData)
	{
        InitializeComponent();
        CertificationData = certificationData;

        string filename = certificationData.FileName;

        string trainedDisplay = "";
        string expireDisplay = "";
        if (!certificationData.TrainedOnDate.HasValue) {
            trainedDisplay = "no training date";
        }
        else
        {
            trainedDisplay = certificationData.TrainedOnDate.Value.ToShortDateString();
        }
        if (!certificationData.ExpiryDate.HasValue)
        {
            expireDisplay = "no expiry date";
        }
        else
        {
            expireDisplay = certificationData.ExpiryDate.Value.ToShortDateString();
        }

        filenameLabel.Text = filename;
        trainLabel.Text = trainedDisplay;
        expiryLabel.Text = expireDisplay;

        if (Preferences.Default.Get("CertFilePath", "Not Found") == "Not Found") {
            FilePathWarning.IsVisible = true;
            FilePickerBtn.IsEnabled = false;
        }
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
            var fullPath = file.FullPath;

            File.Copy(fullPath, Path.Combine(Preferences.Default.Get("CertFilePath", "Not Found"), CertificationData.FileName), true); //overwrites file if it exists
            
            saveCertificationDetails();
            
            await Navigation.PushAsync(new DataSaved(3));
        }
    }

    private async void OnNoFileBtnClicked(object sender, EventArgs e)
    {
        saveCertificationDetails();
        await Navigation.PushAsync(new DataSaved(3));
    }

    async private void saveCertificationDetails()
    {
        string query = "SELECT CertificationID FROM CertificationTypes WHERE CertificationName = @CertName";
        int certId = -1;

        try
        {
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    var certNameParam = new SqlParameter("CertName", CertificationData.CertType);
                    command.Parameters.Add(certNameParam);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            certId = reader.GetInt32(0);
                        }
                    }
                }
            }
        }
        catch
        {
            await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
        }


        //update sql table
        string queryInsert = "INSERT INTO Certifications Values(@EmpId, @CertId, @TrainDate, @ExpireDate);";
        string queryUpdate = "UPDATE Certifications SET TrainedOnDate = @TrainDate, ExpiryDate = @ExpireDate WHERE EmployeeID = @EmpId AND CertificationID = @CertId;";
        
        try { 
            using (SqlConnection connection = new SqlConnection(Preferences.Default.Get("DBConn", "Not Found")))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(queryInsert, connection))
                    {
                        //local vars because once they've been added here in the try, they can't be readded in catch
                        var empIdParam = new SqlParameter("EmpId", CertificationData.EmployeeId);
                        var certTypeParam = new SqlParameter("CertId", certId);
                        var trainDateParam = new SqlParameter("TrainDate", CertificationData.TrainedOnDate);
                        var expireDateParam = new SqlParameter("ExpireDate", CertificationData.ExpiryDate);

                        if (CertificationData.TrainedOnDate == null)
                        {
                            trainDateParam.Value = DBNull.Value;
                        }
                        if (CertificationData.ExpiryDate == null)
                        {
                            expireDateParam.Value = DBNull.Value;
                        }

                        command.Parameters.Add(empIdParam);
                        command.Parameters.Add(certTypeParam);
                        command.Parameters.Add(trainDateParam);
                        command.Parameters.Add(expireDateParam);

                        var results = command.ExecuteReader();
                    }
                }
                catch
                {
                    using (SqlCommand command = new SqlCommand(queryUpdate, connection))
                    {
                        //local vars because once they were added in the try, they can't be readded in catch
                        var empIdParam = new SqlParameter("EmpId", CertificationData.EmployeeId);
                        var certTypeParam = new SqlParameter("CertId", certId);
                        var trainDateParam = new SqlParameter("TrainDate", CertificationData.TrainedOnDate);
                        var expireDateParam = new SqlParameter("ExpireDate", CertificationData.ExpiryDate);

                        if (CertificationData.TrainedOnDate == null)
                        {
                            trainDateParam.Value = DBNull.Value;
                        }
                        if (CertificationData.ExpiryDate == null)
                        {
                            expireDateParam.Value = DBNull.Value;
                        }

                        command.Parameters.Add(empIdParam);
                        command.Parameters.Add(certTypeParam);
                        command.Parameters.Add(trainDateParam);
                        command.Parameters.Add(expireDateParam);

                        var results = command.ExecuteReader();
                    }
                }
            }
        }
        catch
        {
            await DisplayAlert("Database Connection", "There was a problem connecting to the database.", "OK");
        }

    }

 }