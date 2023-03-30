using System.ComponentModel.DataAnnotations;

namespace StudentService.DTO;

public class StudentEnrollDTO
{   
    [Required]
    public long ClassId { get; set; }
    [Required]
    public long StudentId { get; set; }
}