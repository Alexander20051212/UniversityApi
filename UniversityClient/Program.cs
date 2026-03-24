using UniversityClient;

const string BaseUrl = "http://localhost:80"; // через Nginx

var api = new ApiService(BaseUrl);

// ======================================================
// Обработчик 1: вывод в консоль
// ======================================================
OnRequestCompleted consoleHandler = (endpoint, method, statusCode, elapsedMs) =>
{
    var color = statusCode >= 200 && statusCode < 300
        ? ConsoleColor.Green
        : ConsoleColor.Yellow;
    Console.ForegroundColor = color;
    Console.WriteLine($"  >> {method} {endpoint} | Статус: {statusCode} | Время: {elapsedMs} мс");
    Console.ResetColor();
};

// ======================================================
// Обработчик 2: логирование в файл
// ======================================================
var logFile = "api_log.txt";
OnRequestCompleted fileHandler = (endpoint, method, statusCode, elapsedMs) =>
{
    var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {method} {endpoint} | {statusCode} | {elapsedMs}ms";
    File.AppendAllText(logFile, line + Environment.NewLine);
};

// Подключаем оба обработчика к многоадресному делегату
api.RequestCompleted += consoleHandler;
api.RequestCompleted += fileHandler;

Console.WriteLine("=== University API Client ===");
Console.WriteLine($"Лог пишется в файл: {Path.GetFullPath(logFile)}");
Console.WriteLine();

// ======================================================
// Операция 1: Получить список студентов
// ======================================================
Console.WriteLine("--- 1. Получение списка студентов ---");
var result = await api.ExecuteGet("/api/students");
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Операция 2: Создать нового студента
// ======================================================
Console.WriteLine("--- 2. Создание нового студента ---");
var newStudent = new { Name = "Тестов Тест Тестович", Group = "ИТ-23", Email = "test@uni.ru" };
result = await api.ExecutePost("/api/students", newStudent);
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Операция 3: Получить студента по ID
// ======================================================
Console.WriteLine("--- 3. Получение студента по ID=1 ---");
result = await api.ExecuteGet("/api/students/1");
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Отключаем обработчик логирования после 3 операций
// ======================================================
Console.WriteLine(">>> Отключаем логирование в файл (fileHandler -= ...)");
api.RequestCompleted -= fileHandler;
Console.WriteLine();

// ======================================================
// Операция 4: Обновить студента
// ======================================================
Console.WriteLine("--- 4. Обновление студента ID=1 (файл логироваться НЕ должен) ---");
var updated = new { Id = 1, Name = "Козлов Иван Андреевич", Group = "ИТ-21", Email = "kozlov_updated@uni.ru" };
result = await api.ExecutePut("/api/students/1", updated);
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Операция 5: Получить список курсов (кэш Redis)
// ======================================================
Console.WriteLine("--- 5. Получение списка курсов (с Redis-кэшем) ---");
result = await api.ExecuteGet("/api/courses");
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Операция 6: Получить оценки
// ======================================================
Console.WriteLine("--- 6. Получение списка оценок ---");
result = await api.ExecuteGet("/api/grades");
Console.WriteLine(result);
Console.WriteLine();

// ======================================================
// Операция 7: Удалить созданного тестового студента
// ======================================================
Console.WriteLine("--- 7. Удаление тестового студента ---");
// Сначала найдём последнего — получим список и возьмём максимальный ID
var studentsJson = await api.ExecuteGet("/api/students");
Console.WriteLine(studentsJson);
Console.WriteLine();

Console.WriteLine("=== Готово! ===");
Console.WriteLine($"Проверь файл лога: {Path.GetFullPath(logFile)}");
Console.WriteLine("Первые 3 операции должны быть в логе, операции 4-7 — только в консоли.");
