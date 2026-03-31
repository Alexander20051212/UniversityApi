using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UniversityApi.Data;
using UniversityApi.Models;
using System.Linq;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDatabase _redis;

    public StudentsController(AppDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis.GetDatabase();
    }

    // GET: api/students (с кэшем)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        var cacheKey = "students";

        var cachedData = await _redis.StringGetAsync(cacheKey);

        if (!cachedData.IsNullOrEmpty)
        {
            var students = JsonSerializer.Deserialize<List<Student>>(cachedData);
            return students;
        }

        var data = await _context.Students.OrderBy(s => s.Id).ToListAsync();

        var json = JsonSerializer.Serialize(data);

        await _redis.StringSetAsync(cacheKey, json, TimeSpan.FromSeconds(60));

        return data;
    }

    // GET: api/students/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        return student;
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Очистка кэша
        await _redis.KeyDeleteAsync("students");

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    // PUT: api/students/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
            return BadRequest();

        _context.Entry(student).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Очистка кэша
        await _redis.KeyDeleteAsync("students");

        return NoContent();
    }

    // DELETE: api/students/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        // Очистка кэша
        await _redis.KeyDeleteAsync("students");

        return NoContent();
    }
}