using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class PatientsByDOE_Result
    {
        public Int64 PatientIEId { get; set; }
        public Int64 PatientFUId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ExamType { get; set; }
        public DateTime? DOA { get; set; }
        public DateTime DOE { get; set; }
        public string Compensation { get; set; }
    }
}
