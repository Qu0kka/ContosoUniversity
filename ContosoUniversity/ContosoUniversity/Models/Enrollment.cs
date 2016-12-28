using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public int decimal? Grade { get; set; } //nullable (значние может быть пустым)
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}