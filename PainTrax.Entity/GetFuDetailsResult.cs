using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class GetFuDetailsResult
    {
        public int PatientId { get; set; }
        public int PatientIEId { get; set; }
        public int PatientFUId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public DateTime DOE { get; set; }
        public string Location { get; set; }
        public string MAProviders { get; set; }
        public string PrintStatus { get; set; }
        public DateTime DOA { get; set; }
        public DateTime DOEIE { get; set; }
        public string Compensation  { get; set; }
        public string PrintStatusRod { get; set; }
    }
}
