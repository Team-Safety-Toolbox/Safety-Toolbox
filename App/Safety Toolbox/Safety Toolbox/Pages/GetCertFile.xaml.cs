using Microsoft.Data.SqlClient;
using Microsoft.Maui;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Safety_Toolbox.Pages;

public partial class GetCertFile : ContentPage
{
    string FileName { get; set; }

    string EmpName { get; set; }
    string CertType { get; set; }
    int EmpId { get; set; }
    DateTime? TrainDate { get; set; }
    DateTime? ExpireDate { get; set; }
    public GetCertFile(int empId, string employee, string certType, DateTime? trained, DateTime? expires)
	{
        InitializeComponent();

        EmpId = empId;
        EmpName = employee;
        CertType = certType;
        TrainDate = trained;
        ExpireDate = expires;
        
        string separator = "-";
        string fileType = ".pdf";
        string trainedDisplay = "";
        string expireDisplay = "";
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
            var fullPath = file.FullPath;

            File.Copy(fullPath, Path.Combine(Constants.certificationFilePath, FileName), true); //overwrites file if it exists

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
                        var empIdParam = new SqlParameter("EmpId", EmpId);
                        var certTypeParam = new SqlParameter("CertType", CertType);
                        var trainDateParam = new SqlParameter("TrainDate", TrainDate);
                        var expireDateParam = new SqlParameter("ExpireDate", ExpireDate);

                        if (TrainDate == null)
                        {
                            trainDateParam.Value = DBNull.Value;
                        }
                        if (ExpireDate == null)
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
                        var empIdParam = new SqlParameter("EmpId", EmpId);
                        var certTypeParam = new SqlParameter("CertType", CertType);
                        var trainDateParam = new SqlParameter("TrainDate", TrainDate);
                        var expireDateParam = new SqlParameter("ExpireDate", ExpireDate);

                        if (TrainDate == null)
                        {
                            trainDateParam.Value = DBNull.Value;
                        }
                        if (ExpireDate == null)
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