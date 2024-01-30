using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    public class CertificationData
    {
        public int EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string CertType { get; set; }
        public DateTime? TrainedOnDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string FileName { get; set; }

        public bool FileFound { get; set; }

        public CertificationData(){}

        public CertificationData(int EmployeeId_, string EmployeeFirstName_, string EmployeeLastName_, string CertType_, DateTime? TrainedOnDate_, DateTime? ExpiryDate_)
        {
            EmployeeId = EmployeeId_;
            EmployeeFirstName = EmployeeFirstName_;
            EmployeeLastName = EmployeeLastName_;
            CertType = CertType_;
            TrainedOnDate = TrainedOnDate_;
            ExpiryDate = ExpiryDate_;

            FileName = buildFileName();
            FileFound = findFile();
        }

        private string buildFileName()
        {
            string separator = "-";
            string fileType = ".pdf";

            string filename = EmployeeId + separator + FullNameToString() + separator + CertType + fileType;
            return filename;
        }

        private bool findFile()
        {
            return File.Exists(getFullFilePath());
        }

        public string FullNameToString()
        {
            return EmployeeFirstName + " " + EmployeeLastName;
        }

        public string getFullFilePath() { 
            return Path.Combine(Constants.certificationFilePath, FileName);
        }
    }
}
