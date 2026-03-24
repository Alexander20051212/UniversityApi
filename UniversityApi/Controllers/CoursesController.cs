using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDatabase _redis;

    public CoursesController(AppDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis.GetDatabase();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        const string cacheKey = "courses";

        var cachedData = await _redis.StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            var cached = JsonSerializer.Deserialize<List<Course>>(cachedData!);
            return cached;
        }

        var data = await _context.Courses.ToListAsync();
        await _redis.StringSetAsync(cacheKey, JsonSerializer.Serialize(data), TimeSpan.FromSeconds(60));
        return data;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound();
        return course;
    }

    [HttpPost]
    public async Task<ActionResult<Course>> CreateCourse(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("courses");
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, Course course)
    {
        if (id != course.Id)
            return BadRequest();

        _context.Entry(course).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("courses");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("courses");
        return NoContent();
    }
}