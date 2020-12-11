using System;
using System.Collections.Generic;

#nullable disable

namespace ASPNETCore5HW1.Models
{
    public partial class PersonEditVM
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string Discriminator { get; set; }
    }
}
