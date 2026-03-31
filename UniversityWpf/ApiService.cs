using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniversityWpf;

// Модели
public class Student
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("group")] public string Group { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
}

public class Teacher
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("department")] public string Department { get; set; } = "";
}

public class Course
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("teacherId")] public int TeacherId { get; set; }
}

public class Grade
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("studentId")] public int StudentId { get; set; }
    [JsonPropertyName("courseId")] public int CourseId { get; set; }
    [JsonPropertyName("value")] public int Value { get; set; }
    [JsonPropertyName("date")] public DateTime Date { get; set; }
}

public class ApiService
{
    private static readonly HttpClient Http = new() { BaseAddress = new Uri("http://localhost") };
    private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

    private async Task<T?> Get<T>(string path)
    {
        var res = await Http.GetAsync(path);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, Opts);
    }

    private async Task<HttpResponseMessage> Send(HttpMethod method, string path, object body)
    {
        var json = JsonSerializer.Serialize(body);
        var req = new HttpRequestMessage(method, path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        return await Http.SendAsync(req);
    }

    // Students
    public Task<List<Student>?> GetStudents() => Get<List<Student>>("/api/students");
    public Task<HttpResponseMessage> CreateStudent(Student s) => Send(HttpMethod.Post, "/api/students", s);
    public Task<HttpResponseMessage> UpdateStudent(Student s) => Send(HttpMethod.Put, $"/api/students/{s.Id}", s);
    public Task<HttpResponseMessage> DeleteStudent(int id) => Http.DeleteAsync($"/api/students/{id}");

    // Teachers
    public Task<List<Teacher>?> GetTeachers() => Get<List<Teacher>>("/api/teachers");
    public Task<HttpResponseMessage> CreateTeacher(Teacher t) => Send(HttpMethod.Post, "/api/teachers", t);
    public Task<HttpResponseMessage> UpdateTeacher(Teacher t) => Send(HttpMethod.Put, $"/api/teachers/{t.Id}", t);
    public Task<HttpResponseMessage> DeleteTeacher(int id) => Http.DeleteAsync($"/api/teachers/{id}");

    // Courses
    public Task<List<Course>?> GetCourses() => Get<List<Course>>("/api/courses");
    public Task<HttpResponseMessage> CreateCourse(Course c) => Send(HttpMethod.Post, "/api/courses", c);
    public Task<HttpResponseMessage> UpdateCourse(Course c) => Send(HttpMethod.Put, $"/api/courses/{c.Id}", c);
    public Task<HttpResponseMessage> DeleteCourse(int id) => Http.DeleteAsync($"/api/courses/{id}");

    // Grades
    public Task<List<Grade>?> GetGrades() => Get<List<Grade>>("/api/grades");
    public Task<HttpResponseMessage> CreateGrade(Grade g) => Send(HttpMethod.Post, "/api/grades", g);
    public Task<HttpResponseMessage> DeleteGrade(int id) => Http.DeleteAsync($"/api/grades/{id}");
}
