using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Prometheus;
using UniversityApi.Data;
using UniversityApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("redis_cache:6379,abortConnect=false"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Авто-миграция и seeding при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Teachers.Any())
    {
        db.Teachers.AddRange(
            new Teacher { Name = "Иванов Алексей Петрович", Department = "Информатика" },
            new Teacher { Name = "Сидорова Мария Николаевна", Department = "Математика" },
            new Teacher { Name = "Петров Дмитрий Сергеевич", Department = "Физика" }
        );
        db.SaveChanges();
    }

    if (!db.Courses.Any())
    {
        db.Courses.AddRange(
            new Course { Title = "Базы данных", TeacherId = 1 },
            new Course { Title = "Высшая математика", TeacherId = 2 },
            new Course { Title = "Механика", TeacherId = 3 },
            new Course { Title = "Алгоритмы и структуры данных", TeacherId = 1 }
        );
        db.SaveChanges();
    }

    if (!db.Students.Any())
    {
        db.Students.AddRange(
            new Student { Name = "Козлов Иван Андреевич", Group = "ИТ-21", Email = "kozlov@uni.ru" },
            new Student { Name = "Новикова Анна Сергеевна", Group = "ИТ-21", Email = "novikova@uni.ru" },
            new Student { Name = "Морозов Кирилл Олегович", Group = "МТ-22", Email = "morozov@uni.ru" },
            new Student { Name = "Белова Юлия Викторовна", Group = "МТ-22", Email = "belova@uni.ru" }
        );
        db.SaveChanges();
    }

    if (!db.Grades.Any())
    {
        db.Grades.AddRange(
            new Grade { StudentId = 1, CourseId = 1, Value = 5, Date = DateTime.UtcNow.AddDays(-10) },
            new Grade { StudentId = 1, CourseId = 2, Value = 4, Date = DateTime.UtcNow.AddDays(-8) },
            new Grade { StudentId = 2, CourseId = 1, Value = 3, Date = DateTime.UtcNow.AddDays(-7) },
            new Grade { StudentId = 3, CourseId = 3, Value = 5, Date = DateTime.UtcNow.AddDays(-5) },
            new Grade { StudentId = 4, CourseId = 4, Value = 4, Date = DateTime.UtcNow.AddDays(-3) }
        );
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpMetrics();

app.MapControllers();
app.MapMetrics();

app.Run();