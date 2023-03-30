using System;
using System.Collections.Generic;

namespace TeacherService.Domain.Models
{
    public partial class User
    {
        public User()
        {
            TeacherPerCourses = new HashSet<TeacherPerCourse>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long RoleId { get; set; }
        public string KeycloakId { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; set; }
    }
}
