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
        public string EmployeeName { get; set; }
        public string CertType { get; set; }
        public DateTime ExpiryDate { get; set; }

        public CertificationData(){}

        public CertificationData(int EmployeeID_, string EmployeeName_, string CertType_, DateTime ExpiryDate_)
        {
            EmployeeID = EmployeeID_;
            EmployeeName = EmployeeName_;
            CertType = CertType_;
            ExpiryDate = ExpiryDate_;
        }
    }
}
