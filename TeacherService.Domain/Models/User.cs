using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TeacherService.Domain.Models
{
    [Serializable]
    public partial class User
    {
        public User()
        {
            TeacherPerCourses = new HashSet<TeacherPerCourse>();
        }

        [JsonIgnore]
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        [JsonIgnore]
        public long RoleId { get; set; }

        public string KeycloakId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; set; }
    }
}
