using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    public class CertificationData
    {
        public int EmployeeID { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string CertType { get; set; }
        public DateTime ExpiryDate { get; set; }

        public CertificationData(){}

        public CertificationData(int EmployeeID_, string EmployeeFirstName_, string EmployeeLastName_, string CertType_, DateTime ExpiryDate_)
        {
            EmployeeID = EmployeeID_;
            EmployeeFirstName = EmployeeFirstName_;
            EmployeeLastName = EmployeeLastName_;
            CertType = CertType_;
            ExpiryDate = ExpiryDate_;
        }

        public override string ToString()
        {
            return EmployeeFirstName + " " + EmployeeLastName;
        }
    }
}
