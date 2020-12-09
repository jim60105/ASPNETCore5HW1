using System;
using System.Collections.Generic;

#nullable disable

namespace ASPNETCore5HW1.Models
{
    public partial class CourseInstructor
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Person Instructor { get; set; }
    }
}
