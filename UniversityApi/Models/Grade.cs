namespace UniversityApi.Models;

public class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public int CourseId { get; set; }

    public int Value { get; set; }
    public DateTime Date { get; set; }

}