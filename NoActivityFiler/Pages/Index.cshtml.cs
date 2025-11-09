using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoActivityFiler.Models;
using NoActivityFiler.Utilities;

namespace NoActivityFiler.Pages;

public class IndexModel : PageModel
{
    private const string SessionKey = "form-model";
    private readonly SessionJsonSerializer _sessionJson;

    public IndexModel(SessionJsonSerializer sessionJson)
    {
        _sessionJson = sessionJson;
    }

    [BindProperty]
    public GenerateRequest Input { get; set; } = new();

    [BindProperty]
    public int Step { get; set; } = 1;

    public List<string> LegalForms { get; } = new() { "ЕООД", "ООД", "ЕТ", "АД", "СД", "КД", "КДА", "КООп", "Друго" };
    public List<string> Ownerships { get; } = new() { "Private", "State", "Municipal", "Mixed" };

    public void OnGet()
    {
        var saved = _sessionJson.Get<GenerateRequest>(HttpContext.Session, SessionKey);
        if (saved != null)
        {
            Input = saved;
        }
    }

    public IActionResult OnPost(string? action)
    {
        if (string.Equals(action, "Back", StringComparison.OrdinalIgnoreCase))
        {
            Step = 1;
            return Page();
        }

        // Validate before moving forward
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _sessionJson.Set(HttpContext.Session, SessionKey, Input);

        if (string.Equals(action, "Next", StringComparison.OrdinalIgnoreCase))
        {
            Step = 2;
            return Page();
        }

        // default: go to review
        return RedirectToPage("/Review");
    }
}

