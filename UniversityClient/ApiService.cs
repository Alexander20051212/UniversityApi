using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace UniversityClient;

// ======================================================
// Собственный тип делегата для обработки событий API
// ======================================================
public delegate void OnRequestCompleted(string endpoint, string method, int statusCode, long elapsedMs);

// Модели
public record Student(int Id, string Name, string Group, string Email);
public record Teacher(int Id, string Name, string Department);
public record Course(int Id, string Title, int TeacherId);
public record Grade(int Id, int StudentId, int CourseId, int Value, DateTime Date);

public class ApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    // ======================================================
    // Многоадресный делегат — событие завершения запроса
    // ======================================================
    public event OnRequestCompleted? RequestCompleted;

    // ======================================================
    // Action<T> — операция без возвращаемого значения
    // ======================================================
    public Action<string> OnError = message =>
        Console.ForegroundColor = ConsoleColor.Red;

    // ======================================================
    // Func<T, TResult> — форматирование результата
    // ======================================================
    public Func<int, string, string> FormatResult = (statusCode, body) =>
        $"[HTTP {statusCode}] {body}";

    public ApiService(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    }

    // Универсальный метод выполнения запроса с замером времени
    private async Task<(int StatusCode, string Body)> SendAsync(
        HttpMethod method, string path, object? body = null)
    {
        var sw = Stopwatch.StartNew();
        var url = _baseUrl + path;

        var request = new HttpRequestMessage(method, url);
        if (body != null)
        {
            var json = JsonSerializer.Serialize(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response;
        string responseBody;
        try
        {
            response = await _http.SendAsync(request);
            responseBody = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            sw.Stop();
            OnError($"Ошибка запроса {method} {path}: {ex.Message}");
            Console.ResetColor();
            RequestCompleted?.Invoke(path, method.Method, 0, sw.ElapsedMilliseconds);
            return (0, ex.Message);
        }

        sw.Stop();
        RequestCompleted?.Invoke(path, method.Method, (int)response.StatusCode, sw.ElapsedMilliseconds);
        return ((int)response.StatusCode, responseBody);
    }

    // ======================================================
    // CRUD через Func<> и Action<> — параметризованные вызовы
    // ======================================================
    public async Task<string> ExecuteGet(string path)
    {
        Func<string, Task<(int, string)>> getOp = p => SendAsync(HttpMethod.Get, p);
        var (code, body) = await getOp(path);
        return FormatResult(code, body);
    }

    public async Task<string> ExecutePost(string path, object data)
    {
        Func<string, object, Task<(int, string)>> postOp = (p, d) => SendAsync(HttpMethod.Post, p, d);
        var (code, body) = await postOp(path, data);
        return FormatResult(code, body);
    }

    public async Task<string> ExecutePut(string path, object data)
    {
        Func<string, object, Task<(int, string)>> putOp = (p, d) => SendAsync(HttpMethod.Put, p, d);
        var (code, body) = await putOp(path, data);
        return FormatResult(code, body);
    }

    public async Task<string> ExecuteDelete(string path)
    {
        Func<string, Task<(int, string)>> deleteOp = p => SendAsync(HttpMethod.Delete, p);
        var (code, body) = await deleteOp(path);
        return FormatResult(code, body);
    }
}
