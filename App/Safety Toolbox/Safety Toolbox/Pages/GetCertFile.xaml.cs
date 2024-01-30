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
            trainedDisplay = "no train date";
        }
        else
        {
            trainedDisplay = certificationData.TrainedOnDate.Value.ToShortDateString();
        }
        if (!certificationData.ExpiryDate.HasValue)
        {
            expireDisplay = "no expire date";
        }
        else
        {
            expireDisplay = certificationData.ExpiryDate.Value.ToShortDateString();
        }

        filenameLabel.Text = filename;
        trainLabel.Text = trainedDisplay;
        expiryLabel.Text = expireDisplay;
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

            File.Copy(fullPath, Path.Combine(Constants.certificationFilePath, CertificationData.FileName), true); //overwrites file if it exists

            //update sql table
            string queryInsert = "INSERT INTO Certifications Values(@EmpId, @CertType, @TrainDate, @ExpireDate);";
            string queryUpdate = "UPDATE Certifications SET TrainedOnDate = @TrainDate, ExpiryDate = @ExpireDate WHERE EmployeeID = @EmpId AND CertType = @CertType;";


            using (SqlConnection connection = new SqlConnection(Constants.connectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand(queryInsert, connection))
                    {
                        //local vars because once they've been added here in the try, they can't be readded in catch
                        var empIdParam = new SqlParameter("EmpId", CertificationData.EmployeeId);
                        var certTypeParam = new SqlParameter("CertType", CertificationData.CertType);
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
                catch {
                    using (SqlCommand command = new SqlCommand(queryUpdate, connection))
                    {
                        //local vars because once they were added in the try, they can't be readded in catch
                        var empIdParam = new SqlParameter("EmpId", CertificationData.EmployeeId);
                        var certTypeParam = new SqlParameter("CertType", CertificationData.CertType);
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
                await Navigation.PushAsync(new FileSaved());
        }
    }
}