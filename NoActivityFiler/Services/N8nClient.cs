using System.Net.Http.Json;
using System.Text.Json.Serialization;
using NoActivityFiler.Models;

namespace NoActivityFiler.Services;

public class N8nClient : IN8nClient
{
    private readonly HttpClient _http;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public N8nClient(HttpClient http, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    private record N8nResponse(
        [property: JsonPropertyName("app11PdfBase64")] string App11PdfBase64,
        [property: JsonPropertyName("zschPdfBase64")] string ZSchPdfBase64
    );

    public async Task<GenerateResult> GenerateAsync(GenerateRequest request, CancellationToken ct = default)
    {
        var httpResponse = await _http.PostAsJsonAsync("", request, ct);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var body = await httpResponse.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException($"n8n error: {(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}. Body: {body}");
        }

        var data = await httpResponse.Content.ReadFromJsonAsync<N8nResponse>(cancellationToken: ct)
                   ?? throw new InvalidOperationException("Empty n8n response");

        var app11Bytes = Convert.FromBase64String(data.App11PdfBase64);
        var zschBytes = Convert.FromBase64String(data.ZSchPdfBase64);

        var guid = Guid.NewGuid().ToString("N");
        var tempVirtual = $"/tmp/{guid}";
        var tempPhysical = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "tmp", guid);
        Directory.CreateDirectory(tempPhysical);

        var app11Path = Path.Combine(tempPhysical, "Приложение_11-НСИ.pdf");
        var zschPath = Path.Combine(tempPhysical, "Декларация_38_9_2.pdf");
        await File.WriteAllBytesAsync(app11Path, app11Bytes, ct);
        await File.WriteAllBytesAsync(zschPath, zschBytes, ct);

        var app11Url = CombineVirtual(tempVirtual, "Приложение_11-НСИ.pdf");
        var zschUrl = CombineVirtual(tempVirtual, "Декларация_38_9_2.pdf");

        return new GenerateResult
        {
            App11PdfUrl = app11Url,
            ZSchPdfUrl = zschUrl,
            App11PdfPath = app11Path,
            ZSchPdfPath = zschPath,
            TempFolderVirtual = tempVirtual,
            TempFolderPhysical = tempPhysical
        };
    }

    private static string CombineVirtual(string a, string b)
        => (a.TrimEnd('/') + "/" + b).Replace("\\", "/");
}

