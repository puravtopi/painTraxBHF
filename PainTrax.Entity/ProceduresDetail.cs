using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class ProcedureDetail
    {
        public long Procedure_ID { get; set; }
        public long PatientIE_ID { get; set; }
        public long? PatientFU_ID { get; set; }
        public long ProcedureDetail_ID { get; set; }
        public string MCODE { get; set; }
        public string BodyPart { get; set; }
        public DateTime? Date { get; set; }
        public string DateType { get; set; }
        public string SubProcedure { get; set; }
        public long? CreatedBy { get; set; }

    }
}
