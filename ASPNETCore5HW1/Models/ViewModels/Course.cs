using System;
using System.Collections.Generic;

#nullable disable

namespace ASPNETCore5HW1.Models
{
    public partial class CourseEditVM
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
    }
}
