using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class LabelBindings
    {
        public string DiagCervialBulgeDate { get; set; }
        public string DiagThoracicBulgeDate { get; set; }
        public string DiagLumberBulgeDate { get; set; }

        public DateTime Other1Date { get; set; }
        public DateTime Other2Date { get; set; }

        public DateTime Other1Study { get; set; }
        public DateTime Other2Study { get; set; }
    }
}
