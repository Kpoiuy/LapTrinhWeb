using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BookManagerWinForms.Dtos;

namespace BookManagerWinForms.Services;

public class BookApiClient
{
    private readonly HttpClient _httpClient;

    public BookApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:9999/"),
            Timeout = TimeSpan.FromSeconds(20)
        };
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/categories");
        await EnsureSuccessWithMessageAsync(response);
        var data = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        return data ?? new List<CategoryDto>();
    }

    public async Task<List<BookDto>> SearchBooksAsync(string? keyword, int? categoryId)
    {
        var queryParts = new List<string>();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            queryParts.Add($"keyword={Uri.EscapeDataString(keyword.Trim())}");
        }

        if (categoryId.HasValue)
        {
            queryParts.Add($"categoryId={categoryId.Value}");
        }

        var url = "api/books";
        if (queryParts.Count > 0)
        {
            url += "?" + string.Join("&", queryParts);
        }

        var response = await _httpClient.GetAsync(url);
        await EnsureSuccessWithMessageAsync(response);
        var data = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        return data ?? new List<BookDto>();
    }

    public async Task<BookDto> CreateBookAsync(BookCreateInput input)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(input.Title), "Title");
        content.Add(new StringContent(input.Author), "Author");
        content.Add(new StringContent(input.Year.ToString()), "Year");
        content.Add(new StringContent(input.Publisher), "Publisher");
        content.Add(new StringContent(input.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Price");
        content.Add(new StringContent(input.CategoryId.ToString()), "CategoryId");

        if (!string.IsNullOrWhiteSpace(input.ImageFilePath) && File.Exists(input.ImageFilePath))
        {
            var bytes = await File.ReadAllBytesAsync(input.ImageFilePath);
            var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, "ImageFile", Path.GetFileName(input.ImageFilePath));
        }

        var response = await _httpClient.PostAsync("api/books", content);
        await EnsureSuccessWithMessageAsync(response);

        var created = await response.Content.ReadFromJsonAsync<BookDto>();
        return created ?? throw new InvalidOperationException("Không đọc được dữ liệu trả về từ API.");
    }

    private static async Task EnsureSuccessWithMessageAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var raw = await response.Content.ReadAsStringAsync();
        string message = raw;

        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("message", out var msg))
            {
                message = msg.GetString() ?? raw;
            }
        }
        catch
        {
            // keep raw
        }

        throw new HttpRequestException($"API Error {(int)response.StatusCode}: {message}");
    }
}

public class BookCreateInput
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public string? ImageFilePath { get; set; }
}
