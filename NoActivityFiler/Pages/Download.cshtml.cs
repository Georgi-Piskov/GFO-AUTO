using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoActivityFiler.Models;
using NoActivityFiler.Services;
using NoActivityFiler.Utilities;

namespace NoActivityFiler.Pages;

public class DownloadModel : PageModel
{
    private const string SessionFormKey = "form-model";
    private const string SessionResultKey = "gen-result";
    private readonly SessionJsonSerializer _sessionJson;
    private readonly EmailTemplateService _email;

    public DownloadModel(SessionJsonSerializer sessionJson, EmailTemplateService email)
    {
        _sessionJson = sessionJson;
        _email = email;
    }

    public GenerateRequest? Input { get; set; }
    public GenerateResult? Result { get; set; }
    public string EmailBody { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        Input = _sessionJson.Get<GenerateRequest>(HttpContext.Session, SessionFormKey);
        Result = _sessionJson.Get<GenerateResult>(HttpContext.Session, SessionResultKey);
        if (Input == null || Result == null)
        {
            return RedirectToPage("/Index");
        }
        EmailBody = _email.BuildNsiEmailBody(Input);
        return Page();
    }
}

