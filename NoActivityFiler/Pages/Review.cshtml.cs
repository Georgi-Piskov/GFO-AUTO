using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoActivityFiler.Models;
using NoActivityFiler.Services;
using NoActivityFiler.Utilities;

namespace NoActivityFiler.Pages;

public class ReviewModel : PageModel
{
    private const string SessionFormKey = "form-model";
    private const string SessionResultKey = "gen-result";
    private readonly SessionJsonSerializer _sessionJson;
    private readonly IN8nClient _client;

    public ReviewModel(SessionJsonSerializer sessionJson, IN8nClient client)
    {
        _sessionJson = sessionJson;
        _client = client;
    }

    [BindProperty]
    public GenerateRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        var saved = _sessionJson.Get<GenerateRequest>(HttpContext.Session, SessionFormKey);
        if (saved == null)
        {
            return RedirectToPage("/Index");
        }
        Input = saved;
        return Page();
    }

    public async Task<IActionResult> OnPostGenerateAsync()
    {
        var saved = _sessionJson.Get<GenerateRequest>(HttpContext.Session, SessionFormKey);
        if (saved == null)
        {
            return RedirectToPage("/Index");
        }

        // Guard: validate server-side before calling n8n
        TryValidateModel(saved);
        if (!ModelState.IsValid)
        {
            Input = saved;
            ErrorMessage = "Моля, коригирайте грешките и опитайте отново.";
            return Page();
        }

        try
        {
            var result = await _client.GenerateAsync(saved);
            _sessionJson.Set(HttpContext.Session, SessionResultKey, result);
            return RedirectToPage("/Download");
        }
        catch (Exception ex)
        {
            Input = saved;
            ErrorMessage = "Възникна грешка при генерирането. Моля, опитайте отново.";
            HttpContext.Items["gen-error"] = ex.Message;
            return Page();
        }
    }
}

