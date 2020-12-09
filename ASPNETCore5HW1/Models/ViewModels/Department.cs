using System;
using System.Collections.Generic;

#nullable disable

namespace ASPNETCore5HW1.Models
{
    public partial class DepartmentEditVM
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorId { get; set; }
    }
}
