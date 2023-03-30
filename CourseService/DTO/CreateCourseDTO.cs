using System.ComponentModel.DataAnnotations;

namespace CourseService.DTO;

public class CreateCourseDTO
{  
    [Required]
    public string? Name { get; set; }
    [Required]
    public int? MaxStudentsNumber { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
}