using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox
{
    public class Constants // make this static?
    {
        //TODO: get these from a text file or a settings page/table so they are editable by an admin user
        public const string connectionString = "Server=DESKTOP-0LUMUS9;Database=SafetyToolBox;Persist Security Info=False;Integrated Security=true;Encrypt=False;";
        public const string reportServerURL = "/reports/report";
        //public const string certificationFilePath = "C:\\Users\\Yoga3\\Desktop\\TestCertifications";
        //public const string libraryFilePath = "C:\\Users\\Yoga3\\Desktop\\TestLibrary";

        //public const string connectionString = "Server=MIKAYLAS-LAPTOP;Database=SafetyToolBox;Persist Security Info=False;Integrated Security=true;Encrypt=False;";
        //public const string reportServerURL = "/reports/report";
        //public const string certificationFilePath = "D:\\safetytbfiles";
    }
}
