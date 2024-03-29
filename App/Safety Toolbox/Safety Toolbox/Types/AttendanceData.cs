﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safety_Toolbox.Types
{
    public class AttendanceData
    {
        public int EmployeeID { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public bool Present { get; set; }
        public bool Excused { get; set; }
        public bool Absent { get; set; }

        public AttendanceData()
        {
        }

        public AttendanceData(int EmployeeID_, string EmployeeFirstName_, string EmployeeLastName_, DateTime? AttendanceDate_, bool Present_, bool Excused_, bool Absent_)
        {
            EmployeeID = EmployeeID_;
            EmployeeFirstName = EmployeeFirstName_;
            EmployeeLastName = EmployeeLastName_;
            AttendanceDate = AttendanceDate_;
            Present = Present_;
            Excused = Excused_;
            Absent = Absent_;
        }
    }
}
