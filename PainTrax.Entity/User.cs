using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntakeSheet.Entity
{
    public class User
    {
        public long UserId { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EmailId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
