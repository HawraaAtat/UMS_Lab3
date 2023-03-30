using System.ComponentModel.DataAnnotations;

namespace UMS_Lab3.DTO;

public class UpdateCourseDTO
{
    [Required]
    public long Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int? MaxStudentsNumber { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
}