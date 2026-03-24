using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/grades
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
    {
        return await _context.Grades.ToListAsync();
    }

    // GET: api/grades/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Grade>> GetGrade(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
            return NotFound();

        return grade;
    }

    // POST: api/grades
    [HttpPost]
    public async Task<ActionResult<Grade>> CreateGrade(Grade grade)
    {
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
    }

    // PUT: api/grades/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrade(int id, Grade grade)
    {
        if (id != grade.Id)
            return BadRequest();

        _context.Entry(grade).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GradeExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/grades/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
            return NotFound();

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GradeExists(int id)
    {
        return _context.Grades.Any(e => e.Id == id);
    }
}