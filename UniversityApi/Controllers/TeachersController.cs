using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeachersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
    {
        return await _context.Teachers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Teacher>> GetTeacher(int id)
    {
        var Teacher = await _context.Teachers.FindAsync(id);

        if (Teacher == null)
            return NotFound();

        return Teacher;
    }

    [HttpPost]
    public async Task<ActionResult<Teacher>> CreateTeacher(Teacher Teacher)
    {
        _context.Teachers.Add(Teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeacher), new { id = Teacher.Id }, Teacher);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, Teacher Teacher)
    {
        if (id != Teacher.Id)
            return BadRequest();

        _context.Entry(Teacher).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var Teacher = await _context.Teachers.FindAsync(id);

        if (Teacher == null)
            return NotFound();

        _context.Teachers.Remove(Teacher);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}