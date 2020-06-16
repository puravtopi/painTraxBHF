using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class Procedure
    {
        public Int64 ProcedureId { get; set; }
        public string MCode { get; set; }
        public string DateType { get; set; }
        public string BodyPart { get; set; }
        public string Heading { get; set; }
        public string CCDesc { get; set; }
        public string PEDesc { get; set; }
        public string ADesc { get; set; }
        public string PDesc { get; set; }
        public bool CF { get; set; }
        public bool PN { get; set; }
    }
}
