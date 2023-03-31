using System;
using System.Collections.Generic;

namespace CourseService.Domain.Models
{
    public partial class Tenant
    {
        public Tenant()
        {
            Courses = new HashSet<Course>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Domain { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
    }
}
